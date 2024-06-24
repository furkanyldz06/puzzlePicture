using System.Collections;
using System.Collections.Generic;
using TapticPlugin;
using UnityEngine;

public class EmojiLevelInputManager : Singleton<EmojiLevelInputManager>
{
    public bool isActive;
    public Camera mainCamera;
    public EmojiLevelBuilder emojiLevelBuilder;
    public Emoji currentEmoji;

    // Start is called before the first frame update
    public void SetState(bool isActive)
    {
        this.isActive = isActive;
    }

    public void SelectEmoji(Emoji emoji)
    {
        if (isActive)
        {
            currentEmoji = emoji;
            currentEmoji.Release();
            currentEmoji.Select(true);
            TapticManager.Impact(ImpactFeedback.Medium);
            SoundAndMusic.instance.PlaySoundEffectOneShot(SoundEffectType.ButtonClick, 0.5f);
        }
    }

    public void CheckEmoji(Emoji emoji)
    {
        if (isActive && currentEmoji)
        {
            if (emoji.direction != currentEmoji.direction)
            {
                currentEmoji.Connect(emoji);
                currentEmoji = null;
                emojiLevelBuilder.CheckAllEmojies();
                TapticManager.Impact(ImpactFeedback.Medium);
                SoundAndMusic.instance.PlaySoundEffectOneShot(SoundEffectType.ButtonClick, 0.5f);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            if (currentEmoji)
            {
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = 10;
                Vector3 screenPos = mainCamera.GetComponent<Camera>().ScreenToWorldPoint(mousePos);
                currentEmoji.SetLineRendererPosition(screenPos);


                if (Input.GetMouseButtonUp(0))
                {
                    currentEmoji.Select(false);
                    currentEmoji = null;
                }
            }
        }
        
    }
}
