using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UserAdvancedOptions))]
public class UserAdvancedOptionsEditor : Editor
{

    public override void OnInspectorGUI()
    {
        UserAdvancedOptions userAdvancedOption = (UserAdvancedOptions)target;
        GameObject mapGen = GameObject.Find("MapGenerator");
        MapGenerator mapGenerator = mapGen.GetComponent<MapGenerator>();

        if (DrawDefaultInspector())
        {
            if (userAdvancedOption.autoUpdate)
            {
                userAdvancedOption.SetDrawMode();
                mapGenerator.UpdateVariables();

                userAdvancedOption.UpdateOptions();
            }
        }

        if (GUILayout.Button("Generate"))
        {
            userAdvancedOption.SetDrawMode();
            mapGenerator.UpdateVariables();
            userAdvancedOption.UpdateOptions();
        }
    }
}
