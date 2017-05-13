using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMap : MonoBehaviour
{
    public float WeatherLoopSpeed;
    public float Sealevel;
    public float BeachGap;
    public float DesertThreshold;
    public int Seed = 1;
    public bool RandomSeed = true;
    public int HumiditySeed = 1;
    public int Size;
    public MapTile MapTilePrefab;

    public MapList Map = new MapList();

	// Use this for initialization
	void Start ()
    {
        if (RandomSeed)
            Seed = Random.Range(0, 10000);

        for (int x = 0; x < Size; x++)
        {
            for (int y = 0; y < Size; y++)
            {
                MapTile tile = GameObject.Instantiate(MapTilePrefab, new Vector2(x + 0.5f, y + 0.5f), MapTilePrefab.transform.rotation) as MapTile;
                tile.Info.HumidityThreshold = DesertThreshold;
                float Elevation = Mathf.PerlinNoise((float)(x + Seed) * 0.018f, (float)(y + Seed) * 0.018f);
                tile.Info.Elevation = Elevation;
                tile.Info.Latitude = (y - (Size / 2f)) / (Size / 2f);

                if(Elevation > Sealevel + BeachGap)
                {
                    tile.Type = TileType.Land;
                    float Hum1 = Mathf.PerlinNoise((float)(x + HumiditySeed + 1000) * 0.008f, (float)(y + HumiditySeed + 1000) * 0.008f);

                    tile.Info.Humidity = (Hum1 - 0.5f) + (1-Elevation);
                }
                else if (Elevation > Sealevel)
                {
                    tile.Type = TileType.Beach;
                    tile.Info.Humidity = 1.5f;
                    tile.Info.HumidityNormalized = 1.5f;
                }
                else
                {
                    tile.Type = TileType.Water;
                    tile.Info.Humidity = 2f;
                    tile.Info.HumidityNormalized = 2f;
                }


                Map.Add(x, tile);
                tile.Init();
            }
        }
    }

    float timer;
    bool counting = true;
    UpdateMap job = new UpdateMap();
    // Update is called once per frame
    void Update()
    {
        if(counting)
        timer += Time.deltaTime;
        if (timer >= WeatherLoopSpeed)
        {
            counting = false;
            timer = 0;

            job.Map = Map;
            HumiditySeed++;
            job.Seed = HumiditySeed;
            job.Start();

            StartCoroutine(JobWorking());
        }
    }

    IEnumerator JobWorking()
    {
        yield return StartCoroutine(job.WaitFor());

        counting = true;
    }
}

public class MapList
{
    [HideInInspector]
    public List<List<MapTile>> map = new List<List<MapTile>>();

    public MapTile this[int x, int y]
    {
        get
        {
            return map[x][y];
        }
    }

    public void Add(int layer, MapTile tile)
    {
        if(map.Count - 1 >= layer)
        {
            map[layer].Add(tile);
        }
        else
        {
            map.Add(new List<MapTile>());
            map[layer].Add(tile);
        }
    }
}
