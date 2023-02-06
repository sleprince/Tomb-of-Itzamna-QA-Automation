using System.Collections.Generic;
using UnityEngine;

public class MapLocation
{
    public int x;
    public int z;

    public MapLocation(int _x, int _z)
    {
        x = _x;
        z = _z;
    }
    public Vector2 ToVector()
    {
        return new Vector2(x, z);
    }

    public static MapLocation operator +(MapLocation a, MapLocation b)
       => new MapLocation(a.x + b.x, a.z + b.z);

    public override bool Equals(object obj)
    {
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else
        {
            return x == ((MapLocation)obj).x && z == ((MapLocation)obj).z;
        }
    }

    public override int GetHashCode()
    {
        return 0;
    }
}

public class Maze : MonoBehaviour
{
    public List<MapLocation> directions = new List<MapLocation>()
    {
        new MapLocation(1,0),
        new MapLocation(0,1),
        new MapLocation(-1,0),
        new MapLocation(0,-1)
    };
    public List<MapLocation> pillarLocations = new List<MapLocation>();

    public int width = 30;      //x length
    public int depth = 30;      //z length
    public int scale = 6;
    public int level = 0;
    public float levelDistance = 2.0f;
    public float xOffset = 0;
    public float zOffset = 0;

    public byte[,] map;

    [System.Serializable]
    public struct Module
    {
        public GameObject prefab;
        public Vector3 rotation;
    }

    public Module VerticalStraight;
    public Module HorizontalStraight;
    public Module Crossroad;
    public Module NortheastCorner;
    public Module SoutheastCorner;
    public Module NorthwestCorner;
    public Module SouthwestCorner;
    public Module TJunction;
    public Module UpsideDownTJunction;
    public Module EastTJunction;
    public Module WestTJunction;
    public Module DeadEnd;
    public Module UpsideDownDeadEnd;
    public Module EastDeadEnd;
    public Module WestDeadEnd;

    public Module NorthRoomWall;
    public Module SouthRoomWall;
    public Module EastRoomWall;
    public Module WestRoomWall;
    public Module RoomFloor;
    public Module RoomCeiling;
    public Module Pillar;

    public GameObject player;

    public enum PieceType
    {
        Horizontal_Straight,
        Vertical_Straight,
        North_East_Corner,
        South_East_Corner,
        North_West_Corner,
        South_West_Corner,
        T_Junction,
        UpsideDown_T_Junction,
        East_T_Junction,
        West_T_Junction,
        DeadEnd,
        UpsideDown_DeadEnd,
        East_DeadEnd,
        West_DeadEnd,
        Stairs,
        Wall,
        Crossroad,
        Room
    }

    public struct Pieces
    {
        public PieceType piece;
        public GameObject model;

        public Pieces(PieceType pt, GameObject m)
        {
            piece = pt;
            model = m;
        }
    }

    public Pieces[,] piecePlaces;
    public List<MapLocation> locations = new List<MapLocation>();

