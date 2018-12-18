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

    // freq 10 - 20
    // amp 
    public static bool sinClear(int x,int y, float freq, float amp)
    {
        float dx = 10 + (amp * Mathf.Sin(x * freq));
        return !isWall(x,y) && Mathf.Abs(y - dx) < 3;
    }


    public static bool isLevelEnd(int x, int y)
    {
        return !isWall(x, y) && (x + 10 >= 8000 / TILE_SIZE);
    }

    public static bool isStartClearing(int x, int y)
    {
        return  !isWall(x, y) && (x < 10);
    }
    public static void genPlaform(GameObject parent, int x, int y)
    {
        GameObject plat = Resources.Load<GameObject>("prefabs/platform");
        GameObject plat2 = GameObject.Instantiate(plat);
        plat2.transform.SetParent(parent.transform);
        plat2.transform.localPosition = new Vector2(x * TILE_SIZE / 100, y * TILE_SIZE / 100);
    }

    private static int ihash( int x)
    {
        x = ((x >> 16) ^ x) * 0x45d9f3b;
        x = ((x >> 16) ^ x) * 0x45d9f3b;
        x = (x >> 16) ^ x;
        return x;
    }

    public static bool[,] getFilledGamePlatsAry(int difmod)
    {
        int seed = ihash(difmod) % 1000;
        var gamePlatsAry = genGamePlatsAry();
        for (int x = 0; x < 8000 / TILE_SIZE; ++x)
        {
            for (int y = 0; y < 600 / TILE_SIZE; ++y)
            {
                if (isStartClearing(x, y) || isLevelEnd(x, y) || sinClear(x, y, (seed % 100)/500f, 5 + (seed % 5)))
                {
                    continue;
                }

                if (isWall(x, y) ||
                    Mathf.PerlinNoise((seed + x) * PERLIN_RATE, (seed + y) * PERLIN_RATE) > 0.6)
                {
                    gamePlatsAry[x, y] = true;
                }

            }
        }
        return gamePlatsAry;
    }

    public static GameObject levelGenerate(int difmod)
    {
        GameObject parent = new GameObject();
        var plats = getFilledGamePlatsAry(difmod);
        for (int i = 0; i != plats.GetLength(0); ++i)
        {
            bool cageset = false;
            for (int j = 0; j < plats.GetLength(1) - 1; ++j)
            {
               
                if (!plats[i, j])
                {
                    // Decrease with leve counter.
                    if (Random.value > (0.99 - (difmod * 0.005)))
                    {
                        if (i < 20)
                        {
                            continue;
                        }
                        var v = (new EnemyButterfly()).create();
                        v.transform.SetParent(parent.transform);
                        v.transform.position = new Vector2(i * TILE_SIZE / 100, j * TILE_SIZE / 100);
                    } else if (Random.value > .99f && plats[i,j-1])
                    {
                        GameObject helm = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/helm"));
                        helm.transform.SetParent(parent.transform);
                        helm.transform.position = new Vector2(i * TILE_SIZE / 100, j * TILE_SIZE / 100);
                    } else if (i % 100 == 0 && !cageset && plats[i, j - 1] && Random.value > 0.3)
                    {
                        cageset = true;
                        GameObject cage = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/cage"));
                        cage.transform.SetParent(parent.transform);
                        cage.transform.position = new Vector2(i * TILE_SIZE / 100, j * TILE_SIZE / 100);
                    }
                }
                else
                {
                    genPlaform(parent, i, j);
                }
            }
        }
        GameObject flag = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/flag"));
        flag.transform.SetParent(parent.transform);
        flag.transform.position = new Vector2(78,  (1 * TILE_SIZE)/100f );
        return parent;
    }
}
