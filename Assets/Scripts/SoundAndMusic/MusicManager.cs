using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MusicManager : MonoBehaviour {

    public SoundEffect[] soundEffects;
    private Dictionary<int, SoundEffect> soundDictionary = new Dictionary<int, SoundEffect>();

    private AudioSource _audioSource;
    private AudioSource audioSource
    {
        get { return _audioSource != null ? _audioSource : _audioSource = GetComponent<AudioSource>(); }
    }
    public bool isActive;

    public void Initialize()
    {
        for (int i = 0; i < soundEffects.Length; i++)
        {
            soundDictionary.Add((int)soundEffects[i].soundEffectType, soundEffects[i]);
        }
    }

    public void CheckMusicState ()
	{
        isActive = PlayerPrefs.GetInt("Music",1) == 1 ? true : false;

		if (isActive) {
			if(!audioSource.isPlaying && soundEffects.Length>0)
            {
                PlaySound((int)soundEffects[0].soundEffectType);
            }
		} else {
			if(audioSource.isPlaying){
                audioSource.Stop();
            }
		}
	}
	
	public void PlaySound (int soundID)
	{
        if (isActive) {
            audioSource.Stop();
            audioSource.clip= soundDictionary[soundID].audioClip;
            audioSource.Play();
		}
	}

    public void SetMusicsState(bool gamePlay)
    {
        audioSource.DOKill();
        if (gamePlay)
        {
            SetMusicVolume(0.5f);
        }
        else
        {
            SetMusicVolume(0f);
        }
    }

    public void SetMusicVolume(float volume)
    {
        audioSource.DOKill();
        audioSource.DOFade(volume, 2f);
    }

}
