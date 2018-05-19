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
    public GameObject[] flowers;

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

        //initialising list to allow for deleting from list
        architectureModels.Add(Instantiate(housePrefab, new Vector3(-mapWidth * 5 + housingWidthStart * 10, noiseMap[housingWidthStart, housingHeightStart] * heightMultiplier / 2 + 0.1f, mapHeight * 5 - housingHeightStart * 10), Quaternion.Euler(0, 0, 0), transform.parent));

        //On start up of the project, whenever the project has been re-opened *only on a restart of pc or on new pc* please come to this script and comment out the 3 lines following this comment, save the file, go back to Unity and wait for it to load in the script, come back to this script and uncomment these lines, 
        //save and the generating of the architecture and vegetation should work.
        if (architectureModels.Count > 0)
            foreach(GameObject m in architectureModels)
                DestroyImmediate(m);


        
        //spawn houses
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
        //find suitable positions for ancestor trees
        System.Random rng = new System.Random();
        int treeStart1X = rng.Next(21, mapWidth - 21);
        int treeStart1Y = rng.Next(21, mapHeight - 21);

        int treeStart2X = rng.Next(21 , mapWidth - 21);
        int treeStart2Y = rng.Next(21, mapHeight - 21);

        int treeStart3X = rng.Next(21, mapWidth - 21);
        int treeStart3Y = rng.Next(21 , mapHeight - 21);

        if(noiseMapBlocked[treeStart1X,treeStart1Y] == 1)
        {
            for (int i = 0; i < 1000000; i++)
            {
                treeStart1X = rng.Next(21, mapWidth - 21);
                treeStart1Y = rng.Next(21, mapHeight - 21);

                if(noiseMapBlocked[treeStart1X, treeStart1Y] == 0)
                {
                    i = 1000000;
                }
            }
        }
        if (noiseMapBlocked[treeStart2X, treeStart2Y] == 1)
        {
            for (int i = 0; i < 1000000; i++)
            {
                treeStart2X = rng.Next(21, mapWidth - 21);
                treeStart2Y = rng.Next(21, mapHeight - 21);

                if (noiseMapBlocked[treeStart2X, treeStart2Y] == 0)
                {
                    i = 1000000;
                }
            }
        }
        if (noiseMapBlocked[treeStart3X, treeStart3Y] == 1)
        {
            for (int i = 0; i < 1000000; i++)
            {
                treeStart3X = rng.Next(21, mapWidth - 21);
                treeStart3Y = rng.Next(21, mapHeight - 21);

                if (noiseMapBlocked[treeStart3X, treeStart3Y] == 0)
                {
                    i = 1000000;
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

        int listCount = 0;
        int randomTree = 0;
        float randomTreeScaleVariance = 0;
        double treePositionVarianceX = 0;
        double treePositionVarianceY = 0;
        double randomTreeRotation = 0;
        float treeRotation = 0;
        GameObject lastTree = null;

        //generate 3 ancestor trees
        for (int i = 0; i < 6; i+=2)
        {
            randomTree = rng.Next(0, 10);
            randomTreeRotation = rng.NextDouble() * 360;
            treeRotation = (float)randomTreeRotation;
            architectureModels.Add(Instantiate(vegetation[randomTree], new Vector3(-mapWidth * 5 + treeStarts[i] * 10, heightCurve.Evaluate(noiseMap[treeStarts[i], treeStarts[i + 1]]) * heightMultiplier * 10 - 0.5f, mapHeight * 5 - treeStarts[i + 1] * 10), Quaternion.Euler(0, treeRotation, 0), transform.parent));
            listCount = architectureModels.Count;
            lastTree = architectureModels[listCount - 1];
             randomTreeScaleVariance = (float) rng.Next(18000, 22000) / 10000;
            lastTree.transform.localScale = new Vector3(randomTreeScaleVariance, randomTreeScaleVariance, randomTreeScaleVariance);
            noiseMapBlocked[treeStarts[i], treeStarts[i + 1]] = 1;
        }
        //generate forests
        for(int i = 0; i < 6; i += 2)
        {
            for(int j = treeStarts[i] - 20; j < treeStarts[i] + 20; j++)
            {
                for(int k = treeStarts[i+1] - 20; k < treeStarts[i+1] + 20; k++)
                {
                    if (noiseMapBlocked[j, k] == 0)
                    {
                        randomTree = rng.Next(0, 10);
                        int chanceOfSpawn = 0;
                        listCount = 0;
                        randomTreeRotation = rng.NextDouble() * 360;
                        treeRotation = (float)randomTreeRotation;
                        randomTreeScaleVariance = 0;
                        Debug.Log("Inside nested for loops");

                        treePositionVarianceX = rng.NextDouble() * 3;
                        treePositionVarianceY = rng.NextDouble() * 3;
                        int positionPosNegChance = rng.Next(0, 100);
                        if (positionPosNegChance < 50)
                        {
                            treePositionVarianceX *= -1;
                        }
                        positionPosNegChance = rng.Next(0, 100);
                        if (positionPosNegChance < 50)
                        {
                            treePositionVarianceY *= -1;
                        }

                        if (j == treeStarts[i] - 20 || k == treeStarts[i + 1] - 20 || j == treeStarts[i] + 20 || k == treeStarts[i + 1] + 20)
                        {
                            chanceOfSpawn = rng.Next(0, 100);
                            Debug.Log(chanceOfSpawn);
                            if (chanceOfSpawn > 92)
                            {
                                
                                architectureModels.Add(Instantiate(vegetation[randomTree], new Vector3(-mapWidth * 5 + j * 10 + (float) treePositionVarianceX, heightCurve.Evaluate(noiseMap[j, k]) * heightMultiplier * 10 - 0.5f, mapHeight * 5 - k * 10 - (float) treePositionVarianceY), Quaternion.Euler(0, treeRotation, 0), transform.parent));
                                listCount = architectureModels.Count;
                                lastTree = architectureModels[listCount - 1];
                                randomTreeScaleVariance = (float) rng.Next(8500, 9500) / 10000;
                                
                            }

                        }
                        if (j == treeStarts[i] - 19 || k == treeStarts[i + 1] - 19 || j == treeStarts[i] + 19 || k == treeStarts[i + 1] + 19)
                        {
                            chanceOfSpawn = rng.Next(0, 100);
                            Debug.Log(chanceOfSpawn);
                            if (chanceOfSpawn > 88)
                            {

                                architectureModels.Add(Instantiate(vegetation[randomTree], new Vector3(-mapWidth * 5 + j * 10 + (float)treePositionVarianceX, heightCurve.Evaluate(noiseMap[j, k]) * heightMultiplier * 10 - 0.5f, mapHeight * 5 - k * 10 - (float)treePositionVarianceY), Quaternion.Euler(0, treeRotation, 0), transform.parent));
                                listCount = architectureModels.Count;
                                lastTree = architectureModels[listCount - 1];
                                randomTreeScaleVariance = (float) rng.Next(9000, 10000) / 10000;
                                
                            }

                        }
                        if (j == treeStarts[i] - 18 || k == treeStarts[i + 1] - 18 || j == treeStarts[i] + 18 || k == treeStarts[i + 1] + 18)
                        {
                            chanceOfSpawn = rng.Next(0, 100);
                            Debug.Log(chanceOfSpawn);
                            if (chanceOfSpawn > 84)
                            {
                                architectureModels.Add(Instantiate(vegetation[randomTree], new Vector3(-mapWidth * 5 + j * 10 + (float)treePositionVarianceX, heightCurve.Evaluate(noiseMap[j, k]) * heightMultiplier * 10 - 0.5f, mapHeight * 5 - k * 10 - (float)treePositionVarianceY), Quaternion.Euler(0, treeRotation, 0), transform.parent));
                                listCount = architectureModels.Count;
                                lastTree = architectureModels[listCount - 1];
                                randomTreeScaleVariance = (float) rng.Next(9500, 10500) / 10000;
                                
                            }

                        }
                        if (j == treeStarts[i] - 17 || k == treeStarts[i + 1] - 17 || j == treeStarts[i] + 17 || k == treeStarts[i + 1] + 17)
                        {
                            chanceOfSpawn = rng.Next(0, 100);
                            Debug.Log(chanceOfSpawn);
                            if (chanceOfSpawn > 80)
                            {
                                architectureModels.Add(Instantiate(vegetation[randomTree], new Vector3(-mapWidth * 5 + j * 10 + (float)treePositionVarianceX, heightCurve.Evaluate(noiseMap[j, k]) * heightMultiplier * 10 - 0.5f, mapHeight * 5 - k * 10 - (float)treePositionVarianceY), Quaternion.Euler(0, treeRotation, 0), transform.parent));
                                listCount = architectureModels.Count;
                                lastTree = architectureModels[listCount - 1];
                                randomTreeScaleVariance = (float) rng.Next(10000, 11000) / 10000;
                                
                            }

                        }
                        if (j == treeStarts[i] - 16 || k == treeStarts[i + 1] - 16 || j == treeStarts[i] + 16 || k == treeStarts[i + 1] + 16)
                        {
                            chanceOfSpawn = rng.Next(0, 100);
                            Debug.Log(chanceOfSpawn);
                            if (chanceOfSpawn > 76)
                            {
                                architectureModels.Add(Instantiate(vegetation[randomTree], new Vector3(-mapWidth * 5 + j * 10 + (float)treePositionVarianceX, heightCurve.Evaluate(noiseMap[j, k]) * heightMultiplier * 10 - 0.5f, mapHeight * 5 - k * 10 - (float)treePositionVarianceY), Quaternion.Euler(0, treeRotation, 0), transform.parent));
                                listCount = architectureModels.Count;
                                lastTree = architectureModels[listCount - 1];
                                randomTreeScaleVariance = (float) rng.Next(10500, 11500) / 10000;
                                
                            }

                        }
                        if (j == treeStarts[i] - 15 || k == treeStarts[i + 1] - 15 || j == treeStarts[i] + 15 || k == treeStarts[i + 1] + 15)
                        {
                            chanceOfSpawn = rng.Next(0, 100);
                            Debug.Log(chanceOfSpawn);
                            if (chanceOfSpawn > 72)
                            {
                                architectureModels.Add(Instantiate(vegetation[randomTree], new Vector3(-mapWidth * 5 + j * 10 + (float)treePositionVarianceX, heightCurve.Evaluate(noiseMap[j, k]) * heightMultiplier * 10 - 0.5f, mapHeight * 5 - k * 10 - (float)treePositionVarianceY), Quaternion.Euler(0, treeRotation, 0), transform.parent));
                                listCount = architectureModels.Count;
                                lastTree = architectureModels[listCount - 1];
                                randomTreeScaleVariance = (float) rng.Next(11000, 12000) / 10000;
                                
                            }

                        }
                        if (j == treeStarts[i] - 14 || k == treeStarts[i + 1] - 14 || j == treeStarts[i] + 14 || k == treeStarts[i + 1] + 14)
                        {
                            chanceOfSpawn = rng.Next(0, 100);
                            Debug.Log(chanceOfSpawn);
                            if (chanceOfSpawn > 68)
                            {
                                architectureModels.Add(Instantiate(vegetation[randomTree], new Vector3(-mapWidth * 5 + j * 10 + (float)treePositionVarianceX, heightCurve.Evaluate(noiseMap[j, k]) * heightMultiplier * 10 - 0.5f, mapHeight * 5 - k * 10 - (float)treePositionVarianceY), Quaternion.Euler(0, treeRotation, 0), transform.parent));
                                listCount = architectureModels.Count;
                                lastTree = architectureModels[listCount - 1];
                                randomTreeScaleVariance = (float) rng.Next(11500, 12500) / 10000;
                                
                            }

                        }
                        if (j == treeStarts[i] - 13 || k == treeStarts[i + 1] - 13 || j == treeStarts[i] + 13 || k == treeStarts[i + 1] + 13)
                        {
                            chanceOfSpawn = rng.Next(0, 100);
                            Debug.Log(chanceOfSpawn);
                            if (chanceOfSpawn > 64)
                            {
                                architectureModels.Add(Instantiate(vegetation[randomTree], new Vector3(-mapWidth * 5 + j * 10 + (float)treePositionVarianceX, heightCurve.Evaluate(noiseMap[j, k]) * heightMultiplier * 10 - 0.5f, mapHeight * 5 - k * 10 - (float)treePositionVarianceY), Quaternion.Euler(0, treeRotation, 0), transform.parent));
                                listCount = architectureModels.Count;
                                lastTree = architectureModels[listCount - 1];
                                randomTreeScaleVariance = (float) rng.Next(12000, 13000) / 10000;
                                
                            }

                        }
                        if (j == treeStarts[i] - 12 || k == treeStarts[i + 1] - 12 || j == treeStarts[i] + 12 || k == treeStarts[i + 1] + 12)
                        {
                            chanceOfSpawn = rng.Next(0, 100);
                            Debug.Log(chanceOfSpawn);
                            if (chanceOfSpawn > 60)
                            {
                                architectureModels.Add(Instantiate(vegetation[randomTree], new Vector3(-mapWidth * 5 + j * 10 + (float)treePositionVarianceX, heightCurve.Evaluate(noiseMap[j, k]) * heightMultiplier * 10 - 0.5f, mapHeight * 5 - k * 10 - (float)treePositionVarianceY), Quaternion.Euler(0, treeRotation, 0), transform.parent));
                                listCount = architectureModels.Count;
                                lastTree = architectureModels[listCount - 1];
                                randomTreeScaleVariance = (float) rng.Next(12500, 13500) / 10000;
                                
                            }

                        }
                        if (j == treeStarts[i] - 11 || k == treeStarts[i + 1] - 11 || j == treeStarts[i] + 11 || k == treeStarts[i + 1] + 11)
                        {
                            chanceOfSpawn = rng.Next(0, 100);
                            Debug.Log(chanceOfSpawn);
                            if (chanceOfSpawn > 56)
                            {
                                architectureModels.Add(Instantiate(vegetation[randomTree], new Vector3(-mapWidth * 5 + j * 10 + (float)treePositionVarianceX, heightCurve.Evaluate(noiseMap[j, k]) * heightMultiplier * 10 - 0.5f, mapHeight * 5 - k * 10 - (float)treePositionVarianceY), Quaternion.Euler(0, treeRotation, 0), transform.parent));
                                listCount = architectureModels.Count;
                                lastTree = architectureModels[listCount - 1];
                                randomTreeScaleVariance = (float) rng.Next(13000, 14000) / 10000;
                                
                            }

                        }
                        if (j == treeStarts[i] - 10 || k == treeStarts[i + 1] - 10 || j == treeStarts[i] + 10 || k == treeStarts[i + 1] + 10)
                        {
                            chanceOfSpawn = rng.Next(0, 100);
                            Debug.Log(chanceOfSpawn);
                            if (chanceOfSpawn > 52)
                            {
                                architectureModels.Add(Instantiate(vegetation[randomTree], new Vector3(-mapWidth * 5 + j * 10 + (float)treePositionVarianceX, heightCurve.Evaluate(noiseMap[j, k]) * heightMultiplier * 10 - 0.5f, mapHeight * 5 - k * 10 - (float)treePositionVarianceY), Quaternion.Euler(0, treeRotation, 0), transform.parent));
                                listCount = architectureModels.Count;
                                lastTree = architectureModels[listCount - 1];
                                randomTreeScaleVariance = (float) rng.Next(13500, 14500) / 10000;
                               
                            }

                        }
                        if (j == treeStarts[i] - 9 || k == treeStarts[i + 1] - 9 || j == treeStarts[i] + 9 || k == treeStarts[i + 1] + 9)
                        {
                            chanceOfSpawn = rng.Next(0, 100);
                            Debug.Log(chanceOfSpawn);
                            if (chanceOfSpawn > 48)
                            {
                                architectureModels.Add(Instantiate(vegetation[randomTree], new Vector3(-mapWidth * 5 + j * 10 + (float)treePositionVarianceX, heightCurve.Evaluate(noiseMap[j, k]) * heightMultiplier * 10 - 0.5f, mapHeight * 5 - k * 10 - (float)treePositionVarianceY), Quaternion.Euler(0, treeRotation, 0), transform.parent));
                                listCount = architectureModels.Count;
                                lastTree = architectureModels[listCount - 1];
                                randomTreeScaleVariance = (float) rng.Next(14000, 15000) / 10000;
                                
                            }

                        }
                        if (j == treeStarts[i] - 8 || k == treeStarts[i + 1] - 8 || j == treeStarts[i] + 8 || k == treeStarts[i + 1] + 8)
                        {
                            chanceOfSpawn = rng.Next(0, 100);
                            Debug.Log(chanceOfSpawn);
                            if (chanceOfSpawn > 44)
                            {
                                architectureModels.Add(Instantiate(vegetation[randomTree], new Vector3(-mapWidth * 5 + j * 10 + (float)treePositionVarianceX, heightCurve.Evaluate(noiseMap[j, k]) * heightMultiplier * 10 - 0.5f, mapHeight * 5 - k * 10 - (float)treePositionVarianceY), Quaternion.Euler(0, treeRotation, 0), transform.parent));
                                listCount = architectureModels.Count;
                                lastTree = architectureModels[listCount - 1];
                                randomTreeScaleVariance = (float) rng.Next(14500, 15500) / 10000;
                               
                            }

                        }
                        if (j == treeStarts[i] - 7 || k == treeStarts[i + 1] - 7 || j == treeStarts[i] + 7 || k == treeStarts[i + 1] + 7)
                        {
                            chanceOfSpawn = rng.Next(0, 100);
                            Debug.Log(chanceOfSpawn);
                            if (chanceOfSpawn > 40)
                            {
                                architectureModels.Add(Instantiate(vegetation[randomTree], new Vector3(-mapWidth * 5 + j * 10 + (float)treePositionVarianceX, heightCurve.Evaluate(noiseMap[j, k]) * heightMultiplier * 10 - 0.5f, mapHeight * 5 - k * 10 - (float)treePositionVarianceY), Quaternion.Euler(0, treeRotation, 0), transform.parent));
                                listCount = architectureModels.Count;
                                lastTree = architectureModels[listCount - 1];
                                randomTreeScaleVariance = (float)rng.Next(15000, 16000) / 10000;
                                
                            }

                        }
                        if (j == treeStarts[i] - 6 || k == treeStarts[i + 1] - 6 || j == treeStarts[i] + 6 || k == treeStarts[i + 1] + 6)
                        {
                            chanceOfSpawn = rng.Next(0, 100);
                            Debug.Log(chanceOfSpawn);
                            if (chanceOfSpawn > 36)
                            {
                                architectureModels.Add(Instantiate(vegetation[randomTree], new Vector3(-mapWidth * 5 + j * 10 + (float)treePositionVarianceX, heightCurve.Evaluate(noiseMap[j, k]) * heightMultiplier * 10 - 0.5f, mapHeight * 5 - k * 10 - (float)treePositionVarianceY), Quaternion.Euler(0, treeRotation, 0), transform.parent));
                                listCount = architectureModels.Count;
                                lastTree = architectureModels[listCount - 1];
                                randomTreeScaleVariance = (float) rng.Next(15500, 16500) / 10000;
                                
                            }

                        }
                        else if (j == treeStarts[i] - 4 || k == treeStarts[i + 1] - 4 || j == treeStarts[i] + 4 || k == treeStarts[i + 1] + 4)
                        {
                            chanceOfSpawn = rng.Next(0, 100);
                            Debug.Log(chanceOfSpawn);
                            if (chanceOfSpawn > 32)
                            {
                                architectureModels.Add(Instantiate(vegetation[randomTree], new Vector3(-mapWidth * 5 + j * 10 + (float)treePositionVarianceX, heightCurve.Evaluate(noiseMap[j, k]) * heightMultiplier * 10 - 0.5f, mapHeight * 5 - k * 10 - (float)treePositionVarianceY), Quaternion.Euler(0, treeRotation, 0), transform.parent));
                                listCount = architectureModels.Count;
                                lastTree = architectureModels[listCount - 1];
                                randomTreeScaleVariance = (float) rng.Next(16000, 17000) / 10000;
                                
                            }
                        }
                        else if (j == treeStarts[i] - 3 || k == treeStarts[i + 1] - 3 || j == treeStarts[i] + 3 || k == treeStarts[i + 1] + 3)
                        {
                            chanceOfSpawn = rng.Next(0, 100);
                            Debug.Log(chanceOfSpawn);
                            if (chanceOfSpawn > 28)
                            {
                                architectureModels.Add(Instantiate(vegetation[randomTree], new Vector3(-mapWidth * 5 + j * 10 + (float)treePositionVarianceX, heightCurve.Evaluate(noiseMap[j, k]) * heightMultiplier * 10 - 0.5f, mapHeight * 5 - k * 10 - (float)treePositionVarianceY), Quaternion.Euler(0, treeRotation, 0), transform.parent));
                                listCount = architectureModels.Count;
                                lastTree = architectureModels[listCount - 1];
                                randomTreeScaleVariance = (float) rng.Next(16500, 17500) / 10000;
                                
                            }
                        }
                        else if (j == treeStarts[i] - 2 || k == treeStarts[i + 1] - 2 || j == treeStarts[i] + 2 || k == treeStarts[i + 1] + 2)
                        {
                            chanceOfSpawn = rng.Next(0, 100);
                            Debug.Log(chanceOfSpawn);
                            if (chanceOfSpawn > 24)
                            {
                                architectureModels.Add(Instantiate(vegetation[randomTree], new Vector3(-mapWidth * 5 + j * 10 + (float)treePositionVarianceX, heightCurve.Evaluate(noiseMap[j, k]) * heightMultiplier * 10 - 0.5f, mapHeight * 5 - k * 10 - (float)treePositionVarianceY), Quaternion.Euler(0, treeRotation, 0), transform.parent));
                                listCount = architectureModels.Count;
                                lastTree = architectureModels[listCount - 1];
                                randomTreeScaleVariance = (float) rng.Next(17000, 18000) / 10000;
                               
                            }
                        }
                        else if (j == treeStarts[i] - 1 || k == treeStarts[i + 1] - 1 || j == treeStarts[i] + 1 || k == treeStarts[i + 1] + 1)
                        {
                            chanceOfSpawn = rng.Next(0, 100);
                            Debug.Log(chanceOfSpawn);
                            if (chanceOfSpawn > 20)
                            {
                                architectureModels.Add(Instantiate(vegetation[randomTree], new Vector3(-mapWidth * 5 + j * 10 + (float)treePositionVarianceX, heightCurve.Evaluate(noiseMap[j, k]) * heightMultiplier * 10 - 0.5f, mapHeight * 5 - k * 10 - (float)treePositionVarianceY), Quaternion.Euler(0, treeRotation, 0), transform.parent));
                                listCount = architectureModels.Count;
                                lastTree = architectureModels[listCount - 1];
                                randomTreeScaleVariance = (float) rng.Next(17500, 18500) / 10000;
                                
                            }
                        }
                        else
                        {

                        }

                        lastTree.transform.localScale = new Vector3(randomTreeScaleVariance, randomTreeScaleVariance, randomTreeScaleVariance);
                        noiseMapBlocked[treeStarts[i], treeStarts[i + 1]] = 1;
                    }
                }
            }
        }
        //generate trees outside of forests
        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                if(noiseMapBlocked[i,j] == 0)
                {
                    int chanceToSpawn = rng.Next(0, 100);

                    if (chanceToSpawn <= 6)
                    {
                        randomTree = rng.Next(0, 10);
                        listCount = 0;
                        randomTreeRotation = rng.NextDouble() * 360;
                        treeRotation = (float)randomTreeRotation;
                        randomTreeScaleVariance = 0;

                        treePositionVarianceX = rng.NextDouble() * 3;
                        treePositionVarianceY = rng.NextDouble() * 3;
                        int positionPosNegChance = rng.Next(0, 100);
                        if (positionPosNegChance < 50)
                        {
                            treePositionVarianceX *= -1;
                        }
                        positionPosNegChance = rng.Next(0, 100);
                        if (positionPosNegChance < 50)
                        {
                            treePositionVarianceY *= -1;
                        }

                        architectureModels.Add(Instantiate(vegetation[randomTree], new Vector3(-mapWidth * 5 + i * 10 + (float)treePositionVarianceX, heightCurve.Evaluate(noiseMap[i, j]) * heightMultiplier * 10 - 0.5f, mapHeight * 5 - j * 10 - (float)treePositionVarianceY), Quaternion.Euler(0, treeRotation, 0), transform.parent));
                        listCount = architectureModels.Count;
                        lastTree = architectureModels[listCount - 1];
                        randomTreeScaleVariance = (float)rng.Next(14500, 15500) / 10000;
                        lastTree.transform.localScale = new Vector3(randomTreeScaleVariance, randomTreeScaleVariance, randomTreeScaleVariance);
                        noiseMapBlocked[i,j] = 1;

                        for (int k = -2; k < 2; k++)
                        {
                            for (int l = -2; l < 2; l++)
                            {

                                if (i > 3 && j > 3 && i < mapWidth - 3 && j < mapHeight - 3 && noiseMapBlocked[i + k, j + l] == 0)
                                {
                                    chanceToSpawn = rng.Next(0, 100);
                                    
                                    if (chanceToSpawn < 75)
                                    {
                                        
                                        randomTree = rng.Next(0, 10);
                                        listCount = 0;
                                        randomTreeRotation = rng.NextDouble() * 360;
                                        treeRotation = (float)randomTreeRotation;
                                        randomTreeScaleVariance = 0;
                                        //Debug.Log("Inside nested for loops");

                                        treePositionVarianceX = rng.NextDouble() * 3;
                                        treePositionVarianceY = rng.NextDouble() * 3;
                                        positionPosNegChance = rng.Next(0, 100);
                                        if (positionPosNegChance < 50)
                                        {
                                            treePositionVarianceX *= -1;
                                        }
                                        positionPosNegChance = rng.Next(0, 100);
                                        if (positionPosNegChance < 50)
                                        {
                                            treePositionVarianceY *= -1;
                                        }

                                        architectureModels.Add(Instantiate(vegetation[randomTree], new Vector3(-mapWidth * 5 + i + k * 10 + (float)treePositionVarianceX, heightCurve.Evaluate(noiseMap[i + k, j + l]) * heightMultiplier * 10 - 0.5f, mapHeight * 5 - j - l * 10 - (float)treePositionVarianceY), Quaternion.Euler(0, treeRotation, 0), transform.parent));
                                        listCount = architectureModels.Count;
                                        lastTree = architectureModels[listCount - 1];
                                        randomTreeScaleVariance = (float)rng.Next(8500, 12500) / 10000;
                                        lastTree.transform.localScale = new Vector3(randomTreeScaleVariance, randomTreeScaleVariance, randomTreeScaleVariance);
                                        noiseMapBlocked[i + k, j + l] = 1;
                                        Debug.Log("Random tree chance");
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }



    }

}
