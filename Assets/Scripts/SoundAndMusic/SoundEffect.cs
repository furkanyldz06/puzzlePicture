using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sound Effect", menuName = "Sound & Music/SoundEffect", order = 1)]
public class SoundEffect : ScriptableObject
{
    public SoundEffectType soundEffectType;
    public AudioClip audioClip;
}
