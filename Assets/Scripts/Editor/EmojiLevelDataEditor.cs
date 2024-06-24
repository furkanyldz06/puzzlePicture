using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EmojiLevelData))]
public class EmojiLevelDataEditor : Editor
{
    EmojiLevelData targetScript;
    SerializedObject so;
    SerializedProperty emojiPacks;

    int count;
    void OnEnable()
    {
        targetScript = (EmojiLevelData)target;
        count = targetScript.emojiPacks.Length;
        so = new SerializedObject(targetScript);
        emojiPacks = so.FindProperty("emojiPacks");
    }

    public override void OnInspectorGUI()
    {
        count = EditorGUILayout.IntField(count);
        if(count != targetScript.emojiPacks.Length)
        {
            List<EmojiPack> cachedEmojiPacks = new List<EmojiPack>();
            for (int i = 0; i < targetScript.emojiPacks.Length; i++)
            {
                cachedEmojiPacks.Add(targetScript.emojiPacks[i]);
            }

            targetScript.emojiPacks = new EmojiPack[count];
            for (int i = 0; i < cachedEmojiPacks.Count; i++)
            {
                if(i< count)
                {
                    targetScript.emojiPacks[i] = cachedEmojiPacks[i];
                }
            }
        }
        emojiPacks = so.FindProperty("emojiPacks");


        Handles.BeginGUI();

        try
        {
            for (int i = 0; i < emojiPacks.arraySize; i++)
            {
                EditorGUILayout.BeginHorizontal();

                Sprite sprite = targetScript.emojiPacks[i].firstSprite;
                targetScript.emojiPacks[i].firstSprite = (Sprite)EditorGUILayout.ObjectField("", sprite, typeof(Sprite), GUILayout.Width(80), GUILayout.Height(80));

                Sprite secondSprite = targetScript.emojiPacks[i].secondSprite;
                targetScript.emojiPacks[i].secondSprite = (Sprite)EditorGUILayout.ObjectField("", secondSprite, typeof(Sprite), GUILayout.Width(80), GUILayout.Height(80));
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
            }

        }
        catch (System.Exception ex)
        {

        }
        
        if (GUI.changed)
        {
            Repaint();
            EditorUtility.SetDirty(targetScript);
            so.Update();
            //so.ApplyModifiedProperties();
            
        }

        
        Handles.EndGUI();
        //SceneView.RepaintAll();
    }
}
