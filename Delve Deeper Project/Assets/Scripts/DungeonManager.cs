using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public Maze[] mazes;
    public int width = 20;
    public int depth = 20;

    public GameObject stairs;

    List<MapLocation> level1ends = new List<MapLocation>();
    List<MapLocation> level2ends = new List<MapLocation>();

    // Start is called before the first frame update
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

        for (int mazeIndex = 0; mazeIndex < mazes.Length - 1; mazeIndex++)
        {
            level1ends.Clear();
            level2ends.Clear();
            for (int z = 0; z < depth; z++)
                for (int x = 0; x < width; x++)
                {
                    if (mazes[mazeIndex].piecePlaces[x, z].piece == Maze.PieceType.West_DeadEnd)
                        level1ends.Add(new MapLocation(x, z));

                    if (mazes[mazeIndex + 1].piecePlaces[x, z].piece == Maze.PieceType.East_DeadEnd)
                        level2ends.Add(new MapLocation(x, z));

                }

            if (level1ends.Count == 0 || level2ends.Count == 0) break;

            MapLocation bottomOfStairs = level1ends[UnityEngine.Random.Range(0, level1ends.Count)];
            MapLocation topOfStairs = level2ends[UnityEngine.Random.Range(0, level2ends.Count)];

            mazes[mazeIndex + 1].xOffset = bottomOfStairs.x - topOfStairs.x + mazes[mazeIndex].xOffset;
            mazes[mazeIndex + 1].zOffset = bottomOfStairs.z - topOfStairs.z + mazes[mazeIndex].zOffset;

            Vector3 bottomStairPos = new Vector3(bottomOfStairs.x * mazes[mazeIndex].scale,
                            mazes[mazeIndex].scale * mazes[mazeIndex].level * mazes[mazeIndex].levelDistance,
                            bottomOfStairs.z * mazes[mazeIndex].scale);
            Vector3 topStairPos = new Vector3(topOfStairs.x * mazes[mazeIndex].scale,
                            mazes[mazeIndex].scale * mazes[mazeIndex].level * mazes[mazeIndex].levelDistance,
                            topOfStairs.z * mazes[mazeIndex].scale);

            Destroy(mazes[mazeIndex].piecePlaces[bottomOfStairs.x, bottomOfStairs.z].model);
            Destroy(mazes[mazeIndex + 1].piecePlaces[topOfStairs.x, topOfStairs.z].model);

            GameObject stairwell = Instantiate(stairs, bottomStairPos, Quaternion.identity);
            mazes[mazeIndex].piecePlaces[bottomOfStairs.x, bottomOfStairs.z].piece = Maze.PieceType.Stairs;
            mazes[mazeIndex].piecePlaces[bottomOfStairs.x, bottomOfStairs.z].model = stairwell;

            mazes[mazeIndex + 1].piecePlaces[topOfStairs.x, topOfStairs.z].piece = Maze.PieceType.Stairs;
            mazes[mazeIndex + 1].piecePlaces[topOfStairs.x, topOfStairs.z].model = null;

            stairwell.transform.SetParent(mazes[mazeIndex].gameObject.transform);
        }

        for (int mazeIndex = 0; mazeIndex < mazes.Length; mazeIndex++)
        {
            mazes[mazeIndex + 1].gameObject.transform.Translate(mazes[mazeIndex + 1].xOffset * mazes[mazeIndex + 1].scale,
                0, mazes[mazeIndex + 1].zOffset * mazes[mazeIndex + 1].scale);
        }
        
    }
}
