using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserAdvancedOptions : MonoBehaviour {

    
    [HideInInspector]
    public enum DrawMode { HeightMap, ColorMap, Mesh }

    public DrawMode drawMode;

    public int mapWidth;
    public int mapHeight;
    public float noiseScale;
    public int octaves;
    [Range(0, 1)]
    public float persistence;
    public float lacunarity;
    public int seed;

    public Vector2 offset;

    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve;
    public bool autoUpdate;

    [HideInInspector]
    public string dMode;

    public void UpdateOptions()
    {
        GameObject mapGen = GameObject.Find("MapGenerator");
        MapGenerator mapGenerator = mapGen.GetComponent<MapGenerator>();

        mapGenerator.GenerateMap();
        mapGenerator.UpdateDraw();
    }

    public void UpdateVariables()
    {
        GameObject mapGen = GameObject.Find("MapGenerator");
        MapGenerator mapGenerator = mapGen.GetComponent<MapGenerator>();

        if (mapGenerator.dMode == "ColorMap")
            drawMode = DrawMode.ColorMap;
        else if (mapGenerator.dMode == "HeightMap")
            drawMode = DrawMode.HeightMap;
        else if (mapGenerator.dMode == "Mesh")
            drawMode = DrawMode.Mesh;

        mapWidth = mapGenerator.mapWidth;
        mapHeight = mapGenerator.mapHeight;
        noiseScale = mapGenerator.noiseScale;
        octaves = mapGenerator.octaves;
        persistence = mapGenerator.persistence;
        lacunarity = mapGenerator.lacunarity;
        seed = mapGenerator.seed;
        offset.x = mapGenerator.offset.x;
        offset.y = mapGenerator.offset.y;
        meshHeightMultiplier = mapGenerator.meshHeightMultiplier;
        meshHeightCurve = mapGenerator.meshHeightCurve;
        autoUpdate = mapGenerator.autoUpdate;
    }

    public void SetDrawMode()
    {
        if (drawMode == DrawMode.ColorMap)
            dMode = "ColorMap";
        else if (drawMode == DrawMode.HeightMap)
            dMode = "HeightMap";
        else if (drawMode == DrawMode.Mesh)
            dMode = "Mesh";
    }
}
