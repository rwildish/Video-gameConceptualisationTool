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

    public AdvancedOptions[] advancedOptions;

    GameObject mapGen;

    void Start()
    {
        
    }

    public void UpdateOptions()
    {
        mapGen = GameObject.Find("MapGenerator");
        MapGenerator mapGenerator = mapGen.GetComponent<MapGenerator>();

        if (terrainType == TerrainType.Mountainous)
        {
            //Keyframe[] ks;
            //ks = new Keyframe[2];
            //ks[0] = new Keyframe(0, 0.3f);
            //ks[1] = new Keyframe(1, 1f);
            mapGenerator.noiseScale = 48;
            mapGenerator.octaves = 5;
            mapGenerator.persistence = 0.384f;
            mapGenerator.lacunarity = 2.91f;
            mapGenerator.seed = 0;
            mapGenerator.offset.x = 0;
            mapGenerator.offset.y = 0;
            mapGenerator.meshHeightMultiplier = 20.0f;
            mapGenerator.meshHeightCurve = new AnimationCurve(new Keyframe(mapGenerator.regions[1].height, 0f), new Keyframe(1f, 1f));
            mapGenerator.GenerateMap();
        }
        else if (terrainType == TerrainType.Flat)
        {
            mapGenerator.noiseScale = 48;
            mapGenerator.octaves = 5;
            mapGenerator.persistence = 0.384f;
            mapGenerator.lacunarity = 2.91f;
            mapGenerator.seed = 0;
            mapGenerator.offset.x = 0;
            mapGenerator.offset.y = 0;
            mapGenerator.meshHeightMultiplier = 20.0f;
            mapGenerator.meshHeightCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(mapGenerator.regions[1].height, 0.0f), new Keyframe(1f, 0.1f));
            mapGenerator.GenerateMap();
        }
        else if (terrainType == TerrainType.Hilly)
        {
            mapGenerator.noiseScale = 48;
            mapGenerator.octaves = 5;
            mapGenerator.persistence = 0.384f;
            mapGenerator.lacunarity = 2.91f;
            mapGenerator.seed = 0;
            mapGenerator.offset.x = 0;
            mapGenerator.offset.y = 0;
            mapGenerator.meshHeightMultiplier = 10.0f;
            mapGenerator.meshHeightCurve = new AnimationCurve(new Keyframe(mapGenerator.regions[1].height, 0f), new Keyframe(1f, 1f));
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

}
[System.Serializable]
public struct AdvancedOptions
{
    public string name;
}
