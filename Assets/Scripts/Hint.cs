using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TapticPlugin;
using UnityEngine;
using UnityEngine.UI;

public class Hint : MonoBehaviour
{
    public int objectID;
    public bool wasFound;
    public Image image;

    private Hint linkedHint;

    public void Link(Hint linkedHint)
    {
        this.linkedHint = linkedHint;
    }

 
    public void Check() {
        if (!wasFound) {
            bool isActive = HintIndicator.Find(this);
            if (isActive)
            {
                wasFound = true;
                if (linkedHint != null)
                {
                    linkedHint.ForceActivate();
                }
            }
        }
    }


    public void Activate()
    {
        image.DOFade(1f, 0.5f);
        transform.DOScale(-0.35f, 1f).SetDelay(0.2f).SetRelative();
        TapticManager.Impact(ImpactFeedback.Medium);
        SoundAndMusic.instance.PlaySoundEffectOneShot(SoundEffectType.FindObject);
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            Hint childHint = transform.parent.GetChild(i).GetComponent<Hint>();
            if (!childHint.wasFound)
            {
                transform.SetSiblingIndex(i);
                break;
            }
        }
        
    }

    public void ForceActivate()
    {
        wasFound = true;
        Activate();
    }
    
}
