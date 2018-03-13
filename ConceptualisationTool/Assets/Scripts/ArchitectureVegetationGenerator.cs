using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArchitectureVegetationGenerator : MonoBehaviour {

    float[,] noiseMap;
    int mapWidth;
    int mapHeight;
    int housingWidthStart;
    int housingHeightStart;
    int housingWidth;
    int housingHeight;
    float heightMultiplier;

    public GameObject housePrefab;
    public GameObject roadL;
    public GameObject roadStraight;
    GameObject newHouse;
    List<GameObject> architectureModels;

    public void GenerateArchitecture()
    {
        GameObject mapGen = GameObject.Find("MapGenerator");
        MapGenerator mapGenerator = mapGen.GetComponent<MapGenerator>();

        noiseMap = mapGenerator.GetNoiseMap();
        mapWidth = mapGenerator.mapWidth;
        mapHeight = mapGenerator.mapHeight;
        housingHeight = mapGenerator.housingHeight;
        housingWidth = mapGenerator.housingWidth;
        housingHeightStart = mapGenerator.housingHeightStart;
        housingWidthStart = mapGenerator.housingWidthStart;
        heightMultiplier = mapGenerator.meshHeightMultiplier;

        if(architectureModels.Count > 0)
            foreach(GameObject m in architectureModels)
                DestroyImmediate(m);

        architectureModels.Add(Instantiate(housePrefab, new Vector3(-mapWidth * 5 + housingWidthStart * 10, noiseMap[housingWidthStart, housingHeightStart] * heightMultiplier / 2 + 0.1f, mapHeight * 5 - housingHeightStart * 10), Quaternion.Euler(0, 0, 0), transform.parent));

        architectureModels.Add(Instantiate(roadStraight, new Vector3(-mapWidth * 5 + (housingWidthStart + 5) * 10, noiseMap[housingWidthStart + 5, housingHeightStart + 5] * heightMultiplier /2 + 0.1f, mapHeight * 5 - (housingHeightStart + 5) * 10), Quaternion.Euler(-90, -90, 0), transform.parent));
        

    }

}
