using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Lean.Localization;
using UnityEngine;
using UnityEngine.UI;

public class LevelBuilder : MonoBehaviour
{
    [SerializeField] private DummyInputManager inputManager;
    public Transform[] objectImages;
    public HintIndicator[] hintIndicators;
    public List<Hint> allHints= new List<Hint>();
    public LevelType levelType;

    public Text levelDescriptionText;

    public ParticleSystem tutorialEffect;

    #region Images

    public Image firstImageType1;
    [Space(10)]
    public Image firstImageType2;
    public Image secondImageType2;

    #endregion

    [Space(10)]
    #region Prefabs
    public GameObject hintPrefab;
    #endregion

    [ContextMenu("Build Level")]
    public void BuildLevel(Action winCallback,LevelData levelData)
    {
        levelType = levelData.levelType;

        switch (levelData.levelType)
        {
            case LevelType.SingleImage:
                levelDescriptionText.text =  LeanLocalization.GetTranslationText("FindHiddenObjects");
                break;
            case LevelType.MultiImage:
                levelDescriptionText.text =  LeanLocalization.GetTranslationText("FindDifferences","FIND DIFFERENCES");
                break;
        }

        for (int i = 0; i < objectImages.Length; i++)
        {
            objectImages[i].gameObject.SetActive(i == (int)levelType);
        }

        SetBackgroundSprites(levelData);
        GenerateHintObjects(levelData);

        for (int i = 0; i < hintIndicators.Length; i++)
        {
            hintIndicators[i].gameObject.SetActive(i == (int)levelType);
            if (i == (int)levelType)
            {
                hintIndicators[i].Initialize(winCallback,levelData.objectDatas.Count, (levelType== LevelType.SingleImage) ? levelData.objectDatas :  null);
            }
        }
    }

    private void SetBackgroundSprites(LevelData levelData)
    {
        if (levelType == LevelType.SingleImage)
        {
            inputManager.SetCurrentImages(new List<RectTransform> { firstImageType1.GetComponent<RectTransform>() });
            firstImageType1.sprite = levelData.firstSprite;
        }
        else if (levelType == LevelType.MultiImage)
        {
            inputManager.SetCurrentImages(new List<RectTransform> { firstImageType2.GetComponent<RectTransform>(), secondImageType2.GetComponent<RectTransform>() });
            firstImageType2.sprite = levelData.firstSprite;
            secondImageType2.sprite = levelData.secondSprite;
        }
    }

    private void GenerateHintObjects(LevelData levelData)
    {
        switch (levelData.levelType)
        {
            case LevelType.SingleImage:
                for (int i = 0; i < levelData.objectDatas.Count; i++)
                {
                    InstantiateHintPrefabs(firstImageType1.transform, levelData.objectDatas[i]);
                }

                break;
            case LevelType.MultiImage:
                for (int i = 0; i < levelData.objectDatas.Count; i++)
                {
                    Hint hint1 = InstantiateHintPrefabs(firstImageType2.transform, levelData.objectDatas[i]);
                    Hint hint2 = InstantiateHintPrefabs(secondImageType2.transform, levelData.objectDatas[i]);
                    hint1.Link(hint2);
                    hint2.Link(hint1);
                }
                break;
        }
    }

    private Hint InstantiateHintPrefabs(Transform parentObject,ObjectData objectData)
    {
        GameObject createdHint = Instantiate(hintPrefab, parentObject);
        Hint hint = createdHint.GetComponent<Hint>();
        createdHint.transform.localPosition = objectData.localPosition;
        createdHint.transform.localScale = objectData.localScale*1.5f;
        hint.objectID = objectData.objectID;
        allHints.Add(hint);
        return hint;
    }

    [ContextMenu("Highlight")]
    public void Highlight()
    {
        Debug.Log("Highlight");
        for (int i = 0; i < allHints.Count; i++)
        {
            if (allHints[i].wasFound == false)
            {
                tutorialEffect.transform.position = allHints[i].transform.position;
                tutorialEffect.Play();
            }
        }
    }

    public bool GetHint()
    {
        Debug.Log("GetHint");
        for (int i = 0; i < allHints.Count; i++)
        {
            if (allHints[i].wasFound == false)
            {
                Debug.Log("Check");
                Hint targetHint = allHints[i];
                targetHint.Check();
                return true;
            }
        }
        return false;
    }

}

public enum LevelType
{
    SingleImage,
    MultiImage
}
