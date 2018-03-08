using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UserOptions))]
public class UserOptionsEditor : Editor {

    public override void OnInspectorGUI()
    {
        UserOptions userOption = (UserOptions)target;

        if (DrawDefaultInspector())
        {
            if (userOption.autoUpdate)
            {
                userOption.UpdateOptions();
            }
        }

        if (GUILayout.Button("Generate"))
        {
            userOption.UpdateOptions();
        }
    }
}
