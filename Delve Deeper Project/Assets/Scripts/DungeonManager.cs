using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public Maze[] mazes;
    public int width = 20;
    public int depth = 20;

    public GameObject stairwell;

    void Start()
    {
        int level = 0;
        foreach (Maze m in mazes)
        {
            m.width = width;
            m.depth = depth;
            m.level = level++;
            m.levelDistance = 1.5f;
            m.Build();
        }

        width += 6;
        depth += 6;

        for (int mazeIndex = 0; mazeIndex < mazes.Length - 1; mazeIndex++)
        {
            if (PlaceStairs(mazeIndex, 0, Maze.PieceType.West_DeadEnd, Maze.PieceType.East_DeadEnd, stairwell)) continue;
            if (PlaceStairs(mazeIndex, 90, Maze.PieceType.DeadEnd, Maze.PieceType.UpsideDown_DeadEnd, stairwell)) continue;
            if (PlaceStairs(mazeIndex, 180, Maze.PieceType.East_DeadEnd, Maze.PieceType.West_DeadEnd, stairwell)) continue;
            PlaceStairs(mazeIndex, -90, Maze.PieceType.UpsideDown_DeadEnd, Maze.PieceType.DeadEnd, stairwell);
        }

        for (int mazeIndex = 0; mazeIndex < mazes.Length; mazeIndex++)
        {
            mazes[mazeIndex + 1].gameObject.transform.Translate(mazes[mazeIndex + 1].xOffset * mazes[mazeIndex + 1].scale,
                0, mazes[mazeIndex + 1].zOffset * mazes[mazeIndex + 1].scale);
        }
        
    }

    bool PlaceStairs(int mazeIndex, float rotAngle, Maze.PieceType bottom, Maze.PieceType top, GameObject stairPrefab)
    {
        List<MapLocation> startingLocations = new List<MapLocation>();
        List<MapLocation> endingLocations = new List<MapLocation>();

        for (int z = 0; z < depth; z++)
            for (int x = 0; x < width; x++)
            {
                if (mazes[mazeIndex].piecePlaces[x, z].piece == bottom)
                    startingLocations.Add(new MapLocation(x, z));

                if (mazes[mazeIndex + 1].piecePlaces[x, z].piece == top)
                    endingLocations.Add(new MapLocation(x, z));
            }

        if (startingLocations.Count == 0 || endingLocations.Count == 0) return false;

        MapLocation bottomOfStairs = startingLocations[Random.Range(0, startingLocations.Count)];
        MapLocation topOfStairs = endingLocations[Random.Range(0, endingLocations.Count)];

        mazes[mazeIndex + 1].xOffset = bottomOfStairs.x - topOfStairs.x + mazes[mazeIndex].xOffset;
        mazes[mazeIndex + 1].zOffset = bottomOfStairs.z - topOfStairs.z + mazes[mazeIndex].zOffset;

        Vector3 bottomStairPos = new Vector3(bottomOfStairs.x * mazes[mazeIndex].scale,
                        mazes[mazeIndex].scale * mazes[mazeIndex].level * mazes[mazeIndex].levelDistance,
                        bottomOfStairs.z * mazes[mazeIndex].scale);

        Destroy(mazes[mazeIndex].piecePlaces[bottomOfStairs.x, bottomOfStairs.z].model);
        Destroy(mazes[mazeIndex + 1].piecePlaces[topOfStairs.x, topOfStairs.z].model);

        GameObject stairs = Instantiate(stairPrefab, bottomStairPos, Quaternion.identity);
        stairs.transform.Rotate(0, rotAngle, 0);
        mazes[mazeIndex].piecePlaces[bottomOfStairs.x, bottomOfStairs.z].piece = Maze.PieceType.Stairs;
        mazes[mazeIndex].piecePlaces[bottomOfStairs.x, bottomOfStairs.z].model = stairs;

        mazes[mazeIndex + 1].piecePlaces[topOfStairs.x, topOfStairs.z].piece = Maze.PieceType.Stairs;
        mazes[mazeIndex + 1].piecePlaces[topOfStairs.x, topOfStairs.z].model = null;

        stairs.transform.SetParent(mazes[mazeIndex].gameObject.transform);
        return true;
    }
}
