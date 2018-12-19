using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Convert to class. (have to pass lots of args for state
public class LevelGenerator
{
    public static readonly float LEVEL_LENGTH = 5000; //In terms of tile size units
    public static readonly float LEVEL_HEIGHT = 600;
    public static float TILE_SIZE = 30;
    public static float PERLIN_RATE = 0.2f;
    public static int SAFE_ZONE = 20;

    public static bool[,] genGamePlatsAry()
    {
        return new bool[1 + (int)(LEVEL_LENGTH / TILE_SIZE), 1 + (int)(LEVEL_HEIGHT / TILE_SIZE)];
    }

    public static bool isWall(int x, int y)
    {
        return (x == 0 ||
                    (x + 1 >= LEVEL_LENGTH / TILE_SIZE) ||
                    y == 0 ||
                    (y + 1 >= LEVEL_HEIGHT / TILE_SIZE));
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
        return !isWall(x, y) && (x + 10 >= LEVEL_LENGTH / TILE_SIZE);
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
        for (int x = 0; x < LEVEL_LENGTH / TILE_SIZE; ++x)
        {
            for (int y = 0; y < LEVEL_HEIGHT / TILE_SIZE; ++y)
            {
                if (isStartClearing(x, y) || isLevelEnd(x, y) || sinClear(x, y, (seed % 100)/500f, 5 + (seed % 5)))
                {
                    continue;
                }

                if (isWall(x, y) ||
                    Mathf.PerlinNoise((seed + x) * PERLIN_RATE, (seed + y) * PERLIN_RATE) > 0.5)
                {
                    gamePlatsAry[x, y] = true;
                }

            }
        }
        return gamePlatsAry;
    }

    public static void spikeFill(ref bool[,] plats, int difmod, GameObject parent)
    {
        float seed = ihash(1 + difmod) % 777;
        float spikeRand = 0.05f + (Mathf.Pow(difmod, 1.7f) / 100f);

        for (int i = SAFE_ZONE; i < plats.GetLength(0) - 1; ++i)
        {
            for (int j = 1; j < plats.GetLength(1) - 2; ++j)
            {
                if (Mathf.PerlinNoise((seed + i) * PERLIN_RATE, (seed + j) * PERLIN_RATE) < spikeRand)
                {
                    if(!plats[i,j])
                    {
                        continue;
                    }
                    GameObject spike = null;
                    //figure out orientation. 
                    bool up = plats[i, j + 1];
                    bool down = plats[i, j - 1];
                    bool left = plats[i - 1, j];
                    bool right = plats[i + 1, j];
                    if(up && !down)
                    {
                        spike = (new EnemySpike()).create();
                        spike.GetComponent<EnemySpike>().setOrientation(180);
                    }
                    else if(down && !up)
                    {
                        j++;
                        spike = (new EnemySpike()).create();
                        spike.GetComponent<EnemySpike>().setOrientation(0);
                    }
                    else if(right && !left)
                    {
                        spike = (new EnemySpike()).create();
                        spike.GetComponent<EnemySpike>().setOrientation(90);
                    }
                    else if(left && !right)
                    {
                        spike = (new EnemySpike()).create();
                        spike.GetComponent<EnemySpike>().setOrientation(-90);
                    }

                    if(spike != null)
                    {
                        plats[i, j] = false;
                        spike.transform.SetParent(parent.transform);
                        spike.transform.position = new Vector2(i * TILE_SIZE / 100, j * TILE_SIZE / 100);
                    }
                }
            }
        }

    }

    // member method conversion!
    public static bool tryGenerateEnemy(int x, int y, bool[,] plats, int difmod, GameObject parent)
    {
        float scaledDifMod = (difmod * 0.005f); // increase spawn chance by 1% per difmod.

        GameObject v = null;
        if (Random.value > (0.99 - scaledDifMod))
        {
            v = (new EnemyButterfly()).create();

        }
        else if (Random.value > (1 - scaledDifMod)) // orig 1.004
        {
            v = (new EnemyMouse()).create();
        }
        else if (Random.value > (1.01 - scaledDifMod))
        {
             v = (new EnemySnake()).create();
        }
        else if (Random.value > (1.02 - scaledDifMod))
        {
            v = (new EnemySnail()).create();
        } 
        if (v != null)
        {
            v.transform.SetParent(parent.transform);
            v.transform.position = new Vector2(x * TILE_SIZE / 100, y * TILE_SIZE / 100);
            return true;
        }
        return  false;
    }

    public static void tryGenerateItem(int x, int y, bool[,] plats, int difmod, GameObject parent, ref bool cageset)
    {
        if (Random.value > .993f && plats[x, y - 1])
        {
            GameObject helm = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/helm"));
            helm.transform.SetParent(parent.transform);
            helm.transform.position = new Vector2(x * TILE_SIZE / 100, y * TILE_SIZE / 100);
        }
        else if (x % 100 == 0 && !cageset && plats[x, y - 1] && Random.value > 0.3)
        {
            cageset = true;
            GameObject cage = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/cage"));
            cage.transform.SetParent(parent.transform);
            cage.transform.position = new Vector2(x * TILE_SIZE / 100, y * TILE_SIZE / 100);
        }

        // items
        else if (plats[x, y - 1] && Random.value > 0.994)
        {
            GameObject item = null;
            switch(Random.Range(0,3))
            {
                case 0: item = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/axe")); break;
                case 1: item = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/cbow")); ; break;
                case 2: item = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/pistol")); break;
            }
            item.transform.SetParent(parent.transform);
            item.transform.position = new Vector2(x * TILE_SIZE / 100, y * TILE_SIZE / 100);
        }
    }

    public static GameObject levelGenerate(int difmod)
    {
        GameObject parent = new GameObject();
        var plats = getFilledGamePlatsAry(difmod);
        spikeFill(ref plats, difmod, parent);
        for (int i = 0; i != plats.GetLength(0); ++i)
        {
            bool cageset = false;
            for (int j = 0; j < plats.GetLength(1) - 1; ++j)
            {
               
                if (!plats[i, j])
                {
                    if(i < SAFE_ZONE || !tryGenerateEnemy(i,j,plats,difmod,parent))
                    {
                        tryGenerateItem(i, j, plats, difmod, parent, ref cageset);
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
        flag.transform.position = new Vector2((plats.GetLength(0) - 3) * TILE_SIZE / 100,  (1 * TILE_SIZE)/100f );
        return parent;
    }
}
