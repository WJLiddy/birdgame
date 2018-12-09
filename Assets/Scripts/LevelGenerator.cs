using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator
{
    public static float TILE_SIZE = 30;
    public static float PERLIN_RATE = 0.1f;


    public static bool isWall(int x, int y)
    {
        return (x == 0 ||
                    (x + 1 >= 800 / TILE_SIZE) ||
                    y == 0 ||
                    (y + 1 >= 8000 / TILE_SIZE));
    }

    public static bool isStartPlat(int x, int y)
    {
        return ((y == (int)((8000 / TILE_SIZE) - 5)) && x < 8);
    }

    public static bool isGroundFloor(int x, int y)
    {
        return !isWall(x, y) && (y == 1 || y == 2);
    }

    public static bool isTopClearing(int x, int y)
    {
        return !isWall(x, y) && !isStartPlat(x,y) && (y > 7500 / TILE_SIZE);
    }

    public static GameObject levelGenerate()
    {
        GameObject parent = new GameObject();
        GameObject plat = Resources.Load<GameObject>("platform");
        for (int x = 0; x < 800 / TILE_SIZE; ++x)
        {
            for (int y = 0; y < 8000 / TILE_SIZE; ++y)
            {
                if (isTopClearing(x, y) || isGroundFloor(x, y))
                {
                    continue;
                }

                if (isWall(x,y) || isStartPlat(x,y) || 
                    Mathf.PerlinNoise(x * PERLIN_RATE, y * PERLIN_RATE) > 0.6)
                    {
                        GameObject plat2 = GameObject.Instantiate(plat);
                        plat2.transform.SetParent(parent.transform);
                        plat2.transform.localPosition = new Vector2(x * TILE_SIZE / 100, y * TILE_SIZE / 100);
                    }

            }
        }
        return parent;
    }
}
