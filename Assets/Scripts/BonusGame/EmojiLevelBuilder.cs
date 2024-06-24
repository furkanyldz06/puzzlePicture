using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class EmojiLevelBuilder : MonoBehaviour
{
    [SerializeField] private EmojiLevelData levelData;
    [SerializeField] private Transform emojiGrid;

    List<EmojiSpriteWithID> leftEmojiSprites = new List<EmojiSpriteWithID>();
    List<EmojiSpriteWithID> rightEmojiSprites = new List<EmojiSpriteWithID>();
    List<Emoji> activeEmojies = new List<Emoji>();

    public GameObject hintLinePrefab;

    Action winCallback;

    public void CheckAllEmojies()
    {
        bool completed = true;
        for (int i = 0; i < activeEmojies.Count; i++)
        {
            if (activeEmojies[i].IsSolved()==false)
            {
                completed = false;
                break;
            }
        }

        if (completed)
        {
            CompleteLevel();
        }
        else
        {
            bool allEmojiesSelected = true;
            for (int i = 0; i < activeEmojies.Count; i++)
            {
                if (activeEmojies[i].isSelected==false)
                {
                    allEmojiesSelected = false;
                    break;
                }
            }

            if (allEmojiesSelected)
            {
                //Debug.LogError("FAILED");
                SoundAndMusic.instance.PlaySoundEffectOneShot(SoundEffectType.Wrong);
                Shake();
            }
        }
    }

    public bool ReadyToUseHint()
    {
        bool readyToUseHint = false;
        for (int i = 0; i < activeEmojies.Count; i++)
        {
            if (activeEmojies[i].IsSolved() == false && hintEmojies.Count<levelData.emojiPacks.Length)
            {
                readyToUseHint = true;
                break;
            }
        }
        return readyToUseHint;
    }

    public List<Emoji> hintEmojies=new List<Emoji>();
    public void GetHint()
    {
        for (int i = 0; i < activeEmojies.Count; i++)
        {
            if (activeEmojies[i].direction==0 && activeEmojies[i].IsSolved() == false && hintEmojies.Contains(activeEmojies[i])==false)
            {
                hintEmojies.Add(activeEmojies[i]);
                activeEmojies[i].Release();
                SetHintLine(activeEmojies[i]);
                break;
            }
        }
    }

    private void SetHintLine(Emoji currentEmoji)
    {
        LineRenderer createdLineRenderer = Instantiate(hintLinePrefab).GetComponent<LineRenderer>();
        createdLineRenderer.SetPosition(0, (Vector2)currentEmoji.transform.position);
        Emoji targetEmoji = null;
        for (int i = 0; i < activeEmojies.Count; i++)
        {
            if(activeEmojies[i].id == currentEmoji.id && activeEmojies[i] != currentEmoji)
            {
                targetEmoji = activeEmojies[i];
                break;
            }
        }
        createdLineRenderer.SetPosition(1, (Vector2)targetEmoji.transform.position);
    }


    public void Shake()
    {
        EmojiLevelInputManager.instance.SetState(false);
        for (int i = 0; i < activeEmojies.Count; i++)
        {
            Transform emojiTransform = activeEmojies[i].transform;
            emojiTransform.DOKill();
            Sequence shake = DOTween.Sequence();
            shake.Append(emojiTransform.DORotate(Vector3.forward * 10, 0.05f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo));
            shake.Append(emojiTransform.DORotate(Vector3.forward * -10, 0.05f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo));
            shake.SetLoops(2, LoopType.Restart);
            
        }

        DOVirtual.DelayedCall(0.5f,()=> {
            for (int i = 0; i < activeEmojies.Count; i++)
            {
                activeEmojies[i].Release();
            }
            EmojiLevelInputManager.instance.SetState(true);
        });
    }

    public void BuildLevel(Action winCallback, EmojiLevelData levelData)
    {

        this.levelData = levelData;

        if (levelData.emojiPacks.Length == 4)
        {
            emojiGrid.GetComponent<GridLayoutGroup>().spacing = new Vector2(emojiGrid.GetComponent<GridLayoutGroup>().spacing.x, emojiGrid.GetComponent<GridLayoutGroup>().spacing.y / 2f);
        }

        for (int i = 0; i < levelData.emojiPacks.Length; i++)
        {
            leftEmojiSprites.Add(new EmojiSpriteWithID(i, levelData.emojiPacks[i].firstSprite));
            rightEmojiSprites.Add(new EmojiSpriteWithID(i,levelData.emojiPacks[i].secondSprite));
        }

        Shuffle();
        SetEmojiSprites();
        this.winCallback = winCallback;


        int levelIndex=PlayerPrefs.GetInt("LastSubmittedLevel" + "Emoji");
        if (levelIndex == 0)
        {
            DOVirtual.DelayedCall(1.5f, ()=>GetHint());
        }
    }

    public void Shuffle()
    {
        leftEmojiSprites.Shuffle();
        rightEmojiSprites.Shuffle();
        CheckRatio();
    }


    int reshuffleCount = 0;
    void CheckRatio()
    {
        reshuffleCount++;
        if (reshuffleCount < 50)
        {
            float matches = 0;
            for (int i = 0; i < leftEmojiSprites.Count; i++)
            {
                if (leftEmojiSprites[i].id == rightEmojiSprites[i].id)
                {
                    matches++;
                }
            }

            float matchRatio = (float)matches / leftEmojiSprites.Count;
            if (matchRatio > 0.5f)
            {
                Shuffle();
            }
        }
    }


    private void SetEmojiSprites()
    {
        int loopCount = levelData.emojiPacks.Length;
        for (int i = 0; i < emojiGrid.childCount; i++)
        {
            emojiGrid.GetChild(i).gameObject.SetActive(i < loopCount * 2);
            if(i < loopCount * 2)
            {
                activeEmojies.Add(emojiGrid.GetChild(i).GetComponent<Emoji>());
            }
        }


        int index = 0;
        for (int i = 0; i < loopCount; i++)
        {
            Debug.Log("Set Emoji Sprites: "+i);
            emojiGrid.GetChild(index).GetComponent<Emoji>().Initialize(leftEmojiSprites[i].id, 0, leftEmojiSprites[i].sprite);
            emojiGrid.GetChild(index + 1).GetComponent<Emoji>().Initialize(rightEmojiSprites[i].id, 1, rightEmojiSprites[i].sprite);
            index+=2;
        }
    }

    private void CompleteLevel()
    {
        if (winCallback != null)
        {
            winCallback();
        }
    }

    public struct EmojiSpriteWithID
    {
        public int id;
        public Sprite sprite;
        public EmojiSpriteWithID(int id,Sprite sprite)
        {
            this.id = id;
            this.sprite = sprite;
        }
    }
}
