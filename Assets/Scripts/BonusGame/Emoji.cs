using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Emoji : MonoBehaviour
{
    public Emoji connectedEmoji;
    public bool isSelected;

    public int id;
    public int direction; //0 left, 1 right...

    public Image backgroundImage;
    public LineRenderer lineRenderer;

    public void Initialize(int id,int direction,Sprite emojiSprite)
    {
        this.id = id;
        this.direction = direction;
        transform.GetChild(0).GetComponent<Image>().sprite = emojiSprite;
    }


    public void Select(bool isActive)
    {
        backgroundImage.enabled = isActive;
        if (!isActive)
        {
            ResetLineRenderer();
        }
        else
        {
            transform.DOKill();
            transform.localScale = Vector3.one;
            transform.DOScale(Vector3.one * 1.15f, 0.15f).SetLoops(2, LoopType.Yoyo);
        }
        isSelected = isActive;
    }

    public bool IsSolved()
    {
        return (isSelected && connectedEmoji && connectedEmoji.id == id);
    }
    
    public void Connect(Emoji emoji)
    {
        emoji.Release();

        connectedEmoji = emoji;
        connectedEmoji.connectedEmoji = this;
        connectedEmoji.Select(true);
        SetLineRendererPosition((Vector2)connectedEmoji.transform.position);
    }

    public void Release()
    {
        Select(false);
        ResetLineRenderer();
        if (connectedEmoji)
        {
            Emoji cachedEmoji = connectedEmoji;
            connectedEmoji = null;
            cachedEmoji.Release();
        }
    }

    public void Click() {
        EmojiLevelInputManager.instance.SelectEmoji(this);
    }

    public void Touch()
    {
        EmojiLevelInputManager.instance.CheckEmoji(this);
    }


    public void ResetLineRenderer()
    {
        lineRenderer.enabled = false;
        lineRenderer.SetPosition(0, (Vector2)transform.position);
        lineRenderer.SetPosition(1, (Vector2)transform.position);
    }

    public void SetLineRendererPosition(Vector2 targetPosition)
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, (Vector2)transform.position);
        lineRenderer.SetPosition(1, targetPosition);
    }
}
