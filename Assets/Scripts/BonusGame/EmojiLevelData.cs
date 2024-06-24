using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "LevelData", menuName = "Game/EmojiLevelData", order = 1)]
public class EmojiLevelData : ScriptableObject
{
    public EmojiPack[] emojiPacks;
}
[System.Serializable]
public class EmojiPack
{
    public Sprite firstSprite;
    public Sprite secondSprite;
}
