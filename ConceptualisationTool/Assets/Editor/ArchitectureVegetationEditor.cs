using System.Collections;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ArchitectureVegetationGenerator))]
public class ArchitectureVegetationEditor : Editor {

    public override void OnInspectorGUI()
    {
        ArchitectureVegetationGenerator arcVegGen = (ArchitectureVegetationGenerator)target;
        /*GameObject userOpt = GameObject.Find("Options");
        UserOptions userOptions = userOpt.GetComponent<UserOptions>();
        UserAdvancedOptions userAdvancedOptions = userOpt.GetComponent<UserAdvancedOptions>();*/

        if (DrawDefaultInspector())
        {
            
        }

        if (GUILayout.Button("Delete Models"))
        {

            arcVegGen.DeleteObjectsFromList();

        }

        if(GUILayout.Button("Generate Models"))
        {
            arcVegGen.GenerateArchitecture();
        }
    }

}
