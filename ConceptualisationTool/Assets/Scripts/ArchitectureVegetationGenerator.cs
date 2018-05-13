using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArchitectureVegetationGenerator : MonoBehaviour {

    float[,] noiseMap;
    int[,] noiseMapBlocked;
    int mapWidth;
    int mapHeight;
    int housingWidthStart;
    int housingHeightStart;
    int housingWidth;
    int housingHeight;
    float heightMultiplier;
    AnimationCurve heightCurve;
    float avgHeight;

    public GameObject[] houseBuildings;
    public GameObject[] commercialBuildings;
    public GameObject[] roads;
    public GameObject[] vegetation;

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
        avgHeight = mapGenerator.GetAvgHeight();
        heightCurve = mapGenerator.meshHeightCurve;
        noiseMapBlocked = mapGenerator.GetNoiseMapBlocked();


        architectureModels.Add(Instantiate(housePrefab, new Vector3(-mapWidth * 5 + housingWidthStart * 10, noiseMap[housingWidthStart, housingHeightStart] * heightMultiplier / 2 + 0.1f, mapHeight * 5 - housingHeightStart * 10), Quaternion.Euler(0, 0, 0), transform.parent));

        if (architectureModels.Count > 0)
            foreach(GameObject m in architectureModels)
                DestroyImmediate(m);

        //architectureModels.Add(Instantiate(housePrefab, new Vector3(-mapWidth * 5 + (housingWidthStart + 1) * 10, heightCurve.Evaluate(noiseMap[housingWidthStart, housingHeightStart + 1]) * heightMultiplier * 10, mapHeight * 5 - (housingHeightStart + 1) * 10), Quaternion.Euler(0, -90, 0), transform.parent));

        //architectureModels.Add(Instantiate(roadStraight, new Vector3(-mapWidth * 5 + (housingWidthStart + 5) * 10, heightCurve.Evaluate(noiseMap[housingWidthStart + 5, housingHeightStart + 5]) * heightMultiplier * 10, mapHeight * 5 - (housingHeightStart + 5) * 10), Quaternion.Euler(-90, -90, 0), transform.parent));
        

        for (int x = 0; x <  housingWidth; x++)
        {
            for (int y = 0; y < housingHeight; y++)
            {
                if (noiseMapBlocked[housingWidthStart + x, housingHeightStart + y] == 1)
                {
                    Debug.Log(x + " " + y + " " + noiseMapBlocked[x + housingWidthStart, y + housingHeightStart]);
                    //Debug.Log(x + " " + y);
                    //continue;
                    if (y % 2 == 0)
                    {
                        if (y == 0 && x != 0)
                        {
                            architectureModels.Add(Instantiate(roads[2], new Vector3((-mapWidth * 5 + (housingWidthStart + x) * 10 - 5), heightCurve.Evaluate(noiseMap[housingWidthStart + x, housingHeightStart + y]) * heightMultiplier * 10 + 0.001f, (mapHeight * 5 - (housingHeightStart + y) * 10 - 7)), Quaternion.Euler(-90, -90, 0), transform.parent));
                            if(x != housingWidth - 1)
                                architectureModels.Add(Instantiate(roads[0], new Vector3(-mapWidth * 5 + (housingWidthStart + x) * 10, heightCurve.Evaluate(noiseMap[housingWidthStart + x, housingHeightStart + y]) * heightMultiplier * 10, mapHeight * 5 - (housingHeightStart + y) * 10 - 7), Quaternion.Euler(-90, -90, 0), transform.parent));


                        }
                        else if(x == housingWidth - 1 && y != 0 && y != housingHeight -1)
                        {
                            //architectureModels.Add(Instantiate(roads[2], new Vector3((-mapWidth * 5 + (housingWidthStart + x) * 10 - 5), heightCurve.Evaluate(noiseMap[housingWidthStart + x, housingHeightStart + y]) * heightMultiplier * 10 + 0.001f, (mapHeight * 5 - (housingHeightStart + y) * 10 - 7)), Quaternion.Euler(-90, -90, 0), transform.parent));
                            architectureModels.Add(Instantiate(roads[1], new Vector3(-mapWidth * 5 + (housingWidthStart + x) * 10 - 5, heightCurve.Evaluate(noiseMap[housingWidthStart + x, housingHeightStart + y]) * heightMultiplier * 10, mapHeight * 5 - (housingHeightStart + y) * 10 - 1), Quaternion.Euler(-90, 0, 0), transform.parent));
                            architectureModels.Add(Instantiate(roads[2], new Vector3((-mapWidth * 5 + (housingWidthStart + x) * 10 - 5), heightCurve.Evaluate(noiseMap[housingWidthStart + x, housingHeightStart + y]) * heightMultiplier * 10 + 0.001f, (mapHeight * 5 - (housingHeightStart + y) * 10 - 7)), Quaternion.Euler(-90, 0, 0), transform.parent));


                        }
                    }
                    else if(x == housingWidth - 1 && y != 0 && y != housingHeight - 1)
                    {
                        architectureModels.Add(Instantiate(roads[1], new Vector3(-mapWidth * 5 + (housingWidthStart + x) * 10 - 5, heightCurve.Evaluate(noiseMap[housingWidthStart + x, housingHeightStart + y]) * heightMultiplier * 10, mapHeight * 5 - (housingHeightStart + y) * 10 - 3), Quaternion.Euler(-90, 0, 0), transform.parent));

                    }
                   

                }
                else if (noiseMapBlocked[housingWidthStart + x, housingHeightStart + y] == 0)
                {
                    int result = (int)(Mathf.PerlinNoise(housingWidthStart + x / 10.0f, housingWidthStart + y / 10.0f) * 24);
                    
                    Debug.Log(x + " " + y + " " + noiseMapBlocked[x + housingWidthStart, y + housingHeightStart]);

                    if (y == 0 || y % 2 == 0)
                    {
                        architectureModels.Add(Instantiate(roads[0], new Vector3(-mapWidth * 5 + (housingWidthStart + x) * 10, heightCurve.Evaluate(noiseMap[housingWidthStart + x, housingHeightStart + y]) * heightMultiplier * 10, mapHeight * 5 - (housingHeightStart + y) * 10 - 7), Quaternion.Euler(-90, -90, 0), transform.parent));
                        //architectureModels.Add(Instantiate(roads[1], new Vector3(-mapWidth * 5 + (housingWidthStart + x) * 10 - 5, heightCurve.Evaluate(noiseMap[housingWidthStart + x, housingHeightStart + y]) * heightMultiplier * 10, mapHeight * 5 - (housingHeightStart + y) * 10 - 1), Quaternion.Euler(-90, 0, 0), transform.parent));

                        


                    }
                    else if (y % 2 == 1)
                    {
                        //architectureModels.Add(Instantiate(roads[1], new Vector3(-mapWidth * 5 + (housingWidthStart + x) * 10 - 5, heightCurve.Evaluate(noiseMap[housingWidthStart + x, housingHeightStart + y]) * heightMultiplier * 10, mapHeight * 5 - (housingHeightStart + y) * 10 - 3), Quaternion.Euler(-90, 0, 0), transform.parent));
                    }

                    if (x == 1 && y == 1)
                    {

                    }
                    else if (x == housingWidth - 1 && y == housingHeight - 1)
                    {

                    }
                    else if (x == 1 && y == housingHeight - 1)
                    {

                    }
                    else if (x == housingWidth && y == 0)
                    {

                    }
                    else if (x == 1)
                    {
                        architectureModels.Add(Instantiate(roads[2], new Vector3((-mapWidth * 5 + (housingWidthStart + x) * 10 - 5), heightCurve.Evaluate(noiseMap[housingWidthStart + x, housingHeightStart + y]) * heightMultiplier * 10 + 0.001f, (mapHeight * 5 - (housingHeightStart + y) * 10 - 7)), Quaternion.Euler(-90, 180, 0), transform.parent));

                    }
                    
                    else if (x == housingWidth - 1)
                    {
                        architectureModels.Add(Instantiate(roads[2], new Vector3((-mapWidth * 5 + (housingWidthStart + x) * 10 - 5), heightCurve.Evaluate(noiseMap[housingWidthStart + x, housingHeightStart + y]) * heightMultiplier * 10 + 0.001f, (mapHeight * 5 - (housingHeightStart + y) * 10 - 7)), Quaternion.Euler(-90, 0, 0), transform.parent));

                    }
                    else if (y == housingHeight - 2)
                    {
                        architectureModels.Add(Instantiate(roads[2], new Vector3((-mapWidth * 5 + (housingWidthStart + x) * 10 - 5), heightCurve.Evaluate(noiseMap[housingWidthStart + x, housingHeightStart + y]) * heightMultiplier * 10 + 0.001f, (mapHeight * 5 - (housingHeightStart + y) * 10 - 7)), Quaternion.Euler(-90, 90, 0), transform.parent));

                    }
                    else
                    {
                        if (y == 0 || y % 2 == 0)
                        {
                            //architectureModels.Add(Instantiate(roads[3], new Vector3((-mapWidth * 5 + (housingWidthStart + x) * 10 - 5), heightCurve.Evaluate(noiseMap[housingWidthStart + x, housingHeightStart + y]) * heightMultiplier * 10 + 0.001f, (mapHeight * 5 - (housingHeightStart + y) * 10 - 7)), Quaternion.Euler(-90, 0, 0), transform.parent));
                        }
                    }
                    

                    if (result < 2)
                    {
                        if(y == 0 || y % 2 == 0)
                            architectureModels.Add(Instantiate(houseBuildings[0], new Vector3(-mapWidth * 5 + (housingWidthStart + x) * 10, heightCurve.Evaluate(noiseMap[housingWidthStart + x, housingHeightStart + y]) * heightMultiplier * 10, mapHeight * 5 - (housingHeightStart + y) * 10), Quaternion.Euler(0, -90, 0), transform.parent));
                        else if (y % 2 == 1)
                        {
                            architectureModels.Add(Instantiate(houseBuildings[0], new Vector3(-mapWidth * 5 + (housingWidthStart + x) * 10, heightCurve.Evaluate(noiseMap[housingWidthStart + x, housingHeightStart + y]) * heightMultiplier * 10, mapHeight * 5 - (housingHeightStart + y) * 10 - 4f), Quaternion.Euler(0, 90, 0), transform.parent));
                        }
                    }
                    else if (result < 4)
                    {
                        if (y == 0 || y % 2 == 0)
                            architectureModels.Add(Instantiate(houseBuildings[1], new Vector3(-mapWidth * 5 + (housingWidthStart + x) * 10, heightCurve.Evaluate(noiseMap[housingWidthStart + x, housingHeightStart + y]) * heightMultiplier * 10, mapHeight * 5 - (housingHeightStart + y) * 10), Quaternion.Euler(0, -90, 0), transform.parent));
                        else if (y % 2 == 1)
                        {
                            architectureModels.Add(Instantiate(houseBuildings[1], new Vector3(-mapWidth * 5 + (housingWidthStart + x) * 10, heightCurve.Evaluate(noiseMap[housingWidthStart + x, housingHeightStart + y]) * heightMultiplier * 10, mapHeight * 5 - (housingHeightStart + y) * 10 - 4f), Quaternion.Euler(0, 90, 0), transform.parent));
                        }
                    }
                    else if (result < 6)
                    {
                        if (y == 0 || y % 2 == 0)
                            architectureModels.Add(Instantiate(houseBuildings[2], new Vector3(-mapWidth * 5 + (housingWidthStart + x) * 10, heightCurve.Evaluate(noiseMap[housingWidthStart + x, housingHeightStart + y]) * heightMultiplier * 10, mapHeight * 5 - (housingHeightStart + y) * 10), Quaternion.Euler(0, -90, 0), transform.parent));
                        else if (y % 2 == 1)
                        {
                            architectureModels.Add(Instantiate(houseBuildings[2], new Vector3(-mapWidth * 5 + (housingWidthStart + x) * 10, heightCurve.Evaluate(noiseMap[housingWidthStart + x, housingHeightStart + y]) * heightMultiplier * 10, mapHeight * 5 - (housingHeightStart + y) * 10 - 4f), Quaternion.Euler(0, 90, 0), transform.parent));
                        }
                    }
                    else if (result < 8)
                    {
                        if (y == 0 || y % 2 == 0)
                            architectureModels.Add(Instantiate(houseBuildings[3], new Vector3(-mapWidth * 5 + (housingWidthStart + x) * 10, heightCurve.Evaluate(noiseMap[housingWidthStart + x, housingHeightStart + y]) * heightMultiplier * 10, mapHeight * 5 - (housingHeightStart + y) * 10), Quaternion.Euler(0, -90, 0), transform.parent));
                        else if (y % 2 == 1)
                        {
                            architectureModels.Add(Instantiate(houseBuildings[3], new Vector3(-mapWidth * 5 + (housingWidthStart + x) * 10, heightCurve.Evaluate(noiseMap[housingWidthStart + x, housingHeightStart + y]) * heightMultiplier * 10, mapHeight * 5 - (housingHeightStart + y) * 10 - 4f), Quaternion.Euler(0, 90, 0), transform.parent));
                        }
                    }
                    else if (result < 10)
                    {
                        if (y == 0 || y % 2 == 0)
                            architectureModels.Add(Instantiate(houseBuildings[4], new Vector3(-mapWidth * 5 + (housingWidthStart + x) * 10, heightCurve.Evaluate(noiseMap[housingWidthStart + x, housingHeightStart + y]) * heightMultiplier * 10, mapHeight * 5 - (housingHeightStart + y) * 10), Quaternion.Euler(0, -90, 0), transform.parent));
                        else if (y % 2 == 1)
                        {
                            architectureModels.Add(Instantiate(houseBuildings[4], new Vector3(-mapWidth * 5 + (housingWidthStart + x) * 10, heightCurve.Evaluate(noiseMap[housingWidthStart + x, housingHeightStart + y]) * heightMultiplier * 10, mapHeight * 5 - (housingHeightStart + y) * 10 - 4f), Quaternion.Euler(0, 90, 0), transform.parent));
                        }
                    }
                    else if (result < 12)
                    {
                        if (y == 0 || y % 2 == 0)
                            architectureModels.Add(Instantiate(houseBuildings[5], new Vector3(-mapWidth * 5 + (housingWidthStart + x) * 10, heightCurve.Evaluate(noiseMap[housingWidthStart + x, housingHeightStart + y]) * heightMultiplier * 10, mapHeight * 5 - (housingHeightStart + y) * 10), Quaternion.Euler(0, -90, 0), transform.parent));
                        else if (y % 2 == 1)
                        {
                            architectureModels.Add(Instantiate(houseBuildings[5], new Vector3(-mapWidth * 5 + (housingWidthStart + x) * 10, heightCurve.Evaluate(noiseMap[housingWidthStart + x, housingHeightStart + y]) * heightMultiplier * 10, mapHeight * 5 - (housingHeightStart + y) * 10 - 4f), Quaternion.Euler(0, 90, 0), transform.parent));
                        }
                    }
                    else if (result < 14)
                    {
                        if (y == 0 || y % 2 == 0)
                            architectureModels.Add(Instantiate(houseBuildings[6], new Vector3(-mapWidth * 5 + (housingWidthStart + x) * 10, heightCurve.Evaluate(noiseMap[housingWidthStart + x, housingHeightStart + y]) * heightMultiplier * 10, mapHeight * 5 - (housingHeightStart + y) * 10), Quaternion.Euler(0, -90, 0), transform.parent));
                        else if (y % 2 == 1)
                        {
                            architectureModels.Add(Instantiate(houseBuildings[6], new Vector3(-mapWidth * 5 + (housingWidthStart + x) * 10, heightCurve.Evaluate(noiseMap[housingWidthStart + x, housingHeightStart + y]) * heightMultiplier * 10, mapHeight * 5 - (housingHeightStart + y) * 10 - 4f), Quaternion.Euler(0, 90, 0), transform.parent));
                        }
                    }
                    else if (result < 16)
                    {
                        if (y == 0 || y % 2 == 0)
                            architectureModels.Add(Instantiate(houseBuildings[7], new Vector3(-mapWidth * 5 + (housingWidthStart + x) * 10, heightCurve.Evaluate(noiseMap[housingWidthStart + x, housingHeightStart + y]) * heightMultiplier * 10, mapHeight * 5 - (housingHeightStart + y) * 10), Quaternion.Euler(0, -90, 0), transform.parent));
                        else if (y % 2 == 1)
                        {
                            architectureModels.Add(Instantiate(houseBuildings[7], new Vector3(-mapWidth * 5 + (housingWidthStart + x) * 10, heightCurve.Evaluate(noiseMap[housingWidthStart + x, housingHeightStart + y]) * heightMultiplier * 10, mapHeight * 5 - (housingHeightStart + y) * 10 - 4f), Quaternion.Euler(0, 90, 0), transform.parent));
                        }
                    }
                    else if (result < 18)
                    {
                        if (y == 0 || y % 2 == 0)
                            architectureModels.Add(Instantiate(houseBuildings[8], new Vector3(-mapWidth * 5 + (housingWidthStart + x) * 10, heightCurve.Evaluate(noiseMap[housingWidthStart + x, housingHeightStart + y]) * heightMultiplier * 10, mapHeight * 5 - (housingHeightStart + y) * 10), Quaternion.Euler(0, -90, 0), transform.parent));
                        else if (y % 2 == 1)
                        {
                            architectureModels.Add(Instantiate(houseBuildings[8], new Vector3(-mapWidth * 5 + (housingWidthStart + x) * 10, heightCurve.Evaluate(noiseMap[housingWidthStart + x, housingHeightStart + y]) * heightMultiplier * 10, mapHeight * 5 - (housingHeightStart + y) * 10 - 4f), Quaternion.Euler(0, 90, 0), transform.parent));
                        }
                    }
                    else if (result < 20)
                    {
                        if (y == 0 || y % 2 == 0)
                            architectureModels.Add(Instantiate(houseBuildings[9], new Vector3(-mapWidth * 5 + (housingWidthStart + x) * 10, heightCurve.Evaluate(noiseMap[housingWidthStart + x, housingHeightStart + y]) * heightMultiplier * 10, mapHeight * 5 - (housingHeightStart + y) * 10), Quaternion.Euler(0, -90, 0), transform.parent));
                        else if (y % 2 == 1)
                        {
                            architectureModels.Add(Instantiate(houseBuildings[9], new Vector3(-mapWidth * 5 + (housingWidthStart + x) * 10, heightCurve.Evaluate(noiseMap[housingWidthStart + x, housingHeightStart + y]) * heightMultiplier * 10, mapHeight * 5 - (housingHeightStart + y) * 10 - 4f), Quaternion.Euler(0, 90, 0), transform.parent));
                        }
                    }
                    else if (result < 22)
                    {
                        if (y == 0 || y % 2 == 0)
                            architectureModels.Add(Instantiate(houseBuildings[10], new Vector3(-mapWidth * 5 + (housingWidthStart + x) * 10, heightCurve.Evaluate(noiseMap[housingWidthStart + x, housingHeightStart + y]) * heightMultiplier * 10, mapHeight * 5 - (housingHeightStart + y) * 10), Quaternion.Euler(0, -90, 0), transform.parent));
                        else if (y % 2 == 1)
                        {
                            architectureModels.Add(Instantiate(houseBuildings[10], new Vector3(-mapWidth * 5 + (housingWidthStart + x) * 10, heightCurve.Evaluate(noiseMap[housingWidthStart + x, housingHeightStart + y]) * heightMultiplier * 10, mapHeight * 5 - (housingHeightStart + y) * 10 - 4f), Quaternion.Euler(0, 90, 0), transform.parent));
                        }
                    }
                    else if (result < 24)
                    {
                        if (y == 0 || y % 2 == 0)
                            architectureModels.Add(Instantiate(houseBuildings[11], new Vector3(-mapWidth * 5 + (housingWidthStart + x) * 10, heightCurve.Evaluate(noiseMap[housingWidthStart + x, housingHeightStart + y]) * heightMultiplier * 10, mapHeight * 5 - (housingHeightStart + y) * 10), Quaternion.Euler(0, -90, 0), transform.parent));
                        else if (y % 2 == 1)
                        {
                            architectureModels.Add(Instantiate(houseBuildings[11], new Vector3(-mapWidth * 5 + (housingWidthStart + x) * 10, heightCurve.Evaluate(noiseMap[housingWidthStart + x, housingHeightStart + y]) * heightMultiplier * 10, mapHeight * 5 - (housingHeightStart + y) * 10 -4f), Quaternion.Euler(0, 90, 0), transform.parent));
                        }
                    }
                }          
            }
        }
    }

}
