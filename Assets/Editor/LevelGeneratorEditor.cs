using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelGenerator))]
public class LevelGeneratorEditor : Editor
{
    public LevelGenerator levelGenerator;


    void OnEnable()
    {
        levelGenerator = (LevelGenerator)target;
    }

    public override void OnInspectorGUI()
    {

        DrawDefaultInspector();

        if (GUILayout.Button("Set Type"))
        {
            levelGenerator.ChangeLevelType();
        }

        if(GUILayout.Button("Set Sprite"))
        {
            levelGenerator.SetSprites();
            SceneView.RepaintAll();
            EditorUtility.SetDirty(target);
        }

        if (GUILayout.Button("Create HiddenObject"))
        {
            Selection.activeGameObject=levelGenerator.CreateHiddenObject();
        }

        if (GUILayout.Button("Create HiddenObject (With Sprites)"))
        {
            levelGenerator.CreateHiddenObjectsWithSprites();
        }


        if (GUILayout.Button("SAVE"))
        {
            levelGenerator.SaveLevel();
        }

    }
}