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
    //GameObject newHouse;
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
                noiseMapBlocked[housingWidthStart + x, housingHeightStart + y] = 1;
            }
        }
        GenerateVegetation();
    }

    public void GenerateVegetation()
    {

        System.Random rng = new System.Random();
        int treeStart1X = rng.Next(50, mapWidth - 50);
        int treeStart1Y = rng.Next(50, mapHeight - 50);

        int treeStart2X = rng.Next(50 , mapWidth - 50);
        int treeStart2Y = rng.Next(50, mapHeight - 50);

        int treeStart3X = rng.Next(50, mapWidth - 50);
        int treeStart3Y = rng.Next(50 , mapHeight - 50);

        if(noiseMapBlocked[treeStart1X,treeStart1Y] == 1)
        {
            for (int i = 0; i < 100000; i++)
            {
                treeStart1X = rng.Next(50, mapWidth - 50);
                treeStart1Y = rng.Next(50, mapHeight - 50);

                if(noiseMapBlocked[treeStart1X, treeStart1Y] == 0)
                {
                    i = 100000;
                }
            }
        }
        if (noiseMapBlocked[treeStart2X, treeStart2Y] == 1)
        {
            for (int i = 0; i < 100000; i++)
            {
                treeStart1X = rng.Next(50, mapWidth - 50);
                treeStart1Y = rng.Next(50, mapHeight - 50);

                if (noiseMapBlocked[treeStart1X, treeStart1Y] == 0)
                {
                    i = 100000;
                }
            }
        }
        if (noiseMapBlocked[treeStart3X, treeStart3Y] == 1)
        {
            for (int i = 0; i < 100000; i++)
            {
                treeStart1X = rng.Next(50, mapWidth - 50);
                treeStart1Y = rng.Next(50, mapHeight - 50);

                if (noiseMapBlocked[treeStart1X, treeStart1Y] == 0)
                {
                    i = 100000;
                }
            }
        }

        int[] treeStarts = new int[6];

        treeStarts[0] = treeStart1X;
        treeStarts[1] = treeStart1Y;
        treeStarts[2] = treeStart2X;
        treeStarts[3] = treeStart2Y;
        treeStarts[4] = treeStart3X;
        treeStarts[5] = treeStart3Y;

        for (int i = 0; i < 6; i+=2)
        {
            int randomTree = rng.Next(0, 10);
            architectureModels.Add(Instantiate(vegetation[randomTree], new Vector3(-mapWidth * 5 + treeStarts[i] * 10, heightCurve.Evaluate(noiseMap[treeStarts[i], treeStarts[i + 1]]) * heightMultiplier * 10 - 0.5f, mapHeight * 5 - treeStarts[i + 1] * 10), Quaternion.Euler(0,0,0), transform.parent));
            int listCount = architectureModels.Count;
            GameObject lastTree = architectureModels[listCount - 1];
            lastTree.transform.localScale = new Vector3(2, 2, 2);
            noiseMapBlocked[treeStarts[i], treeStarts[i + 1]] = 1;
        }

        for(int i = 0; i < 6; i += 2)
        {
            for(int j = treeStarts[i] - 5; j < treeStarts[i] + 5; j++)
            {
                for(int k = treeStarts[i+1] - 5; k < treeStarts[i+1] + 5; k++)
                {
                    if (noiseMapBlocked[j, k] == 0)
                    {
                        int randomTree = rng.Next(0, 10);
                        int chanceOfSpawn = 0;
                        int listCount = 0;
                        Debug.Log("Inside nested for loops");

                        if (j == treeStarts[i] - 5 || k == treeStarts[i + 1] - 5 || j == treeStarts[i] + 5 || k == treeStarts[i + 1] + 5)
                        {
                            chanceOfSpawn = rng.Next(0, 100);
                            Debug.Log(chanceOfSpawn);
                            if (chanceOfSpawn > 80)
                            {
                                architectureModels.Add(Instantiate(vegetation[randomTree], new Vector3(-mapWidth * 5 + j * 10, heightCurve.Evaluate(noiseMap[treeStarts[i], treeStarts[i + 1]]) * heightMultiplier * 10 - 0.5f, mapHeight * 5 - k * 10), Quaternion.Euler(0, 0, 0), transform.parent));

                                listCount = architectureModels.Count;
                                GameObject lastTree = architectureModels[listCount - 1];
                                lastTree.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                                noiseMapBlocked[treeStarts[i], treeStarts[i + 1]] = 1;
                            }

                        }
                        else if (j == treeStarts[i] - 4 || k == treeStarts[i + 1] - 4 || j == treeStarts[i] + 4 || k == treeStarts[i + 1] + 4)
                        {
                            chanceOfSpawn = rng.Next(0, 100);
                            Debug.Log(chanceOfSpawn);
                            if (chanceOfSpawn > 60)
                            {
                                architectureModels.Add(Instantiate(vegetation[randomTree], new Vector3(-mapWidth * 5 + j * 10, heightCurve.Evaluate(noiseMap[treeStarts[i], treeStarts[i + 1]]) * heightMultiplier * 10 - 0.5f, mapHeight * 5 - k * 10), Quaternion.Euler(0, 0, 0), transform.parent));

                                listCount = architectureModels.Count;
                                GameObject lastTree = architectureModels[listCount - 1];
                                lastTree.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                                noiseMapBlocked[treeStarts[i], treeStarts[i + 1]] = 1;
                            }
                        }
                        else if (j == treeStarts[i] - 3 || k == treeStarts[i + 1] - 3 || j == treeStarts[i] + 3 || k == treeStarts[i + 1] + 3)
                        {
                            chanceOfSpawn = rng.Next(0, 100);
                            Debug.Log(chanceOfSpawn);
                            if (chanceOfSpawn > 40)
                            {
                                architectureModels.Add(Instantiate(vegetation[randomTree], new Vector3(-mapWidth * 5 + j * 10, heightCurve.Evaluate(noiseMap[treeStarts[i], treeStarts[i + 1]]) * heightMultiplier * 10 - 0.5f, mapHeight * 5 - k * 10), Quaternion.Euler(0, 0, 0), transform.parent));

                                listCount = architectureModels.Count;
                                GameObject lastTree = architectureModels[listCount - 1];
                                lastTree.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
                                noiseMapBlocked[treeStarts[i], treeStarts[i + 1]] = 1;
                            }
                        }
                        else if (j == treeStarts[i] - 2 || k == treeStarts[i + 1] - 2 || j == treeStarts[i] + 2 || k == treeStarts[i + 1] + 2)
                        {
                            chanceOfSpawn = rng.Next(0, 100);
                            Debug.Log(chanceOfSpawn);
                            if (chanceOfSpawn > 20)
                            {
                                architectureModels.Add(Instantiate(vegetation[randomTree], new Vector3(-mapWidth * 5 + j * 10, heightCurve.Evaluate(noiseMap[treeStarts[i], treeStarts[i + 1]]) * heightMultiplier * 10 - 0.5f, mapHeight * 5 - k * 10), Quaternion.Euler(0, 0, 0), transform.parent));

                                listCount = architectureModels.Count;
                                GameObject lastTree = architectureModels[listCount - 1];
                                lastTree.transform.localScale = new Vector3(1.4f, 1.4f, 1.4f);
                                noiseMapBlocked[treeStarts[i], treeStarts[i + 1]] = 1;
                            }
                        }
                        else if (j == treeStarts[i] - 1 || k == treeStarts[i + 1] - 1 || j == treeStarts[i] + 1 || k == treeStarts[i + 1] + 1)
                        {
                            chanceOfSpawn = rng.Next(0, 100);
                            Debug.Log(chanceOfSpawn);
                            if (chanceOfSpawn > 0)
                            {
                                architectureModels.Add(Instantiate(vegetation[randomTree], new Vector3(-mapWidth * 5 + j * 10, heightCurve.Evaluate(noiseMap[treeStarts[i], treeStarts[i + 1]]) * heightMultiplier * 10 - 0.5f, mapHeight * 5 - k * 10), Quaternion.Euler(0, 0, 0), transform.parent));

                                listCount = architectureModels.Count;
                                GameObject lastTree = architectureModels[listCount - 1];
                                lastTree.transform.localScale = new Vector3(1.7f, 1.7f, 1.7f);
                                noiseMapBlocked[treeStarts[i], treeStarts[i + 1]] = 1;
                            }
                        }
                        else
                        {

                        }
                    }
                }
            }
        }

    }

}
