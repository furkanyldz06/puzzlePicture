using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour {

    public SoundEffect[] soundEffects;
    private Dictionary<int, SoundEffect> soundDictionary = new Dictionary<int, SoundEffect>();

    private AudioSource _audioSource;
    private AudioSource audioSource
    {
        get { return _audioSource != null ? _audioSource : _audioSource = GetComponent<AudioSource>(); }
    }
    public bool isActive;

    public void Initialize(Dictionary<int,SoundManager> soundManagerDictionary)
    {
        for (int i = 0; i < soundEffects.Length; i++)
        {
            soundDictionary.Add((int)soundEffects[i].soundEffectType, soundEffects[i]);
            soundManagerDictionary.Add((int)soundEffects[i].soundEffectType, this);
        }
    }

    public void CheckSoundSettings()
    {
        isActive = PlayerPrefs.GetInt("Sound",1) == 1 ? true : false;
    }

    public bool IsExist(int soundID)
    {
        return soundDictionary.ContainsKey(soundID);
    }

    public void PlaySound (int soundID)
	{
		if (isActive) {
            audioSource.Stop ();
            audioSource.clip = soundDictionary[soundID].audioClip;
            audioSource.Play ();
		}
	}

    public void TryToPlaySound(int soundID)
    {
        if(isActive && !audioSource.isPlaying)
        {
            audioSource.clip = soundDictionary[soundID].audioClip;
            audioSource.Play();
        }
    }

    public void PlaySoundEffectOneShot(int soundID, float volume=1f)
    {
        if (isActive)
        {
            audioSource.PlayOneShot(soundDictionary[soundID].audioClip, volume);
        }
    }


}