    public void Build()
    {
        InitialiseMap();
        GenerateCorridor();
        AddRooms(1, 4, 6);

        byte[,] oldmap = map;
        int oldWidth = width;
        int oldDepth = depth;

        width += 6;
        depth += 6;

        map = new byte[width, depth];
        InitialiseMap();

        for (int z = 0; z < oldDepth; z++)
            for (int x = 0; x < oldWidth; x++)
            {
                map[x + 3, z + 3] = oldmap[x, z];
            }

        int xpos;
        int zpos;


        FindPathAStar astar = GetComponent<FindPathAStar>();
        if (astar != null)
        {
            astar.Build();
            if (astar.startNode.location.x < astar.endNode.location.x) //start is left
            {
                xpos = astar.startNode.location.x;
                zpos = astar.startNode.location.z;

                while (xpos > 1)
                {
                    map[xpos, zpos] = 0;
                    xpos--;
                }

                xpos = astar.endNode.location.x;
                zpos = astar.endNode.location.z;

                while (xpos < width - 2)
                {
                    map[xpos, zpos] = 0;
                    xpos++;
                }
            }
            else
            {
                xpos = astar.startNode.location.x;
                zpos = astar.startNode.location.z;

                while (xpos < width - 2)
                {
                    map[xpos, zpos] = 0;
                    xpos++;
                }

                xpos = astar.endNode.location.x;
                zpos = astar.endNode.location.z;

                while (xpos > 1)
                {
                    map[xpos, zpos] = 0;
                    xpos--;
                }

            }

        }
        else
        {
            //upper vertical corridor
            xpos = Random.Range(5, width - 5);
            zpos = depth - 2;

            while (map[xpos, zpos] != 0 && zpos > 1)
            {
                map[xpos, zpos] = 0;
                zpos--;
            }

            //lower vertical corridor
            xpos = Random.Range(5, width - 5);
            zpos = 1;

            while (map[xpos, zpos] != 0 && zpos < depth - 2)
            {
                map[xpos, zpos] = 0;
                zpos++;
            }

            //right horizontal corridor
            zpos = Random.Range(5, depth - 5);
            xpos = width - 2;

            while (map[xpos, zpos] != 0 && xpos > 1)
            {
                map[xpos, zpos] = 0;
                xpos--;
            }

            //left horizontal corridor
            zpos = Random.Range(5, depth - 5);
            xpos = 1;

            while (map[xpos, zpos] != 0 && xpos < width - 2)
            {
                map[xpos, zpos] = 0;
                xpos++;
            }
        }

        DrawMap();
        
        if (player != null)
            PlacePlayerInMaze();

        PlaceObject placeObject = GetComponent<PlaceObject>();
        if (placeObject != null)
            placeObject.Go();
    }

    public void InitialiseMap()
    {
        map = new byte[width, depth];
        piecePlaces = new Pieces[width, depth];
        for (int z = 0; z < depth; z++)
            for (int x = 0; x < width; x++)
            {
                map[x, z] = 1;                  // 1 for wall
            }
    }

    public virtual void GenerateCorridor()
    {
        for (int z = 0; z < depth; z++)
            for (int x = 0; x < width; x++)
            {
                if (Random.Range(0, 100) < 50)
                    map[x, z] = 0;              // 0 for corridor
            }
    }

    public virtual void AddRooms(int roomCount, int minSize, int maxSize)
    {
       
    }

