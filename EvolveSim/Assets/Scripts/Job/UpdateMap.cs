using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateMap : ThreadedJob
{
    public MapList Map;
    public int Seed;

    protected override void ThreadFunction()
    {
        for (int x = 0; x < Map.map.Count; x++)
        {
            for (int y = 0; y < Map.map.Count; y++)
            {
                if (Map[x, y].Type == TileType.Land)
                {
                    float Hum1 = Mathf.PerlinNoise((float)(x + Seed + 1000) * 0.018f, (float)(y + Seed + 1000) * 0.018f);
                    float Temp1 = Mathf.PerlinNoise((float)(x + Seed + 1000) * 0.018f, (float)(y + Seed + 1000) * 0.018f);

                    Map[x, y].Info.Humidity = ((Hum1 * 2) - 1) + (1 - ((Map[x, y].Info.Elevation - 0.35f) / (1f - 0.35f)));
                    Map[x, y].Info.Temperature = (1 - (Mathf.Abs(Map[x, y].Info.Latitude) * 1.1f)) < 0 ? 0 : (1 - (Mathf.Abs(Map[x, y].Info.Latitude) * 1.1f));
                }
            }
        }
    }
    protected override void OnFinished()
    {
        for (int x = 0; x < Map.map.Count; x++)
        {
            for (int y = 0; y < Map.map.Count; y++)
            {
                if (Map[x, y].Type == TileType.Land)
                {
                    Map[x, y].Init();
                }
            }
        }
    }
}
