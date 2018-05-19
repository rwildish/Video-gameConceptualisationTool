using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class MapGenerator : MonoBehaviour {

    const int textureSize = 512;
    const TextureFormat textureFormat = TextureFormat.RGB565;

    public GameObject plane;
    public GameObject mesh;

    public enum DrawMode {HeightMap, ColorMap, Mesh, FalloffMap};
    [Tooltip("Determines the Draw Mode")]
    public DrawMode drawMode;
    public enum TerrainGenerationType {PerlinNoise, CellularAutomata, ReverseCellularAutomata};
    [Tooltip("Determines which terrain generation type to use. Use PerlinNoise for a standard cuboid shaped terrain or CellullarAutomata for a more organicly shaped terrain")]
    public TerrainGenerationType terrainGenerationType;
    public enum EnvironmentType {Custom, Forest, RainForest, Fields, Urban, City};
    [Tooltip("Determines what environment time will be generated, some other options may change how this works")]
    public EnvironmentType environmentType;
    public enum ShaderMode {Standard, Colour, Texture};
    [Tooltip("Determines which shader to use on the terrain")]
    public ShaderMode shaderMode;
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
    [Tooltip("Determines whether or not to use a random seed (If autoUpdate is on at the same time this will generate a random seed each time you change an attribute on the inspector)")]
    public bool useRandomSeed;
    [Tooltip("Allows for an increase or decrease of the starting point of the noisemap (on x and y axis")]
    public Vector2 offset;
    [Tooltip("Determines whether or not to use a falloff map")]
    public bool useFalloffMap;
    [Tooltip("Determines whether or not to use flatshading")]
    public bool useFlatShading;
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
    int[,] noiseMapBlocked;
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
    [Range(0,100)]
    public int randomFillPercent;
    int[,] map;

    public GameObject cellularAutomata;

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

        autoUpdate = false;
        useRandomSeed = false;

        GenerateMap();

    }

    public void GenerateMap()
    {
        if (terrainGenerationType == TerrainGenerationType.PerlinNoise)
        {
            if (useRandomSeed)
            {
                seed = UnityEngine.Random.Range(-9999, 9999);
            }
            float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistence, lacunarity, offset);

            if (useFalloffMap)
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
            if (useFalloffMap == false) { 
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
                                noiseMap[i, j] = UnityEngine.Random.Range(0.19f, 0.28f);
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
                                noiseMap[i, j] = UnityEngine.Random.Range(0.32f, 0.35f);
                        }
                    }
                }

            //sand
            
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
                                noiseMap[i, j] = UnityEngine.Random.Range(0.32f, 0.34f);
                        }
                    }
                }
        }

            noiseMapBlocked = new int[mapWidth, mapHeight];

            for(int i = 0; i < mapWidth; i++)
            {
                for(int j = 0; j < mapHeight; j++)
                {
                    if (noiseMap[i, j] <= 0.31f || noiseMap[i, j] >= 0.5f)
                    {
                        noiseMapBlocked[i, j] = 1;
                    }
                    else if (i == 0 || i == mapWidth || j == mapWidth || j == 0)
                    {
                        noiseMapBlocked[i, j] = 1;
                    }
                    else
                    {
                        noiseMapBlocked[i, j] = 0;
                    }
                }
            }

            System.Random rng = new System.Random();
            housingWidthStart = rng.Next(1, mapWidth - housingWidth);
            housingHeightStart = rng.Next(1, mapHeight - housingHeight);
            bool canExit = false;
            avgHeight = 0;
            int blockedCounter = 0;

            for (int z = 0; z < 10000; z++)
            {

                if (canExit == true)
                {
                    break;
                }

                canExit = true;
                avgHeight = 0;
                blockedCounter = 0;

                for (int i = housingWidthStart; i < housingWidthStart + housingWidth; i++)
                {
                    for (int j = housingHeightStart; j < housingHeightStart + housingHeight; j++)
                    {
                        if (noiseMap[i, j] < 0.32f || noiseMap[i, j] > 0.5f)
                        {
                            blockedCounter++;

                            if (blockedCounter > (housingWidth * housingHeight) / 20)
                            {
                                canExit = false;
                                avgHeight = 0;
                                housingWidthStart = rng.Next(1, mapWidth - housingWidth);
                                housingHeightStart = rng.Next(1, mapHeight - housingHeight);
                                
                            }
                        }
                        else
                        {
                            avgHeight += noiseMap[i, j];

                        }
                    }
                }


            }

            avgHeight = avgHeight / (housingWidth * housingHeight - blockedCounter);


            //if (avgHeight > 0.49f)
            //  avgHeight = 0.49f;
            //else if (avgHeight < 0.33f)
            //  avgHeight = 0.33f;

            for (int i = housingWidthStart - 1; i < housingWidthStart + housingWidth + 1; i++)
            {
                for (int j = housingHeightStart - 1; j < housingHeightStart + housingHeight + 1; j++)
                {
                    if(noiseMapBlocked[i,j] == 1)
                    {
                        int connectedBlocks = 0;
                        for(int x = i-1; x < i + 1; x++)
                        {
                            for (int y = j - 1; y < j + 1; y++)
                            {
                                if(x == i && y == j)
                                {
                                    continue;
                                }
                                else if(noiseMapBlocked[x,y] == 1)
                                {
                                    connectedBlocks++;
                                }
                            }
                        }

                        if(connectedBlocks == 0)
                        {
                            noiseMap[i, j] = avgHeight;
                        }

                        if(noiseMap[i,j] > avgHeight + 0.07f)
                        {
                            noiseMap[i, j] = UnityEngine.Random.Range(avgHeight + 0.03f, avgHeight + 0.07f);
                        }
                    }
                    else
                    {
                        noiseMap[i, j] = avgHeight;
                        //Debug.Log("Set avgHeight");
                    }
                    
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
                        if (currentHeight <= regions[i].height)
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

            for (int i = 0; i < mapWidth; i++)
            {
                for (int j = 0; j < mapHeight; j++)
                {
                    if (noiseMap[i, j] <= 0.31f || noiseMap[i, j] >= 0.4f)
                    {
                        noiseMapBlocked[i, j] = 1;
                    }
                    else if (i == 0 || i == mapWidth || j == mapWidth || j == 0)
                    {
                        noiseMapBlocked[i, j] = 1;
                    }
                    else
                    {
                        noiseMapBlocked[i, j] = 0;
                    }
                }
            }

            int[,] blockedNodes = new int[mapWidth, mapHeight];

            for (int i = 0; i < mapWidth; i++)
            {
                for (int j = 0; j < mapHeight; j++)
                {
                    if(noiseMapBlocked[i,j] == 1)
                    {
                        blockedNodes[i, j] = 1;
                    }
                    else
                    {
                        blockedNodes[i, j] = 0;
                    }
                }
            }

            for (int i = 1; i < mapWidth - 1; i++)
            {
                for (int j = 1; j < mapHeight - 1; j++)
                {
                    if(blockedNodes[i,j] == 1)
                    {
                        for (int k = -1; k < 2; k++)
                        {
                            for (int l = -1; l < 2; l++)
                            {
                               
                               noiseMapBlocked[i + k, j + l] = 1;
                                
                            }
                        }
                    }
                }
            }

            if (shaderMode == ShaderMode.Texture)
            {
                shader = Shader.Find("Custom/Terrain");
                terrainMaterial.shader = shader;
                UpdateMeshHeights(terrainMaterial, minHeight, maxHeight);
            }
            else if (shaderMode == ShaderMode.Colour)
            {
                shader = Shader.Find("Custom/ColourShader");
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
            //else if (drawMode == DrawMode.FalloffMap)
                //display.DrawTexture(TextureGenerator.TextureHeightMap(FalloffGenerator.GenerateFalloffMap(mapWidth, mapHeight)));
            UpdateDraw();
        }
        else
        {
            System.Random rng = new System.Random(seed.GetHashCode());

            if (useRandomSeed)
            {
                seed = UnityEngine.Random.Range(-9999, 9999);
            }

            //randomFillPercent = 53;

            map = new int[mapWidth, mapHeight];

            for(int i = 0; i < mapWidth; i++)
            {
                for(int j = 0; j < mapHeight; j++)
                {
                    if (i == 0 || i == mapWidth - 1 || j == 0 || j == mapHeight - 1)
                    {
                        map[i, j] = 1;
                    }
                    else {
                        if (terrainGenerationType == TerrainGenerationType.CellularAutomata)
                        {
                            map[i, j] = (rng.Next(0, 100) < randomFillPercent) ? 1 : 0;
                        }
                        else
                        {
                            map[i, j] = (rng.Next(0, 100) < randomFillPercent) ? 0 : 1;
                        }
                    }
                }
            }

            for (int x = 0; x < 10; x++)
            {
                for (int i = 0; i < mapWidth; i++)
                {
                    for (int j = 0; j < mapHeight; j++)
                    {
                        int neightbourWallTiles = GetSurroundingWallCount(i, j);

                        if (neightbourWallTiles > 4)
                        {
                            map[i, j] = 1;
                        }
                        else if (neightbourWallTiles < 4)
                        {
                            map[i, j] = 0;
                        }

                    }
                }
            }

            ProcessMap();

            int borderSize = 1;
            int[,] borderedMap = new int[mapWidth + borderSize * 2, mapHeight + borderSize * 2];

            for(int i = 0; i < borderedMap.GetLength(0); i++)
            {
                for(int j = 0; j < borderedMap.GetLength(1); j++)
                {
                    if(i >= borderSize && i < mapWidth + borderSize && j >= borderSize && j < mapHeight + borderSize)
                    {
                        borderedMap[i, j] = map[i - borderSize, j - borderSize];
                    }
                    
                    else
                    {
                        borderedMap[i, j] = 1;
                    }
                    
                }
            }

            if (terrainGenerationType == TerrainGenerationType.ReverseCellularAutomata)
            {
                for (int i = 0; i < borderedMap.GetLength(0); i++)
                {
                    for (int j = 0; j < borderedMap.GetLength(1); j++)
                    {
                        if (borderedMap[i, j] == 1)
                        {
                            borderedMap[i, j] = 0;
                        }
                        else
                        {
                            borderedMap[i, j] = 1;
                        }
                    }
                }
            }

            CellGenerator cellGen = cellularAutomata.GetComponent<CellGenerator>();
            cellGen.GenerateMesh(borderedMap, 1);

        }

    }

    struct Coord
    {
        public int tileX;
        public int tileY;

        public Coord(int x, int y)
        {
            tileX = x;
            tileY = y;
        }
    }

    int GetSurroundingWallCount(int gridX, int gridY)
    {
        int wallCount = 0;
        for(int neighbourX = gridX -1; neighbourX <= gridX +1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if (IsInMapRange(neighbourX,neighbourY))
                {
                    if (neighbourX != gridX || neighbourY != gridY)
                    {
                        wallCount += map[neighbourX, neighbourY];
                    }
                }
                else
                {
                    wallCount++;
                }
            }
        }

        return wallCount;
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

        if(terrainGenerationType == TerrainGenerationType.CellularAutomata || terrainGenerationType == TerrainGenerationType.ReverseCellularAutomata)
        {
            mesh.SetActive(false);
            //GameObject cellularAutomata = GameObject.Find("CellularAutomata");
            cellularAutomata.SetActive(true);

        }
        else
        {
            //GameObject cellularAutomata = GameObject.Find("CellularAutomata");
            //if(cellularAutomata != null)
                cellularAutomata.SetActive(false);
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

        if(autoUpdate == false && useRandomSeed == false)
        aVGenerator.GenerateArchitecture();
    }

    public void GenerateArchitectureVegetation()
    {
        GameObject aVG = GameObject.Find("MapGenerator");
        ArchitectureVegetationGenerator aVGenerator = aVG.GetComponent<ArchitectureVegetationGenerator>();

        aVGenerator.GenerateArchitecture();
    }

    public void UpdateMeshHeights(Material material, float minHeight, float maxHeight)
    {
        material.SetFloat("minHeight", minHeight);
        material.SetFloat("maxHeight", maxHeight);

        if (shaderMode == ShaderMode.Colour)
        {
            material.SetInt("baseColourCount", baseColours.Length);
            material.SetColorArray("baseColours", baseColours);
            material.SetFloatArray("baseStartHeights", baseStartHeights);
            material.SetFloatArray("baseBlends", baseBlends);
        }
        else if (shaderMode == ShaderMode.Texture)
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

    void ProcessMap()
    {
        List<List<Coord>> wallSections = GetCaveSections(1);

        int wallThresholdSize = 50;
        foreach(List<Coord> wallSection in wallSections)
        {
            if(wallSection.Count < wallThresholdSize)
            {
                foreach(Coord tile in wallSection)
                {
                    map[tile.tileX, tile.tileY] = 0;
                }
            }
        }

        List<List<Coord>> roomSections = GetCaveSections(0);

        int roomThresholdSize = 50;
        List<Room> survivingRooms = new List<Room>();
        foreach (List<Coord> roomSection in roomSections)
        {
            if (roomSection.Count < roomThresholdSize)
            {
                foreach (Coord tile in roomSection)
                {
                    map[tile.tileX, tile.tileY] = 1;
                }
            }
            else
            {
                survivingRooms.Add(new Room(roomSection, map));
            }
        }
        if (terrainGenerationType == TerrainGenerationType.CellularAutomata)
        {
            survivingRooms.Sort();
            survivingRooms[0].isMainRoom = true;
            survivingRooms[0].isAccessibleFromMainRoom = true;
            ConnectClosestRooms(survivingRooms);
        }
    }

    void ConnectClosestRooms(List<Room> allRooms, bool forceAccessibilityFromMainRoom = false)
    {
        List<Room> roomListA = new List<Room>();
        List<Room> roomListB = new List<Room>();

        if (forceAccessibilityFromMainRoom)
        {
            foreach(Room room in allRooms)
            {
                if (room.isAccessibleFromMainRoom)
                {
                    roomListB.Add(room);
                }
                else
                {
                    roomListA.Add(room);
                }
            }
        }
        else
        {
            roomListA = allRooms;
            roomListB = allRooms;
        }

        int bestDistance = 0;
        Coord bestTileA = new Coord();
        Coord bestTileB = new Coord();
        Room bestRoomA = new Room();
        Room bestRoomB = new Room();
        bool possibleConnectionFound = false;

        foreach(Room a in roomListA)
        {
            if (!forceAccessibilityFromMainRoom)
            {
                possibleConnectionFound = false;
                if(a.connectedRooms.Count > 0)
                {
                    continue;
                }
            }

            foreach(Room b in roomListB)
            {
                if (a == b || a.IsConnected(b))
                {
                    continue;
                }
                for(int tileIndexA = 0; tileIndexA < a.edgeTiles.Count; tileIndexA++)
                {
                    for (int tileIndexB = 0; tileIndexB < b.edgeTiles.Count; tileIndexB++)
                    {
                        Coord tileA = a.edgeTiles[tileIndexA];
                        Coord tileB = b.edgeTiles[tileIndexB];
                        int distanceBetweenRooms = (int) (Mathf.Pow(tileA.tileX - tileB.tileX, 2) + Mathf.Pow(tileA.tileY - tileB.tileY, 2));

                        if(distanceBetweenRooms < bestDistance || !possibleConnectionFound)
                        {
                            bestDistance = distanceBetweenRooms;
                            possibleConnectionFound = true;
                            bestTileA = tileA;
                            bestTileB = tileB;
                            bestRoomA = a;
                            bestRoomB = b;
                        }
                    }
                }
            }
            if (possibleConnectionFound && !forceAccessibilityFromMainRoom)
            {
                CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
            }
        }

        if (possibleConnectionFound && forceAccessibilityFromMainRoom)
        {
            CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
            ConnectClosestRooms(allRooms, true);
        }

        if (!forceAccessibilityFromMainRoom)
        {
            ConnectClosestRooms(allRooms, true);
        }
    }

    void CreatePassage(Room roomA, Room roomB, Coord tileA, Coord tileB)
    {
        Room.ConnectRooms(roomA, roomB);
        //Debug.DrawLine(CoordToWorldPoint(tileA), CoordToWorldPoint(tileB), Color.green, 100);

        List<Coord> line = GetLine(tileA, tileB);

        foreach(Coord c in line)
        {
            DrawCircle(c, 2);
        }

    }

    void DrawCircle(Coord c, int r)
    {
        for(int x = -r; x<= r; x++)
        {
            for (int y = -r; y <= r; y++)
            {
                if(x*x + y*y <= r * r)
                {
                    int drawX = c.tileX + x;
                    int drawY = c.tileY + y;
                    if(IsInMapRange(drawX, drawY))
                    {
                        map[drawX, drawY] = 0;
                    }
                }
            }
        }
    }

    List<Coord> GetLine(Coord from, Coord to)
    {
        List<Coord> line = new List<Coord>();

        int x = from.tileX;
        int y = from.tileY;

        int dx = to.tileX - from.tileX;
        int dy = to.tileY - from.tileY;

        bool inverted = false;
        int step = (int) (Mathf.Sign(dx));
        int gradientStep = (int) (Mathf.Sign(dy));

        int longest = Mathf.Abs(dx);
        int shortest = Mathf.Abs(dy);

        if(longest < shortest)
        {
            inverted = true;
            longest = Mathf.Abs(dy);
            shortest = Mathf.Abs(dx);

            step = (int)(Mathf.Sign(dy));
            gradientStep = (int)(Mathf.Sign(dx));
        }

        int gradientAccumulation = longest / 2;
        for(int i = 0; i < longest; i++)
        {
            line.Add(new Coord(x, y));

            if (inverted)
            {
                y += step;
            }
            else
            {
                x += step;
            }

            gradientAccumulation += shortest;
            if(gradientAccumulation >= longest)
            {
                if (inverted)
                {
                    x += gradientStep;
                }
                else
                {
                    y += gradientStep;
                }
                gradientAccumulation -= longest;
            }
        }
        return line;
    }

    Vector3 CoordToWorldPoint(Coord tile)
    {
        return new Vector3(-mapWidth / 2 + 0.5f + tile.tileX, 2, -mapHeight / 2 + 0.5f + tile.tileY);
    }

    List<List<Coord>> GetCaveSections(int tileType)
    {
        List<List<Coord>> sections = new List<List<Coord>>();
        int[,] mapFlags = new int[mapWidth, mapHeight];

        for(int i = 0; i < mapWidth; i++)
        {
            for(int j = 0; j< mapHeight; j++)
            {
                if(mapFlags[i,j] == 0 && map[i,j] == tileType)
                {
                    List<Coord> newSection = GetSectionTiles(i, j);
                    sections.Add(newSection);
                    foreach(Coord tile in newSection)
                    {
                        mapFlags[tile.tileX, tile.tileY] = 1;
                    }
                }
            }
        }
        return sections;
    }

    List<Coord> GetSectionTiles(int startX, int startY)
    {
        List<Coord> tiles = new List<Coord>();
        int[,] mapFlags = new int[mapWidth, mapHeight];
        int tileType = map[startX, startY];

        Queue<Coord> queue = new Queue<Coord>();
        queue.Enqueue(new Coord(startX, startY));
        mapFlags[startX, startY] = 1;

        while(queue.Count > 0)
        {
            Coord tile = queue.Dequeue();
            tiles.Add(tile);

            for(int i = tile.tileX - 1; i <= tile.tileX + 1; i++)
            {
                for (int j = tile.tileY - 1; j <= tile.tileY + 1; j++)
                {
                    if (IsInMapRange(i, j) && (j == tile.tileY || i == tile.tileX))
                    {
                        if(mapFlags[i,j] == 0 && map[i,j] == tileType)
                        {
                            mapFlags[i, j] = 1;
                            queue.Enqueue(new Coord(i, j));
                        }
                    }
                }
            }
        }
        return tiles;
    }

    class Room : IComparable<Room>
    {
        public List<Coord> tiles;
        public List<Coord> edgeTiles;
        public List<Room> connectedRooms;
        public int roomSize;
        public bool isAccessibleFromMainRoom;
        public bool isMainRoom;

        public Room()
        {
            //for empty rooms
        }

        public Room(List<Coord> roomTiles, int[,] map)
        {
            tiles = roomTiles;
            roomSize = tiles.Count;
            connectedRooms = new List<Room>();
            edgeTiles = new List<Coord>();

            foreach (Coord tile in tiles)
            {
                for (int i = tile.tileX - 1; i <= tile.tileX + 1; i++)
                {
                    for (int j = tile.tileY - 1; j <= tile.tileY + 1; j++)
                    {
                        if (i == tile.tileX || j == tile.tileY)
                        {
                            if (map[i, j] == 1)
                            {
                                edgeTiles.Add(tile);
                            }
                        }
                    }
                }
            }
        }

        public void SetAccessibleFromMainRoom()
        {
            if (!isAccessibleFromMainRoom)
            {
                isAccessibleFromMainRoom = true;
                foreach(Room connectedRoom in connectedRooms)
                {
                    connectedRoom.SetAccessibleFromMainRoom();
                }
            }
        }

        public static void ConnectRooms(Room a, Room b)
        {
            if (a.isAccessibleFromMainRoom)
            {
                b.SetAccessibleFromMainRoom();
            }
            else if (b.isAccessibleFromMainRoom)
            {
                a.SetAccessibleFromMainRoom();
            }
            a.connectedRooms.Add(b);
            b.connectedRooms.Add(a);
        }

        public bool IsConnected(Room otherRoom)
        {
            return connectedRooms.Contains(otherRoom);
        }

        public int CompareTo(Room otherRoom)
        {
            return otherRoom.roomSize.CompareTo(roomSize);
        }
        
    }

    bool IsInMapRange(int x, int y)
    {
       return x >= 0 && x < mapWidth && y >= 0 && y < mapHeight;
    }

    public float[,] GetNoiseMap()
    {
        return noiseMapCopy;
    }
    public int[,] GetNoiseMapBlocked()
    {
        return noiseMapBlocked;
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
