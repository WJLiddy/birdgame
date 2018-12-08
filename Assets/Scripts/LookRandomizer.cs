using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookRandomizer
{
    public class PlayerLookData
    {
        public Sprite[] walk;
        public Sprite[] fly;
    }

    public static Sprite Randomize(Sprite s, Vector3 data)
    {
        Texture2D orig = s.texture;
        var pixels = orig.GetPixels32();
        List<int> radiances = new List<int>();
        var greyPixels = new List<int>();
        foreach (Color32 p in pixels)
        {
            if (p.a != 255)
            {
                greyPixels.Add(-1);
                radiances.Add(0);
                continue;
            }

            List<int> cols = new List<int> { p.r, p.g, p.b };
            cols.Sort();

            int radiance = ((cols[2] - cols[1]) + (cols[2] - cols[0]));
            greyPixels.Add(cols[0]);
            radiances.Add(radiance);
        }

        var newPixels = new List<Color32>();

        for (int i = 0; i != greyPixels.Count; ++i)
        {
            Color32 cr = new Color32();
            if(greyPixels[i] == -1)
            {
                cr.a = 0;
            } else
            {
                cr.a = 255;
                cr.r = (byte)(Mathf.Min(255,greyPixels[i] + radiances[i] * data.x));
                cr.g = (byte)(Mathf.Min(255, greyPixels[i] + radiances[i] * data.y));
                cr.b = (byte)(Mathf.Min(255, greyPixels[i] + radiances[i] * data.z));
            }
            newPixels.Add(cr);
        }
        Texture2D tex = new Texture2D(s.texture.width, s.texture.height);
        tex.SetPixels32(newPixels.ToArray());
        tex.Apply();
        return Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f));
    }

    public static PlayerLookData PLDRandomizer()
    {
        // Gen player color
        Vector3 v = new Vector3(Random.value, Random.value, Random.value).normalized;
        PlayerLookData pld = new PlayerLookData();
        var a = Resources.LoadAll<Sprite>("birdflyanim/");
        var b = Resources.LoadAll<Sprite>("birdwalkanim/");
        pld.walk = new Sprite[b.Length];
        pld.fly =  new Sprite[a.Length];

        for (int i =0; i != a.Length; ++i)
        {
            pld.fly[i] = Randomize(a[i], v);
        }

        for (int j = 0; j != b.Length; ++j)
        {
            pld.walk[j] = Randomize(b[j], v);
        }
        return pld;
    }
    // Load original image.
}
