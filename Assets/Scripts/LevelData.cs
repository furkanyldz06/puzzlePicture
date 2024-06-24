using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Game/LevelData", order = 0)]
public class LevelData : ScriptableObject
{
    public int levelIndex;
    public LevelType levelType;
    public Sprite firstSprite;
    public Sprite secondSprite;
    public List<ObjectData> objectDatas;

    public void Initialize()
    {
        objectDatas = new List<ObjectData>();
    }

    public void SetSprites(Sprite firstSprite, Sprite secondSprite = null)
    {
        this.firstSprite = firstSprite;
        this.secondSprite = secondSprite;
    }

    public void AddObjectData(int objectID,Sprite objectSprite, Vector3 localPosition, Vector3 localScale)
    {
        objectDatas.Add(new ObjectData(objectID,objectSprite, localPosition, localScale));
    }
    public void AddObjectData(ObjectData objectData)
    {
        objectDatas.Add(objectData);
    }
}

[System.Serializable]
public class ObjectData
{
    public int objectID;
    public Sprite objectSprite;
    public Vector3 localPosition;
    public Vector3 localScale;
    public ObjectData(int objectID,Sprite objectSprite, Vector3 localPosition,Vector3 localScale)
    {
        this.objectID = objectID;
        this.objectSprite = objectSprite;
        this.localPosition = localPosition;
        this.localScale = localScale;
    }   
}


