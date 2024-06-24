using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LevelGenerator : MonoBehaviour
{
    public int levelIndex;
    public LevelType levelType;
    public Transform[] objectImages;
    public GameObject hintPrefab;

    public LevelData currentLevelData;
    public Sprite defaultHintSprite;

    [Space(20)]
    #region Sprites
    public Sprite firstSprite;
    public Sprite secondSprite;

    public Sprite[] hintSprites;
    #endregion
    private Transform targetImage,targetSecondImage;


    public void SetSprites()
    {
        targetImage.GetComponent<Image>().sprite = firstSprite;
        targetSecondImage.GetComponent<Image>().sprite = secondSprite;
    }

    public GameObject CreateHiddenObject()
    {
       return Instantiate(hintPrefab, targetImage);
    }

    public void CreateHiddenObjectsWithSprites()
    {
        for (int i = 0; i < hintSprites.Length; i++)
        {
            Instantiate(hintPrefab, targetImage).GetComponent<Image>().sprite= hintSprites[i];
        }
    }

    public void ChangeLevelType()
    {
        for (int i = 0; i < objectImages.Length; i++)
        {
            objectImages[i].gameObject.SetActive(i == (int)levelType);
            if (i == (int)levelType)
            {
                targetImage = objectImages[i].Find("Mask/TargetImage");
                if(levelType== LevelType.MultiImage)
                {
                    targetSecondImage= objectImages[i].Find("Mask2/TargetImage");
                }
            }
        }
    }

    public void SaveLevel()
    {
        currentLevelData=ScriptableObject.CreateInstance<LevelData>();
        currentLevelData.Initialize();
        currentLevelData.levelIndex = levelIndex;
        currentLevelData.levelType = levelType;

        currentLevelData.firstSprite = firstSprite;
        currentLevelData.secondSprite = secondSprite;

        GameObject[] hintObjects = GameObject.FindGameObjectsWithTag("Hint");
        for (int i = 0; i < hintObjects.Length; i++)
        {
            Transform targetHintObject = hintObjects[i].transform;
            Sprite objectSprite = targetHintObject.GetComponent<Image>().sprite;
            int objectID = (objectSprite == defaultHintSprite) ? - 1: targetHintObject.GetSiblingIndex();

            ObjectData objectData = new ObjectData(objectID, objectSprite, targetHintObject.localPosition, targetHintObject.localScale);
            currentLevelData.AddObjectData(objectData);
        }

#if UNITY_EDITOR
        CreateNewLevelData();
#endif
    }

    public void CreateNewLevelData()
    {
#if UNITY_EDITOR
        AssetDatabase.CreateAsset(currentLevelData, "Assets/Resources/" + currentLevelData.levelIndex + ".asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
#endif
    }

}