    void DrawMap()
    {
        int height = (int)(level * scale * levelDistance);

        for (int z = 0; z < depth; z++)
            for (int x = 0; x < width; x++)
            {
                if (map[x, z] == 1)
                {
                    /*Vector3 pos = new Vector3(x * scale, 0, z * scale);
                    GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    wall.transform.localScale = new Vector3(scale, scale, scale);
                    wall.transform.position = pos;*/

                    piecePlaces[x, z].piece = PieceType.Wall;
                    piecePlaces[x, z].model = null;
                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 0, 0, 1, 5, 1, 5 }))                     //horizontal dead end ->|
                {
                    GameObject end = Instantiate(EastDeadEnd.prefab);
                    end.transform.position = new Vector3(x * scale, height, z * scale);
                    end.transform.Rotate(EastDeadEnd.rotation);
                    end.transform.SetParent(this.gameObject.transform);

                    piecePlaces[x, z].piece = PieceType.East_DeadEnd;
                    piecePlaces[x,z].model = end;
                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 1, 0, 0, 5, 1, 5 }))                     //horizontal dead end |<-
                {
                    GameObject end = Instantiate(WestDeadEnd.prefab);
                    end.transform.position = new Vector3(x * scale, height, z * scale);
                    end.transform.SetParent(this.gameObject.transform);

                    piecePlaces[x, z].piece = PieceType.West_DeadEnd;
                    piecePlaces[x, z].model = end;
                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 1, 0, 1, 5, 0, 5 }))                     //vertical dead end T
                {
                    GameObject end = Instantiate(DeadEnd.prefab);
                    end.transform.position = new Vector3(x * scale, height, z * scale);
                    end.transform.Rotate(DeadEnd.rotation);
                    end.transform.SetParent(this.gameObject.transform);

                    piecePlaces[x, z].piece = PieceType.DeadEnd;
                    piecePlaces[x, z].model = end;
                }
                else if (Search2D(x, z, new int[] { 5, 0, 5, 1, 0, 1, 5, 1, 5 }))                     //vertical dead end downT
                {
                    GameObject end = Instantiate(UpsideDownDeadEnd.prefab);
                    end.transform.position = new Vector3(x * scale, height, z * scale);
                    end.transform.Rotate(UpsideDownDeadEnd.rotation);
                    end.transform.SetParent(this.gameObject.transform);

                    piecePlaces[x, z].piece = PieceType.UpsideDown_DeadEnd;
                    piecePlaces[x, z].model = end;
                }
                else if (Search2D(x, z, new int[] { 5, 0, 5, 1, 0, 1, 5, 0, 5 }))                     //vertical straight piece
                {
                    Vector3 pos = new Vector3(x * scale, height, z * scale);
                    GameObject go = Instantiate(VerticalStraight.prefab, pos, Quaternion.identity);
                    go.transform.Rotate(VerticalStraight.rotation);
                    go.transform.SetParent(this.gameObject.transform);

                    piecePlaces[x, z].piece = PieceType.Vertical_Straight;
                    piecePlaces[x, z].model = go;
                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 0, 0, 0, 5, 1, 5 }))                     //horizontal straight piece
                {
                    Vector3 pos = new Vector3(x * scale, height, z * scale);
                    GameObject go = Instantiate(HorizontalStraight.prefab, pos, Quaternion.identity);
                    go.transform.SetParent(this.gameObject.transform);

                    piecePlaces[x, z].piece = PieceType.Horizontal_Straight;
                    piecePlaces[x, z].model = go;
                }
                else if(Search2D(x,z, new int[] { 1, 0, 1, 0, 0, 0, 1, 0, 1 }))                       //crossroad
                {
                    GameObject cross = Instantiate(Crossroad.prefab);
                    cross.transform.position = new Vector3(x * scale, height, z * scale);
                    cross.transform.SetParent(this.gameObject.transform);

                    piecePlaces[x, z].piece = PieceType.Crossroad;
                    piecePlaces[x, z].model = cross;
                }
                else if (Search2D(x, z, new int[] {5, 1, 5, 0, 0, 1, 1, 0, 5 }))                      //upper left corner
                {
                    GameObject go = Instantiate(NorthwestCorner.prefab);
                    go.transform.position = new Vector3(x * scale, height, z * scale);
                    go.transform.Rotate(NorthwestCorner.rotation);
                    go.transform.SetParent(this.gameObject.transform);

                    piecePlaces[x, z].piece = PieceType.North_West_Corner;
                    piecePlaces[x, z].model = go;
                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 1, 0, 0, 5, 0, 1 }))                     //upper right corner
                {
                    GameObject go = Instantiate(NortheastCorner.prefab);
                    go.transform.position = new Vector3(x * scale, height, z * scale);
                    go.transform.Rotate(NortheastCorner.rotation);
                    go.transform.SetParent(this.gameObject.transform);

                    piecePlaces[x, z].piece = PieceType.North_East_Corner;
                    piecePlaces[x, z].model = go;
                }
                else if (Search2D(x, z, new int[] { 5, 0, 1, 1, 0, 0, 5, 1, 5 }))                     //lower right corner
                {
                    GameObject go = Instantiate(SoutheastCorner.prefab);
                    go.transform.position = new Vector3(x * scale, height, z * scale);
                    go.transform.SetParent(this.gameObject.transform);

                    piecePlaces[x, z].piece = PieceType.South_East_Corner;
                    piecePlaces[x, z].model = go;
                }
                else if (Search2D(x, z, new int[] { 1, 0, 5, 5, 0, 1, 5, 1, 5 }))                     //lower left corner
                {
                    GameObject go = Instantiate(SouthwestCorner.prefab);
                    go.transform.position = new Vector3(x * scale, height, z * scale);
                    go.transform.Rotate(SouthwestCorner.rotation);
                    go.transform.SetParent(this.gameObject.transform);

                    piecePlaces[x, z].piece = PieceType.South_West_Corner;
                    piecePlaces[x, z].model = go;
                }
                else if (Search2D(x, z, new int[] { 1, 0, 1, 0, 0, 0, 5, 1, 5 }))                     //upside t junction
                {
                    GameObject junc = Instantiate(UpsideDownTJunction.prefab);
                    junc.transform.position = new Vector3(x * scale, height, z * scale);
                    junc.transform.Rotate(UpsideDownTJunction.rotation);
                    junc.transform.SetParent(this.gameObject.transform);

                    piecePlaces[x, z].piece = PieceType.UpsideDown_T_Junction;
                    piecePlaces[x, z].model = junc;
                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 0, 0, 0, 1, 0, 1 }))                     //t junction
                {
                    GameObject junc = Instantiate(TJunction.prefab);
                    junc.transform.position = new Vector3(x * scale, height, z * scale);
                    junc.transform.Rotate(TJunction.rotation);
                    junc.transform.SetParent(this.gameObject.transform);

                    piecePlaces[x, z].piece = PieceType.T_Junction;
                    piecePlaces[x, z].model = junc;
                }
                else if (Search2D(x, z, new int[] { 1, 0, 5, 0, 0, 1, 1, 0, 5 }))                     //t junction right
                {
                    GameObject junc = Instantiate(EastTJunction.prefab);
                    junc.transform.position = new Vector3(x * scale, height, z * scale);
                    junc.transform.Rotate(0, 180, 0);
                    junc.transform.SetParent(this.gameObject.transform);

                    piecePlaces[x, z].piece = PieceType.East_T_Junction;
                    piecePlaces[x, z].model = junc;
                }
                else if (Search2D(x, z, new int[] { 5, 0, 1, 1, 0, 0, 5, 0, 1 }))                     //t junction left
                {
                    GameObject junc = Instantiate(WestTJunction.prefab);
                    junc.transform.position = new Vector3(x * scale, height, z * scale);
                    junc.transform.SetParent(this.gameObject.transform);

                    piecePlaces[x, z].piece = PieceType.West_T_Junction;
                    piecePlaces[x, z].model = junc;
                }
                else if (map[x,z] == 0 && (CountSquareNeighbours(x,z) > 1 && CountDiagonalNeighbours(x,z) >= 1 ||
                                            CountSquareNeighbours(x,z) >= 1 && CountDiagonalNeighbours(x,z) > 1))
                {
                    GameObject floor = Instantiate(RoomFloor.prefab);
                    floor.transform.position = new Vector3(x * scale, height, z * scale);
                    floor.transform.SetParent(this.gameObject.transform);

                    GameObject ceiling = Instantiate(RoomCeiling.prefab);
                    ceiling.transform.position = new Vector3(x * scale, height, z * scale);
                    ceiling.transform.SetParent(this.gameObject.transform);

                    piecePlaces[x, z].piece = PieceType.Room;
                    piecePlaces[x, z].model = floor;

                    GameObject cornerPillar;
                    LocateWalls(x, z);
                    if (north)
                    {
                        GameObject wall1 = Instantiate(NorthRoomWall.prefab);
                        wall1.transform.position = new Vector3(x * scale, height, z * scale);
                        wall1.name = "North Wall";
                        wall1.transform.SetParent(this.gameObject.transform);

                        if (map[x + 1, z] == 0 && map[x + 1, z + 1] == 0 && !pillarLocations.Contains(new MapLocation(x,z)))
                        {
                            cornerPillar = Instantiate(Pillar.prefab);
                            cornerPillar.transform.position = new Vector3(x * scale, height, z * scale);
                            cornerPillar.transform.localScale = new Vector3(1.01f, 1, 1.01f);
                            cornerPillar.name = "North Right";
                            cornerPillar.transform.SetParent(this.gameObject.transform);
                            pillarLocations.Add(new MapLocation(x, z));
                        }

                        if (map[x - 1, z] == 0 && map[x - 1, z + 1] == 0 && !pillarLocations.Contains(new MapLocation(x - 1, z)))
                        {
                            cornerPillar = Instantiate(Pillar.prefab);
                            cornerPillar.transform.position = new Vector3((x - 1) * scale, height, z * scale);
                            cornerPillar.transform.localScale = new Vector3(1.01f, 1, 1.01f);
                            cornerPillar.name = "North Left";
                            cornerPillar.transform.SetParent(this.gameObject.transform);
                            pillarLocations.Add(new MapLocation(x - 1, z));
                        }
                    }

                    if (south)
                    {
                        GameObject wall2 = Instantiate(SouthRoomWall.prefab);
                        wall2.transform.position = new Vector3(x * scale, height, z * scale);
                        wall2.transform.Rotate(SouthRoomWall.rotation);
                        wall2.name = "South Wall";
                        wall2.transform.SetParent(this.gameObject.transform);

                        if (map[x + 1, z] == 0 && map[x + 1, z - 1] == 0 && !pillarLocations.Contains(new MapLocation(x, z - 1)))
                        {
                            cornerPillar = Instantiate(Pillar.prefab);
                            cornerPillar.transform.position = new Vector3(x * scale, height, (z - 1) * scale);
                            cornerPillar.transform.localScale = new Vector3(1.01f, 1, 1.01f);
                            cornerPillar.name = "South Right";
                            cornerPillar.transform.SetParent(this.gameObject.transform);
                            pillarLocations.Add(new MapLocation(x, z - 1));
                        }

                        if (map[x - 1, z - 1] == 0 && map[x - 1, z] == 0 && !pillarLocations.Contains(new MapLocation(x - 1, z - 1)))
                        {
                            cornerPillar = Instantiate(Pillar.prefab);
                            cornerPillar.transform.position = new Vector3((x - 1) * scale, height,  (z - 1) * scale);
                            cornerPillar.transform.localScale = new Vector3(1.01f, 1, 1.01f);
                            cornerPillar.name = "South Left";
                            cornerPillar.transform.SetParent(this.gameObject.transform);
                            pillarLocations.Add(new MapLocation(x - 1, z - 1));
                        }
                    }

                    if (east)
                    {
                        GameObject wall3 = Instantiate(EastRoomWall.prefab);
                        wall3.transform.position = new Vector3(x * scale, height, z * scale);
                        wall3.transform.Rotate(EastRoomWall.rotation);
                        wall3.name = "East Wall";
                        wall3.transform.SetParent(this.gameObject.transform);

                        if (map[x + 1, z + 1] == 0 && map[x, z + 1] == 0 && !pillarLocations.Contains(new MapLocation(x, z - 1)))
                        {
                            cornerPillar = Instantiate(Pillar.prefab);
                            cornerPillar.transform.position = new Vector3(x * scale, height, (z - 1) * scale);
                            cornerPillar.transform.localScale = new Vector3(1.01f, 1, 1.01f);
                            cornerPillar.name = "East Top";
                            cornerPillar.transform.SetParent(this.gameObject.transform);
                            pillarLocations.Add(new MapLocation(x, z - 1));
                        }

                        if (map[x, z - 1] == 0 && map[x + 1, z - 1] == 0 && !pillarLocations.Contains(new MapLocation(x + 1, z - 1)))
                        {
                            cornerPillar = Instantiate(Pillar.prefab);
                            cornerPillar.transform.position = new Vector3((x + 1) * scale, height, (z - 1) * scale);
                            cornerPillar.transform.localScale = new Vector3(1.01f, 1, 1.01f);
                            cornerPillar.name = "East Bottom";
                            cornerPillar.transform.SetParent(this.gameObject.transform);
                            pillarLocations.Add(new MapLocation(x + 1, z - 1));
                        }
                    }

                    if (west)
                    {
                        GameObject wall4 = Instantiate(WestRoomWall.prefab);
                        wall4.transform.position = new Vector3(x * scale, height, z * scale);
                        wall4.transform.Rotate(WestRoomWall.rotation);
                        wall4.name = "West Wall";
                        wall4.transform.SetParent(this.gameObject.transform);

                        if (map[x - 1, z + 1] == 0 && map[x, z + 1] == 0 && !pillarLocations.Contains(new MapLocation(x - 1, z)))
                        {
                            cornerPillar = Instantiate(Pillar.prefab);
                            cornerPillar.transform.position = new Vector3((x - 1) * scale, height, z * scale);
                            cornerPillar.transform.localScale = new Vector3(1.01f, 1, 1.01f);
                            cornerPillar.name = "West Top";
                            cornerPillar.transform.SetParent(this.gameObject.transform);
                            pillarLocations.Add(new MapLocation(x - 1, z));
                        }

                        if (map[x - 1, z - 1] == 0 && map[x, z - 1] == 0 && !pillarLocations.Contains(new MapLocation(x - 1, z - 1)))
                        {
                            cornerPillar = Instantiate(Pillar.prefab);
                            cornerPillar.transform.position = new Vector3((x - 1) * scale, height, (z - 1) * scale);
                            cornerPillar.transform.localScale = new Vector3(1.01f, 1, 1.01f);
                            cornerPillar.name = "West Bottom";
                            cornerPillar.transform.SetParent(this.gameObject.transform);
                            pillarLocations.Add(new MapLocation(x - 1, z - 1));
                        }
                    }                    
                }
            }
    }

    bool north;
    bool south;
    bool east;
    bool west;

    public void LocateWalls(int x, int z)
    {
        north = false;
        south = false;
        east = false;
        west = false;

        if (x <= 0 || x >= width - 1 || z <= 0 || z >= depth - 1) return;
        if (map[x, z + 1] == 1) north = true;
        if (map[x, z - 1] == 1) south = true;
        if (map[x + 1, z] == 1) east = true;
        if (map[x - 1, z] == 1) west = true;
    }

    public virtual void PlacePlayerInMaze()
    {
        for (int z = 0; z < depth; z++)
            for (int x = 0; x < width; x++)
            {
                if (map[x, z] == 0)
                {
                    player.transform.position = new Vector3(x * scale, 0, z * scale);
                    return;
                }
            }
    }

    bool Search2D(int c, int r, int[] pattern)
    {
        int count = 0;
        int pos = 0;
        for (int z = 1; z > -2; z--)
        {
            for (int x = -1; x < 2; x++)
            {
                if (pattern[pos] == map[c + x, r + z] || pattern[pos] == 5)
                    count++;
                pos++;
            }
        }
        return (count == 9);
    }

    public int CountSquareNeighbours(int x, int z)
    {
        int count = 0;

        if (x <= 0 || x >= width - 1 || z <= 0 || z >= depth - 1) return 5;
        if (map[x - 1, z] == 0) count++;
        if (map[x + 1, z] == 0) count++;
        if (map[x, z - 1] == 0) count++;
        if (map[x, z + 1] == 0) count++;

        return count;
    }

    public int CountDiagonalNeighbours(int x, int z)
    {
        int count = 0;

        if (x <= 0 || x >= width - 1 || z <= 0 || z >= depth - 1) return 5;
        if (map[x - 1, z + 1] == 0) count++;
        if (map[x + 1, z + 1] == 0) count++;
        if (map[x - 1, z - 1] == 0) count++;
        if (map[x + 1, z - 1] == 0) count++;

        return count;
    }

    public int CountAllNeighbours(int x, int z)
    {
        return CountSquareNeighbours(x, z) + CountDiagonalNeighbours(x, z);
    }
}
