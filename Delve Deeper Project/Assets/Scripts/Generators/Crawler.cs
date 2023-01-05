using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crawler : Maze
{
    public override void GenerateCorridor()
    {
        for(int i = 0; i < 2; i++)
            CrawlVertically();

        for (int i = 0; i < 3; i++)
            CrawlHorizontally();
    }

    private void CrawlVertically()
    {
        bool done = false;
        int x = Random.Range(1, width);
        int z = 1;

        while (!done)
        {
            map[x, z] = 0;
            if (Random.Range(0, 100) < 50)
                x += Random.Range(-1, 2);
            else
                z += Random.Range(0, 2);
            done |= (x < 1 || x >= width - 1 || z < 1 || z >= depth - 1);
        }
    }

    private void CrawlHorizontally()
    {
        bool done = false;
        int x = 1;
        int z = Random.Range(1, depth);

        while (!done)
        {
            map[x, z] = 0;
            if (Random.Range(0, 100) < 50)
                x += Random.Range(0, 2);
            else
                z += Random.Range(-1, 2);
            done |= (x < 1 || x >= width - 1 || z < 1 || z >= depth - 1);
        }
    }
}
