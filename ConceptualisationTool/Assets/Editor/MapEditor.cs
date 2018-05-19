using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (MapGenerator))]
public class MapEditor : Editor {

    private enum TerrainType { Custom, Mountainous, Flat, Hilly };

    public override void OnInspectorGUI()
    {
        MapGenerator mapGen = (MapGenerator)target;
        GameObject userOpt = GameObject.Find("Options");
        UserOptions userOptions = userOpt.GetComponent<UserOptions>();
        UserAdvancedOptions userAdvancedOptions = userOpt.GetComponent<UserAdvancedOptions>();

        if (DrawDefaultInspector())
        {
            if (mapGen.autoUpdate){
                
                
                mapGen.SetDrawMode();
                userOptions.UpdateVariables();
                userAdvancedOptions.UpdateVariables();
                mapGen.GenerateMap();
            }
        }

        if(GUILayout.Button("Generate"))
        {
            
            mapGen.SetDrawMode();
            userOptions.UpdateVariables();
            userAdvancedOptions.UpdateVariables();
            mapGen.GenerateMap();
            
        }

        if(GUILayout.Button("Generate Models"))
        {
            GameObject arcVeg = GameObject.Find("MapGenerator");
            ArchitectureVegetationGenerator arcVegGen = arcVeg.GetComponent<ArchitectureVegetationGenerator>();

            arcVegGen.GenerateArchitecture();
        }
    }

    

    
}
