using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile : MonoBehaviour
{
    public float HumidityAlpha = 2f;
    public TileType Type;
    [SerializeField]
    public TileInfo Info;

    private Color GrasslandColor = new Color(0f / 255f, 120 / 255f, 60 / 255f);
    private Color DesertColor = new Color(137f / 255f, 152f / 255f, 44f / 255f);
    private Color BeachColor = new Color(208 / 255f, 226 / 255f, 141 / 255f);

    // Use this for initialization
    void Start()
    {

    }

    public void Init()
    {
        switch (Type)
        {
            case TileType.Water:
                transform.GetComponent<SpriteRenderer>().color = new Color(0, 102f / 255f, 175f / 255f);
                break;

            case TileType.Land:
                Info.HumidityNormalized = (Info.Humidity - Info.HumidityThreshold) / (1.5f - Info.HumidityThreshold);

                transform.GetComponent<SpriteRenderer>().color = Color.Lerp(DesertColor, GrasslandColor, Info.HumidityNormalized / HumidityAlpha);
                break;

            case TileType.Beach:
                transform.GetComponent<SpriteRenderer>().color = BeachColor;
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

[System.Serializable]
public struct TileInfo
{
    public float Latitude;
    public float Elevation;
    public float food;
    public Color foodColor;
    public float Temperature;
    [HideInInspector]
    public float Humidity;
    [HideInInspector]
    public float HumidityThreshold;
    public float HumidityNormalized;
    //public 
}

public enum TileType
{
    Water,
    Beach,
    Land,
    Ice
}
