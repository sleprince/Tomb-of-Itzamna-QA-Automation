using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class MapLocation
{
    public int x;
    public int z;

    public MapLocation(int _x, int _z)
    {
        x = _x;
        z = _z;
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

    //values to determine the X and Z lengths of maze
    public int width = 30;
    public int depth = 30;
    public int scale = 6;

    public byte[,] map;

    public GameObject straight;
    public GameObject crossroad;
    public GameObject corner;
    public GameObject tSection;
    public GameObject endPiece;
    public GameObject roomWall;
    public GameObject roomFloor;
    public GameObject roomCeiling;
    public GameObject pillar;
    public GameObject door;

    public GameObject player;

    void Start()
    {
        InitialiseMap();
        GenerateCorridor();
        AddRooms(3, 4, 6);
        DrawMap();
        PlacePlayerInMaze();
    }

    void InitialiseMap()
    {
        map = new byte[width, depth];
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
        for (int c = 0; c < roomCount; c++)
        {
            int startX = Random.Range(3, width - 3);
            int startZ = Random.Range(3, depth - 3);
            int roomWidth = Random.Range(minSize, maxSize);
            int roomDepth = Random.Range(minSize, maxSize);

            for (int x = startX; x < width - 3 && x < startX + roomWidth; x++)
            {
                for (int z = startZ; z < depth - 3 && z < startZ + roomDepth; z++)
                {
                    map[x, z] = 0;
                }
            }
        }
    }

    void DrawMap()
    {
        for (int z = 0; z < depth; z++)
            for (int x = 0; x < width; x++)
            {
                if (map[x, z] == 1)
                {
                    /*Vector3 pos = new Vector3(x * scale, 0, z * scale);
                    GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    wall.transform.localScale = new Vector3(scale, scale, scale);
                    wall.transform.position = pos;*/
                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 0, 0, 1, 5, 1, 5 }))                     //horizontal dead end ->|
                {
                    GameObject go = Instantiate(endPiece);
                    go.transform.position = new Vector3(x * scale, 0, z * scale);
                    go.transform.Rotate(0, 180, 0);
                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 1, 0, 0, 5, 1, 5 }))                     //horizontal dead end |<-
                {
                    GameObject go = Instantiate(endPiece);
                    go.transform.position = new Vector3(x * scale, 0, z * scale);
                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 1, 0, 1, 5, 0, 5 }))                     //vertical dead end T
                {
                    GameObject go = Instantiate(endPiece);
                    go.transform.position = new Vector3(x * scale, 0, z * scale);
                    go.transform.Rotate(0, 90, 0);
                }
                else if (Search2D(x, z, new int[] { 5, 0, 5, 1, 0, 1, 5, 1, 5 }))                     //vertical dead end downT
                {
                    GameObject go = Instantiate(endPiece);
                    go.transform.position = new Vector3(x * scale, 0, z * scale);
                    go.transform.Rotate(0, -90, 0);
                }
                else if (Search2D(x, z, new int[] { 5, 0, 5, 1, 0, 1, 5, 0, 5 }))                     //vertical straight piece
                {
                    Vector3 pos = new Vector3(x * scale, 0, z * scale);
                    Instantiate(straight, pos, Quaternion.identity);
                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 0, 0, 0, 5, 1, 5 }))                     //horizontal straight piece
                {
                    Vector3 pos = new Vector3(x * scale, 0, z * scale);
                    GameObject go = Instantiate(straight, pos, Quaternion.identity);
                    go.transform.Rotate(0, 90, 0);
                }
                else if(Search2D(x,z, new int[] { 1, 0, 1, 0, 0, 0, 1, 0, 1 }))                       //crossroad
                {
                    GameObject go = Instantiate(crossroad);
                    go.transform.position = new Vector3(x * scale, 0, z * scale);
                }
                else if (Search2D(x, z, new int[] {5, 1, 5, 0, 0, 1, 1, 0, 5 }))                      //upper left corner
                {
                    GameObject go = Instantiate(corner);
                    go.transform.position = new Vector3(x * scale, 0, z * scale);
                    go.transform.Rotate(0, 180, 0);
                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 1, 0, 0, 5, 0, 1 }))                     //upper right corner
                {
                    GameObject go = Instantiate(corner);
                    go.transform.position = new Vector3(x * scale, 0, z * scale);
                    go.transform.Rotate(0, 90, 0);
                }
                else if (Search2D(x, z, new int[] { 5, 0, 1, 1, 0, 0, 5, 1, 5 }))                     //lower right corner
                {
                    GameObject go = Instantiate(corner);
                    go.transform.position = new Vector3(x * scale, 0, z * scale);
                }
                else if (Search2D(x, z, new int[] { 1, 0, 5, 5, 0, 1, 5, 1, 5 }))                     //lower left corner
                {
                    GameObject go = Instantiate(corner);
                    go.transform.position = new Vector3(x * scale, 0, z * scale);
                    go.transform.Rotate(0, -90, 0);
                }
                else if (Search2D(x, z, new int[] { 1, 0, 1, 0, 0, 0, 5, 1, 5 }))                     //upside t section
                {
                    GameObject go = Instantiate(tSection);
                    go.transform.position = new Vector3(x * scale, 0, z * scale);
                    go.transform.Rotate(0, -90, 0);
                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 0, 0, 0, 1, 0, 1 }))                     //t section
                {
                    GameObject go = Instantiate(tSection);
                    go.transform.position = new Vector3(x * scale, 0, z * scale);
                    go.transform.Rotate(0, 90, 0);
                }
                else if (Search2D(x, z, new int[] { 1, 0, 5, 0, 0, 1, 1, 0, 5 }))                     //t section left
                {
                    GameObject go = Instantiate(tSection);
                    go.transform.position = new Vector3(x * scale, 0, z * scale);
                    go.transform.Rotate(0, 180, 0);
                }
                else if (Search2D(x, z, new int[] { 5, 0, 1, 1, 0, 0, 5, 0, 1 }))                     //t section right
                {
                    GameObject go = Instantiate(tSection);
                    go.transform.position = new Vector3(x * scale, 0, z * scale);
                }
                else if (map[x,z] == 0 && (CountSquareNeighbours(x,z) > 1 && CountDiagonalNeighbours(x,z) >= 1 ||
                                            CountSquareNeighbours(x,z) >= 1 && CountDiagonalNeighbours(x,z) > 1))
                {
                    GameObject floor = Instantiate(roomFloor);
                    floor.transform.position = new Vector3(x * scale, 0, z * scale);

                    GameObject ceiling = Instantiate(roomCeiling);
                    ceiling.transform.position = new Vector3(x * scale, 0, z * scale);

                    GameObject cornerPillar;
                    LocateWalls(x, z);
                    if (north)
                    {
                        GameObject wall1 = Instantiate(roomWall);
                        wall1.transform.position = new Vector3(x * scale, 0, z * scale);
                        wall1.transform.Rotate(0, 90, 0);
                        wall1.name = "North Wall";

                        if (map[x + 1, z] == 0 && map[x + 1, z + 1] == 0 && !pillarLocations.Contains(new MapLocation(x,z)))
                        {
                            cornerPillar = Instantiate(pillar);
                            cornerPillar.transform.position = new Vector3((x + .5f) * scale, 0, (z + .5f) * scale);
                            cornerPillar.name = "North Right";
                            pillarLocations.Add(new MapLocation(x, z));
                        }

                        if (map[x - 1, z] == 0 && map[x - 1, z + 1] == 0 && !pillarLocations.Contains(new MapLocation(x - 1, z)))
                        {
                            cornerPillar = Instantiate(pillar);
                            cornerPillar.transform.position = new Vector3((x - .5f) * scale, 0, (z + .5f) * scale);
                            cornerPillar.name = "North Left";
                            pillarLocations.Add(new MapLocation(x - 1, z));
                        }
                    }

                    if (south)
                    {
                        GameObject wall2 = Instantiate(roomWall);
                        wall2.transform.position = new Vector3(x * scale, 0, z * scale);
                        wall2.transform.Rotate(0, -90, 0);
                        wall2.name = "South Wall";

                        if (map[x + 1, z] == 0 && map[x + 1, z - 1] == 0 && !pillarLocations.Contains(new MapLocation(x, z - 1)))
                        {
                            cornerPillar = Instantiate(pillar);
                            cornerPillar.transform.position = new Vector3((x + .5f) * scale, 0, (z - .5f) * scale);
                            cornerPillar.name = "South Right";
                            pillarLocations.Add(new MapLocation(x, z - 1));
                        }

                        if (map[x - 1, z - 1] == 0 && map[x - 1, z] == 0 && !pillarLocations.Contains(new MapLocation(x - 1, z - 1)))
                        {
                            cornerPillar = Instantiate(pillar);
                            cornerPillar.transform.position = new Vector3((x - .5f) * scale, 0, (z - .5f) * scale);
                            cornerPillar.name = "South Left";
                            pillarLocations.Add(new MapLocation(x - 1, z - 1));
                        }
                    }

                    if (east)
                    {
                        GameObject wall3 = Instantiate(roomWall);
                        wall3.transform.position = new Vector3(x * scale, 0, z * scale);
                        wall3.transform.Rotate(0, 180, 0);
                        wall3.name = "East Wall";

                        if (map[x + 1, z + 1] == 0 && map[x, z + 1] == 0 && !pillarLocations.Contains(new MapLocation(x, z - 1)))
                        {
                            cornerPillar = Instantiate(pillar);
                            cornerPillar.transform.position = new Vector3((x + .5f) * scale, 0, (z + .5f) * scale);
                            cornerPillar.name = "East Top";
                            pillarLocations.Add(new MapLocation(x, z - 1));
                        }

                        if (map[x, z - 1] == 0 && map[x + 1, z - 1] == 0 && !pillarLocations.Contains(new MapLocation(x + 1, z - 1)))
                        {
                            cornerPillar = Instantiate(pillar);
                            cornerPillar.transform.position = new Vector3((x + .5f) * scale, 0, (z - .5f) * scale);
                            cornerPillar.name = "East Bottom";
                            pillarLocations.Add(new MapLocation(x + 1, z - 1));
                        }
                    }

                    if (west)
                    {
                        GameObject wall4 = Instantiate(roomWall);
                        wall4.transform.position = new Vector3(x * scale, 0, z * scale);
                        wall4.name = "West Wall";

                        if (map[x - 1, z + 1] == 0 && map[x, z + 1] == 0 && !pillarLocations.Contains(new MapLocation(x - 1, z)))
                        {
                            cornerPillar = Instantiate(pillar);
                            cornerPillar.transform.position = new Vector3((x - .5f) * scale, 0, (z + .5f) * scale);
                            cornerPillar.name = "West Top";
                            pillarLocations.Add(new MapLocation(x - 1, z));
                        }

                        if (map[x - 1, z - 1] == 0 && map[x, z - 1] == 0 && !pillarLocations.Contains(new MapLocation(x - 1, z - 1)))
                        {
                            cornerPillar = Instantiate(pillar);
                            cornerPillar.transform.position = new Vector3((x - .5f) * scale, 0, (z - .5f) * scale);
                            cornerPillar.name = "West Bottom";
                            pillarLocations.Add(new MapLocation(x - 1, z - 1));
                        }
                    }

                    /*GameObject doorway;
                    LocateDoors(x, z);
                    if (north)
                    {
                        doorway = Instantiate(door);
                        doorway.transform.position = new Vector3(x * scale, 0, z * scale);
                        doorway.transform.Rotate(0, 180, 0);
                        doorway.name = "North Door";
                    }

                    if (south)
                    {
                        doorway = Instantiate(door);
                        doorway.transform.position = new Vector3(x * scale, 0, z * scale);
                        doorway.name = "South Door";
                    }

                    if (east)
                    {
                        doorway = Instantiate(door);
                        doorway.transform.position = new Vector3(x * scale, 0, z * scale);
                        doorway.transform.Rotate(0, 90, 0);
                        doorway.name = "East Door";
                    }

                    if (west)
                    {
                        doorway = Instantiate(door);
                        doorway.transform.position = new Vector3(x * scale, 0, z * scale);
                        doorway.transform.Rotate(0, -90, 0);
                        doorway.name = "West Door";
                    }*/
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

    public void LocateDoors(int x, int z)
    {
        north = false;
        south = false;
        east = false;
        west = false;

        if (x <= 0 || x >= width - 1 || z <= 0 || z >= depth - 1) return;
        if (map[x, z + 1] == 0 && map[x - 1, z + 1] == 1 && map[x + 1, z + 1] == 1) north = true;
        if (map[x, z - 1] == 0 && map[x - 1, z - 1] == 1 && map[x + 1, z - 1] == 1) south = true;
        if (map[x + 1, z] == 0 && map[x + 1, z + 1] == 1 && map[x + 1, z - 1] == 1) east = true;
        if (map[x - 1, z] == 0 && map[x - 1, z + 1] == 1 && map[x - 1, z - 1] == 1) west = true;
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
