using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public LevelData levelData;

    public void Initialize(LevelData levelData)
    {
        this.levelData = levelData;
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

}



