using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapGenerator : MonoBehaviour {

    const int textureSize = 512;
    const TextureFormat textureFormat = TextureFormat.RGB565;

    public GameObject plane;
    public GameObject mesh;

    public enum DrawMode {HeightMap, ColorMap, Mesh, FalloffMap};
    [Tooltip("Determines the Draw Mode")]
    public DrawMode drawMode;
    [Tooltip("Determines the maps width, in whichever Draw Mode")]
    public int mapWidth;
    [Tooltip("Determines the maps height, in whichever Draw Mode")]
    public int mapHeight;
    [Tooltip("Determines the scale of the noise map in comparison to the map size")]
    public float noiseScale;
    [Tooltip("Determines the amount of overlapping noise maps to give a higher detail noisemap (Hint: keep around 5)")]
    public int octaves;
    [Tooltip("Determines the control of decrease in amplitude within each octave")]
    [Range(0,1)]
    public float persistence;
    [Tooltip("Determines the increase in frequency within each octave")]
    public float lacunarity;
    [Tooltip("Determines the starting point of the noisemap")]
    public int seed;
    [Tooltip("Allows for an increase or decrease of the starting point of the noisemap (on x and y axis")]
    public Vector2 offset;
    [Tooltip("Determines whether or not to use a falloff map")]
    public bool useFalloffMap;
    [Tooltip("Determines whether or not to use flatshading")]
    public bool useFlatShading;
    [Tooltip("Determines whether or not to use the colour shader")]
    public bool useColourShader;
    [Tooltip("Determines whether or not to use the texture shader")]
    public bool useTextureShader;
    [Tooltip("Determines the multiplied height of the mesh")]
    public float meshHeightMultiplier;
    [Tooltip("Allows for the flatness of water, whilst allowing other areas to gain height (Mesh only)")]
    public AnimationCurve meshHeightCurve;
    [Tooltip("Determines whether to auto update or not")]
    public bool autoUpdate;
    [Tooltip("Region colours for standard shader")]
    public TerrainType[] regions;
    [Tooltip("Determines the base colours for the colour shader")]
    public Color[] baseColours;
    [Tooltip("Determines the start heights for each colour in the base colours")]
    [Range(0,1)]
    public float[] baseStartHeights;
    [Tooltip("Determines the blend between colours in the base colours")]
    [Range(0,1)]
    public float[] baseBlends;
    [Tooltip("Determines the blend between colours and textures as well as the height in the Texture Shader")]
    public Layer[] layers;
    float[,] falloffMap;
    [HideInInspector]
    public string dMode;
    [Tooltip("Determines the housing area width")]
    public int housingWidth;
    [Tooltip("Determines the housing area height")]
    public int housingHeight;
    [Tooltip("Determines the housing area start (x axis)")]
    public int housingWidthStart;
    [Tooltip("Determines the housing area start (y axis)")]
    public int housingHeightStart;
    float[,] noiseMapCopy;
    float avgHeight;
    public Material terrainMaterial;
    Shader shader;
    [HideInInspector]
    public float minHeight
    {
        get
        {
            return 10 * meshHeightMultiplier * meshHeightCurve.Evaluate(0);
        }
    }
    [HideInInspector]
    public float maxHeight
    {
        get
        {
            return 10 * meshHeightMultiplier * meshHeightCurve.Evaluate(1);
        }
    }

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

    void Awake()
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

        if(useFalloffMap)
        {
            falloffMap = FalloffGenerator.GenerateFalloffMap(mapWidth, mapHeight);
            for (int i = 0; i < mapWidth; i++)
            {
                for (int j = 0; j < mapHeight; j++)
                {
                    noiseMap[i, j] = Mathf.Clamp01(noiseMap[i, j] - falloffMap[i, j]);
                }
            }
        }


        //deepWater
        if (useFalloffMap == false)
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
        if (useFalloffMap == false)
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
                        noiseMap[i, j] = Random.Range(0.32f, 0.35f);
                }
            }
        }

        //sand
        if (useFalloffMap == false)
            for (int i = 1; i < mapWidth - 1; i++)
        {
            for (int j = 1; j < mapHeight - 1; j++)
            {
                if (noiseMap[i, j] < 0.32f && noiseMap[i, j] > 0.28f)
                {
                    int connectedHeight = 0;
                    for (int a = i - 1; a <= i + 1; a++)
                    {
                        for (int b = j - 1; b <= j + 1; b++)
                        {
                            if (noiseMap[a, b] < 0.32f)
                            {
                                connectedHeight++;
                            }
                        }
                    }
                    if (connectedHeight < 6)
                        noiseMap[i, j] = Random.Range(0.32f, 0.35f);
                }
            }
        }

        System.Random rng = new System.Random();
        housingWidthStart = rng.Next(1, mapWidth - housingWidth);
        housingHeightStart = rng.Next(1, mapHeight - housingHeight);
        bool canExit = false;
        avgHeight = 0;

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
                noiseMap[i, j] = avgHeight;
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
        if (useColourShader || useTextureShader)
        {
            shader = Shader.Find("Custom/Terrain");
            terrainMaterial.shader = shader;
            UpdateMeshHeights(terrainMaterial, minHeight, maxHeight);
        }
        else
        {
            shader = Shader.Find("Standard");
            terrainMaterial.shader = shader;
        }

        MapDisplay display = FindObjectOfType<MapDisplay>();
        if (drawMode == DrawMode.HeightMap)
            display.DrawTexture(TextureGenerator.TextureHeightMap(noiseMap));
        else if (drawMode == DrawMode.ColorMap)
            display.DrawTexture(TextureGenerator.TextureColorMap(colorMap, mapWidth, mapHeight));
        else if (drawMode == DrawMode.Mesh)
            display.DrawMesh(MeshGenerator.GenerateMesh(noiseMap, meshHeightMultiplier, meshHeightCurve), TextureGenerator.TextureColorMap(colorMap, mapWidth, mapHeight));
        else if (drawMode == DrawMode.FalloffMap)
            display.DrawTexture(TextureGenerator.TextureHeightMap(FalloffGenerator.GenerateFalloffMap(mapWidth, mapHeight)));
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

        if (mapHeight * mapWidth > 255 * 255)
        {
            mapHeight = 255;
            mapWidth = 255;
        }

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

        if (useFlatShading)
        {
            if(mapWidth * mapHeight > 96 * 96)
            {
                mapWidth = 96;
                mapHeight = 96;
            }
            
        }

        if(useColourShader)
        {
            useColourShader = true;
            useTextureShader = false;
        }

        if (useTextureShader)
        {
            useTextureShader = true;
            useColourShader = false;
        }
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
        else if (userAdvancedOptions.dMode == "FalloffMap")
            drawMode = DrawMode.FalloffMap;

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
        else if (drawMode == DrawMode.FalloffMap)
            dMode = "FalloffMap";
    }

    public void UpdateDraw()
    {
        if (drawMode == DrawMode.Mesh || drawMode == DrawMode.FalloffMap)
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

    public void UpdateMeshHeights(Material material, float minHeight, float maxHeight)
    {
        material.SetFloat("minHeight", minHeight);
        material.SetFloat("maxHeight", maxHeight);

        if (useColourShader)
        {
            material.SetInt("baseColourCount", baseColours.Length);
            material.SetColorArray("baseColours", baseColours);
            material.SetFloatArray("baseStartHeights", baseStartHeights);
            material.SetFloatArray("baseBlends", baseBlends);
        }
        else if (useTextureShader)
        {
            material.SetInt("baseColourCount", layers.Length);
            material.SetColorArray("baseColours", layers.Select(x => x.tint).ToArray());
            material.SetFloatArray("baseStartHeights", layers.Select(x => x.startHeight).ToArray());
            material.SetFloatArray("baseBlends", layers.Select(x => x.blendStrength).ToArray());
            material.SetFloatArray("baseColourStrength", layers.Select(x => x.tintStrength).ToArray());
            material.SetFloatArray("baseTextureScales", layers.Select(x => x.textureScale).ToArray());
            Texture2DArray texturesArray = GenerateTextureArray(layers.Select(x => x.texture).ToArray());
            material.SetTexture("baseTextures", texturesArray);

        }
    }

    Texture2DArray GenerateTextureArray(Texture2D[] textures)
    {
        Texture2DArray textureArray = new Texture2DArray(textureSize, textureSize, textures.Length, textureFormat, true);

        for(int i = 0; i< textures.Length; i++)
        {
            textureArray.SetPixels(textures[i].GetPixels(), i);
        }
        textureArray.Apply();
        return textureArray;
    }

    public float[,] GetNoiseMap()
    {
        return noiseMapCopy;
    }

    public float GetAvgHeight()
    {
        return avgHeight;
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

[System.Serializable]
public class Layer
{
    public Texture2D texture;
    public Color tint;
    [Range(0,1)]
    public float tintStrength;
    [Range(0, 1)]
    public float startHeight;
    [Range(0, 1)]
    public float blendStrength;
    public float textureScale;
}
