using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserOptions : MonoBehaviour {

    //controls variables linked to terrain, i.e. height multiplier, lacunarity, etc.
	public enum TerrainType {Custom, Mountainous, Flat, Hilly};
    public TerrainType terrainType;
    //controls variables linked to colour and architectural/vegetation details
    public enum EnvironmentType {Custom, Forest, RainForest, Fields, Urban, City};
    public EnvironmentType environmentType;

    public enum TerrainGenerationType {PerlinNoise, CellularAutomata};
    public TerrainGenerationType terrainGenerationType;

    public bool autoUpdate;
    [HideInInspector]
    public string tType;
    [HideInInspector]
    public string eType;
    [HideInInspector]
    public string tGType;



    GameObject mapGen;

    void Start()
    {
        
        
    }

    

    public void UpdateOptions()
    {
        mapGen = GameObject.Find("MapGenerator");
        MapGenerator mapGenerator = mapGen.GetComponent<MapGenerator>();
        GameObject userAO = GameObject.Find("Options");
        UserAdvancedOptions userAdvancedOptions = userAO.GetComponent<UserAdvancedOptions>();

        if (terrainType == TerrainType.Mountainous)
        {
            tType = "Mountainous";
            mapGenerator.noiseScale = 48;
            mapGenerator.octaves = 5;
            mapGenerator.persistence = 0.384f;
            mapGenerator.lacunarity = 2.91f;
            mapGenerator.seed = 0;
            mapGenerator.offset.x = 0;
            mapGenerator.offset.y = 0;
            mapGenerator.meshHeightMultiplier = 20.0f;
            //mapGenerator.meshHeightCurve = new AnimationCurve(new Keyframe(mapGenerator.regions[1].height, 0f), new Keyframe(1f, 1f));
            userAdvancedOptions.UpdateVariables();
            mapGenerator.GenerateMap();
            
        }
        else if (terrainType == TerrainType.Flat)
        {
            tType = "Flat";
            mapGenerator.noiseScale = 48;
            mapGenerator.octaves = 5;
            mapGenerator.persistence = 0.384f;
            mapGenerator.lacunarity = 2.91f;
            mapGenerator.seed = 0;
            mapGenerator.offset.x = 0;
            mapGenerator.offset.y = 0;
            mapGenerator.meshHeightMultiplier = 20.0f;
            //mapGenerator.meshHeightCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(mapGenerator.regions[1].height, 0.0f), new Keyframe(1f, 0.1f));
            userAdvancedOptions.UpdateVariables();
            mapGenerator.GenerateMap();
        }
        else if (terrainType == TerrainType.Hilly)
        {
            tType = "Hilly";
            mapGenerator.noiseScale = 48;
            mapGenerator.octaves = 5;
            mapGenerator.persistence = 0.384f;
            mapGenerator.lacunarity = 2.91f;
            mapGenerator.seed = 0;
            mapGenerator.offset.x = 0;
            mapGenerator.offset.y = 0;
            mapGenerator.meshHeightMultiplier = 10.0f;
            //mapGenerator.meshHeightCurve = new AnimationCurve(new Keyframe(mapGenerator.regions[1].height, 0f), new Keyframe(1f, 1f));
            userAdvancedOptions.UpdateVariables();
            mapGenerator.GenerateMap();
        }
        
        if (environmentType == EnvironmentType.Forest)
        {

        }
        else if (environmentType == EnvironmentType.RainForest)
        {

        }
        else if (environmentType == EnvironmentType.Fields)
        {

        }
        else if (environmentType == EnvironmentType.Urban)
        {

        }
        else if (environmentType == EnvironmentType.City)
        {

        }

    }

    public void UpdateVariables()
    {
        //GameObject mapGen = GameObject.Find("MapGenerator");
        //MapGenerator mapGenerator = mapGen.GetComponent<MapGenerator>();

        terrainType = TerrainType.Custom;

    }

}

