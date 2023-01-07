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

    //values to determine the X and Z lengths of maze
    public int width = 30;
    public int depth = 30;
    public int scale = 6;

    public byte[,] map;

    public GameObject straight;
    public GameObject crossroad;
    public GameObject corner;
    public GameObject tSection;
    public GameObject deadEnd;

    public GameObject player;

    void Start()
    {
        InitialiseMap();
        GenerateCorridor();
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
                    GameObject go = Instantiate(deadEnd);
                    go.transform.position = new Vector3(x * scale, 0, z * scale);
                    go.transform.Rotate(0, 90, 0);
                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 1, 0, 0, 5, 1, 5 }))                     //horizontal dead end |<-
                {
                    GameObject go = Instantiate(deadEnd);
                    go.transform.position = new Vector3(x * scale, 0, z * scale);
                    go.transform.Rotate(0, -90, 0);
                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 1, 0, 1, 5, 0, 5 }))                     //vertical dead end T
                {
                    GameObject go = Instantiate(deadEnd);
                    go.transform.position = new Vector3(x * scale, 0, z * scale);
                }
                else if (Search2D(x, z, new int[] { 5, 0, 5, 1, 0, 1, 5, 1, 5 }))                     //vertical dead end downT
                {
                    GameObject go = Instantiate(deadEnd);
                    go.transform.position = new Vector3(x * scale, 0, z * scale);
                    go.transform.Rotate(0, 180, 0);
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
                else if (Search2D(x, z, new int[] {5, 1, 5, 0, 0, 1, 1, 0, 5 }))                      //upper right corner
                {
                    GameObject go = Instantiate(corner);
                    go.transform.position = new Vector3(x * scale, 0, z * scale);
                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 1, 0, 0, 5, 0, 1 }))                     //upper left corner
                {
                    GameObject go = Instantiate(corner);
                    go.transform.position = new Vector3(x * scale, 0, z * scale);
                    go.transform.Rotate(0, -90, 0);
                }
                else if (Search2D(x, z, new int[] { 5, 0, 1, 1, 0, 0, 5, 1, 5 }))                     //lower left corner
                {
                    GameObject go = Instantiate(corner);
                    go.transform.position = new Vector3(x * scale, 0, z * scale);
                    go.transform.Rotate(0, 180, 0);
                }
                else if (Search2D(x, z, new int[] { 1, 0, 5, 5, 0, 1, 5, 1, 5 }))                     //lower right corner
                {
                    GameObject go = Instantiate(corner);
                    go.transform.position = new Vector3(x * scale, 0, z * scale);
                    go.transform.Rotate(0, 90, 0);
                }
                else if (Search2D(x, z, new int[] { 1, 0, 1, 0, 0, 0, 5, 1, 5 }))                     //upside t section
                {
                    GameObject go = Instantiate(tSection);
                    go.transform.position = new Vector3(x * scale, 0, z * scale);
                    go.transform.Rotate(0, 180, 0);
                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 0, 0, 0, 1, 0, 1 }))                     //t section
                {
                    GameObject go = Instantiate(tSection);
                    go.transform.position = new Vector3(x * scale, 0, z * scale);
                }
                else if (Search2D(x, z, new int[] { 1, 0, 5, 0, 0, 1, 1, 0, 5 }))                     //t section left
                {
                    GameObject go = Instantiate(tSection);
                    go.transform.position = new Vector3(x * scale, 0, z * scale);
                    go.transform.Rotate(0, 90, 0);
                }
                else if (Search2D(x, z, new int[] { 5, 0, 1, 1, 0, 0, 5, 0, 1 }))                     //t section right
                {
                    GameObject go = Instantiate(tSection);
                    go.transform.position = new Vector3(x * scale, 0, z * scale);
                    go.transform.Rotate(0, -90, 0);
                }
            }
    }

    public virtual void PlacePlayerInMaze()
    {
        for (int z = 0; z < depth; z++)
            for (int x = 0; x < width; x++)
            {
                if (map[x, z] == 0)
                {
                    player.transform.position = new Vector3(x * scale, -2.75f, z * scale);
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
