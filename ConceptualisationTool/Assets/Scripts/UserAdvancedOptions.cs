using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserAdvancedOptions : MonoBehaviour {

    public bool autoUpdate;
    [HideInInspector]
    public enum DrawMode { HeightMap, ColorMap, Mesh }

    public DrawMode drawMode;

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
