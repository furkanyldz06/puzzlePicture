using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HintIndicator : MonoBehaviour
{
    public Transform findEffects;
    public int effectIndex;

    public bool isActive;
    public Transform hints;
    public List<Hint> foundHints=new List<Hint>();

    public static Func<Hint, bool> Find;
    public int targetObjectCount;
    Action winCallback;

    public Sprite tickSprite;

    public float rewardedHintTimer;
    public float inputTimer;

    public RectTransform hintButton;

    public bool callOffer;


    public void OnGameModeChanged(GameMode gameMode)
    {
        switch (gameMode)
        {
            case GameMode.Failed:
            case GameMode.Paused:
            case GameMode.Completed:
                isActive = false;
                break;
            case GameMode.Active:
                isActive = true;
                break;
        }
    }

    private void Update()
    {
        if (isActive)
        {
            if (GameManager.instance.gameMode == GameMode.Active)
            {
                inputTimer += Time.deltaTime;
                if (inputTimer > 15f)
                {
                    inputTimer = 0f;
                    ShakeHintButton();
                }
            }

            if(GameManager.instance.gameMode == GameMode.Active && GameManager.instance.isRewardedAdButtonActive == false)
            {
                rewardedHintTimer += Time.deltaTime;
                if (rewardedHintTimer >= 30f)
                {
                    if (GoogleMobileAdsDemoScript.instance.IsRewardedVideoReady())
                    {
                        GameManager.instance.SetRewardedAdButtonState(true);
                        rewardedHintTimer = -60f;
                    }
                    else
                    {
                        rewardedHintTimer = 20f;
                    }
                }
            }
        }
    }

    public int shakeCount;
    private void ShakeHintButton()
    {
        shakeCount++;

        hintButton.DOKill();
        hintButton.transform.localPosition = Vector3.up * 9f;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(hintButton.DOAnchorPosX(15f, 0.06f).SetEase(Ease.Linear));
        sequence.Append(hintButton.DOAnchorPosX(-15f, 0.12f).SetEase(Ease.Linear));
        sequence.Append(hintButton.DOAnchorPosX(0f, 0.06f).SetEase(Ease.Linear));
        sequence.SetLoops(3, LoopType.Restart);

        if (GameManager.instance.currentLevel<5 && GameManager.selectedGameMode== "Classic") 
        {
            GameManager.instance.levelBuilder.Highlight();
        }


        if (GameManager.instance.hintCount == 0 && callOffer ==false && UnityEngine.Random.value>0.5f && shakeCount >2 && GoogleMobileAdsDemoScript.instance.IsRewardedVideoReady())
        {
            callOffer = true;
            GameManager.instance.CallOffer();
        }
    }

    public Transform GetFindEffect()
    {
        Transform currentEffect = findEffects.GetChild(effectIndex);
        effectIndex = (effectIndex + 1) % findEffects.childCount;
        return currentEffect;
    }

    private void OnEnable()
    {
        Find += OnObjectFind;
        GameManager.GameModeChanged += OnGameModeChanged;
    }

    private void OnDisable()
    {
        Find -= OnObjectFind;
        GameManager.GameModeChanged -= OnGameModeChanged;
    }

    public void Initialize(Action winCallback,int objectCount, List<ObjectData> objectDatas)
    {
        this.winCallback = winCallback;
        targetObjectCount = objectCount;
        for (int i = 0; i < hints.childCount; i++)
        {
            hints.GetChild(i).gameObject.SetActive(i < objectCount);
            if (i < objectCount)
            {
                if (objectDatas != null)
                {
                    hints.GetChild(i).Find("Frame/HintImage").GetComponent<Image>().sprite = objectDatas[i].objectSprite;
                }
            }
        }
        isActive = true;
    }

    public bool OnObjectFind(Hint hint)
    {
        if (isActive)
        {
            bool finded = FindObject(hint);
            if (foundHints.Count == targetObjectCount)
            {
                if (winCallback != null)
                {
                    GameManager.instance.ChangeGameMode(GameMode.Paused);
                    DOVirtual.DelayedCall(1.25f, () => {
                        winCallback();

                    });
                }
                else
                {
                    Debug.LogError("NULL winCallback");
                }
            }
            return finded;
        }
        return false;
    }

    public bool FindObject(Hint hint)
    {
        if (foundHints.Contains(hint) == false)
        {
            foundHints.Add(hint);
            hint.Activate(); // FADE...
            ActivateFindedObject(hint.objectID); // Obje ID'si -1 değilse singletype
            Transform effect = GetFindEffect();
            effect.transform.position = hint.transform.position;
            effect.GetComponent<ParticleSystem>().Play();
            inputTimer = 0f;
            if (rewardedHintTimer > 0f)
            {
                rewardedHintTimer = 0f;
            }
            if (GameManager.instance.isRewardedAdButtonActive)
            {
                GameManager.instance.SetRewardedAdButtonState(false);
            }

            return true;
        }
        return false;
    }

    public void ForceActivate() //Type2...
    {

    }

    public void ActivateFindedObject(int objectID)
    {
        for (int i = 0; i < hints.childCount; i++)
        {
            if (objectID!=-1)
            {
                if(i== objectID)
                {
                    hints.GetChild(i).Find("Frame/Tick").gameObject.SetActive(true);
                }
            }
            else
            {
                if(i< foundHints.Count)
                {
                    hints.GetChild(i).GetComponent<Image>().sprite = tickSprite;
                }
                
            }
        }
    }
}
