using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator
{
    public static float TILE_SIZE = 30;
    public static float PERLIN_RATE = 0.2f;


    public static bool[,] genGamePlatsAry()
    {
        return new bool[1 + (int)(8000 / TILE_SIZE), 1 + (int)(600 / TILE_SIZE)];
    }

    public static bool isWall(int x, int y)
    {
        return (x == 0 ||
                    (x + 1 >= 8000 / TILE_SIZE) ||
                    y == 0 ||
                    (y + 1 >= 600 / TILE_SIZE));
    }


    public static bool isLevelEnd(int x, int y)
    {
        return !isWall(x, y) && (x + 10 >= 8000);
    }

    public static bool isStartClearing(int x, int y)
    {
        return  !isWall(x, y) && (x < 10);
    }
    public static void genPlaform(GameObject parent, int x, int y)
    {
        GameObject plat = Resources.Load<GameObject>("platform");
        GameObject plat2 = GameObject.Instantiate(plat);
        plat2.transform.SetParent(parent.transform);
        plat2.transform.localPosition = new Vector2(x * TILE_SIZE / 100, y * TILE_SIZE / 100);
    }

    public static bool[,] getFilledGamePlatsAry()
    {
        var gamePlatsAry = genGamePlatsAry();
        for (int x = 0; x < 8000 / TILE_SIZE; ++x)
        {
            for (int y = 0; y < 600 / TILE_SIZE; ++y)
            {
                if (isStartClearing(x, y) || isLevelEnd(x, y))
                {
                    continue;
                }

                if (isWall(x, y) ||
                    Mathf.PerlinNoise(x * PERLIN_RATE, y * PERLIN_RATE) > 0.6)
                {
                    gamePlatsAry[x, y] = true;
                }

            }
        }
        return gamePlatsAry;
    }

    public static GameObject levelGenerate()
    {
        GameObject parent = new GameObject();
        var plats = getFilledGamePlatsAry();
        for (int i = 0; i != plats.GetLength(0); ++i)
        {
            for(int j = 0; j != plats.GetLength(1); ++j)
            {
                if (!plats[i, j])
                {
                    if (Random.value > 0.9)
                    {
                        var v = (new EnemyButterfly()).create();
                        v.transform.SetParent(parent.transform);
                        v.transform.position = new Vector2(i * TILE_SIZE / 100, j * TILE_SIZE / 100);
                    }
                } else
                {
                    genPlaform(parent, i, j);
                }
            }
        }
        return parent;
    }
}
