using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    public GameObject plane;
    public GameObject mesh;

    public enum DrawMode {HeightMap, ColorMap, Mesh};
    public DrawMode drawMode;

    public int mapWidth;
    public int mapHeight;
    public float noiseScale;

    public int octaves;
    [Range(0,1)]
    public float persistence;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve;

    public bool autoUpdate;

    public TerrainType[] regions;
    [HideInInspector]
    public string dMode;

    public int housingWidth;
    public int housingHeight;
    public int housingWidthStart;
    public int housingHeightStart;
    float[,] noiseMapCopy;

    TerrainType deepWater;
    TerrainType water;
    TerrainType sand;
    TerrainType grass;
    TerrainType grass2;
    TerrainType grass3;
    TerrainType rock;
    TerrainType rock2;
    TerrainType rock3;
    TerrainType snow;
    TerrainType none;

    void Start()
    {
        deepWater.name = "Deep Water";
        deepWater.height = 0.22f;
        deepWater.color = new Color (8, 89, 129, 0);

        water.name = "Water";
        water.height = 0.32f;
        water.color = new Color(5, 108, 159, 0);

        sand.name = "Sand";
        sand.height = 0.35f;
        sand.color = new Color(214, 216, 127, 0);

        grass.name = "Grass";
        grass.height = 0.5f;
        grass.color = new Color(61, 229, 69, 0);

        grass2.name = "Grass 2";
        grass2.height = 0.65f;
        grass2.color = new Color(46, 203, 54, 0);

        grass3.name = "Grass 3";
        grass3.height = 0.75f;
        grass3.color = new Color(0, 191, 10, 0);

        rock.name = "Rock";
        rock.height = 0.85f;
        rock.color = new Color(118, 102, 66, 0);

        rock2.name = "Rock 2";
        rock2.height = 0.9f;
        rock2.color = new Color(77, 72, 47, 0);

        rock3.name = "Rock 3";
        rock3.height = 1f;
        rock3.color = new Color(144, 136, 107, 0);

        snow.name = "Snow";
        snow.height = 0.97f;
        snow.color = new Color(255, 255, 255, 0);

        none.name = "none";
        none.height = 0;
        none.color = new Color (255,255,255,255);
    }

    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistence, lacunarity, offset);


        //deepWater
        for (int i = 1; i < mapWidth - 1; i++)
        {
            for (int j = 1; j < mapHeight - 1; j++)
            {
                if (noiseMap[i, j] < 0.19f)
                {
                    int connectedHeight = 0;
                    for (int a = i - 1; a <= i + 1; a++)
                    {
                        for (int b = j - 1; b <= j + 1; b++)
                        {
                            if (noiseMap[a, b] < 0.19f)
                            {
                                connectedHeight++;
                            }
                        }
                    }
                    if (connectedHeight < 6)
                        noiseMap[i, j] = Random.Range(0.19f, 0.28f);
                }
            }
        }

        //water
        for (int i = 1; i < mapWidth - 1; i++)
        {
            for (int j = 1; j < mapHeight - 1; j++)
            {
                if (noiseMap[i, j] < 0.29f && noiseMap[i, j] > 0.18f)
                {
                    int connectedHeight = 0;
                    for (int a = i - 1; a <= i + 1; a++)
                    {
                        for (int b = j - 1; b <= j + 1; b++)
                        {
                            if (noiseMap[a, b] < 0.29f)
                            {
                                connectedHeight++;
                            }
                        }
                    }
                    if (connectedHeight < 6)
                        noiseMap[i, j] = Random.Range(0.29f, 0.31f);
                }
            }
        }

        //sand
        for (int i = 1; i < mapWidth-1; i++)
        {
            for (int j = 1; j < mapHeight-1; j++)
            {
                if(noiseMap[i,j] < 0.32f && noiseMap[i,j] > 0.28f)
                {
                    int connectedHeight = 0;
                    for (int a = i -1; a <= i +1; a++)
                    {
                        for (int b = j-1; b <= j+1; b++)
                        {
                            if (noiseMap[a,b] < 0.32f)
                            {
                                connectedHeight++;
                            }
                        }
                    }
                    if (connectedHeight < 6)
                        noiseMap[i, j] = Random.Range(0.32f,0.35f);
                }
            }
        }

        System.Random rng = new System.Random();
        housingWidthStart = rng.Next(1, mapWidth - housingWidth);
        housingHeightStart = rng.Next(1, mapHeight - housingHeight);
        bool canExit = false;
        float avgHeight = 0;

        for (int z = 0; z < 10000; z++)
        {
            
            if (canExit == true)
            {
                break;
            }

            canExit = true;
            avgHeight = 0;

            for (int i = housingWidthStart; i < housingWidthStart + housingWidth; i++)
            {
                for (int j = housingHeightStart; j < housingHeightStart + housingHeight; j++)
                {
                    if (noiseMap[i, j] < 0.32f || noiseMap[i,j] > 0.5f)
                    {
                        canExit = false;
                        avgHeight = 0;
                        housingWidthStart = rng.Next(1, mapWidth - housingWidth);
                        housingHeightStart = rng.Next(1, mapHeight - housingHeight);
                    }
                    else
                    {
                        avgHeight += noiseMap[i, j];
                        
                    }
                }
            }
            
            
        }

        avgHeight = avgHeight / ((housingWidth) * (housingHeight));


        //if (avgHeight > 0.49f)
          //  avgHeight = 0.49f;
        //else if (avgHeight < 0.33f)
          //  avgHeight = 0.33f;

        for (int i = housingWidthStart; i < housingWidthStart + housingWidth; i++)
        {
            for (int j = housingHeightStart; j < housingHeightStart + housingHeight; j++)
            {
                noiseMap[i, j] = Random.Range(avgHeight - 0.002f, avgHeight + 0.002f);
            }
        }

        //make an array with a size of the height of the map times the width of the map so each element in the array represents a pixel
        Color[] colorMap = new Color[mapWidth * mapHeight];
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++)
                {
                    if(currentHeight <= regions[i].height)
                    {
                        colorMap[y * mapWidth + x] = regions[i].color;
                        break;
                    }
                }
            }
        }

        noiseMapCopy = new float[mapWidth, mapHeight];

        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                noiseMapCopy[i, j] = noiseMap[i, j];
            }
        }
        

        MapDisplay display = FindObjectOfType<MapDisplay>();
        if (drawMode == DrawMode.HeightMap)
            display.DrawTexture(TextureGenerator.TextureHeightMap(noiseMap));
        else if (drawMode == DrawMode.ColorMap)
            display.DrawTexture(TextureGenerator.TextureColorMap(colorMap, mapWidth, mapHeight));
        else if (drawMode == DrawMode.Mesh)
            display.DrawMesh(MeshGenerator.GenerateMesh(noiseMap, meshHeightMultiplier, meshHeightCurve), TextureGenerator.TextureColorMap(colorMap, mapWidth, mapHeight));
        UpdateDraw();
    }
    //OnValidate allows me to apply restrictions to variables that the users will change in the inspector of Unity
    //This is useful as users may not know why the Octaves cannot be negative, etc.
    void OnValidate()
    {
        if (mapWidth < 1)
            mapWidth = 1;

        if (mapHeight < 1)
            mapHeight = 1;

        if (lacunarity < 1)
            lacunarity = 1;

        if (octaves < 0)
            octaves = 0;

        if (housingWidthStart < 0)
            housingWidthStart = 0;

        if (housingHeightStart < 0)
            housingHeightStart = 0;

        if (housingHeight < 0)
            housingHeight = 0;

        if (housingWidth < 0)
            housingWidth = 0;

        if (octaves > 20)
            octaves = 20;

    }

    public void UpdateVariables()
    {
        GameObject userAO = GameObject.Find("Options");
        UserAdvancedOptions userAdvancedOptions = userAO.GetComponent<UserAdvancedOptions>();

        if (userAdvancedOptions.dMode == "ColorMap")
            drawMode = DrawMode.ColorMap;
        else if (userAdvancedOptions.dMode == "HeightMap")
            drawMode = DrawMode.HeightMap;
        else if (userAdvancedOptions.dMode == "Mesh")
            drawMode = DrawMode.Mesh;

        mapWidth = userAdvancedOptions.mapWidth;
        mapHeight = userAdvancedOptions.mapHeight;
        noiseScale = userAdvancedOptions.noiseScale;
        octaves = userAdvancedOptions.octaves;
        persistence = userAdvancedOptions.persistence;
        lacunarity = userAdvancedOptions.lacunarity;
        seed = userAdvancedOptions.seed;
        offset.x = userAdvancedOptions.offset.x;
        offset.y = userAdvancedOptions.offset.y;
        meshHeightMultiplier = userAdvancedOptions.meshHeightMultiplier;
        meshHeightCurve = userAdvancedOptions.meshHeightCurve;
        autoUpdate = userAdvancedOptions.autoUpdate;
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

    public void UpdateDraw()
    {
        if (drawMode == DrawMode.Mesh)
        {
            mesh.SetActive(true);
            plane.SetActive(false);
        }
        else
        {
            mesh.SetActive(false);
            plane.SetActive(true);
        }
        GameObject aVG = GameObject.Find("MapGenerator");
        ArchitectureVegetationGenerator aVGenerator = aVG.GetComponent<ArchitectureVegetationGenerator>();

        aVGenerator.GenerateArchitecture();
    }

    public float[,] GetNoiseMap()
    {
        return noiseMapCopy;
    }
}
//A struct to allow users to create their own terrain colours and essentially their design
//Example already made for them so they can stick to this if they prefer
//Could make several arrays to allow for presets
[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color color;
}
