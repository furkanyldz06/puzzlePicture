using UnityEngine;
using System.Collections;
using DG.Tweening;

public class OtherSounds : MonoBehaviour {

	public AudioClip[] sounds;
    private AudioSource _audioSource;
    private AudioSource audioSource
    {
        get { return _audioSource != null ? _audioSource : _audioSource = GetComponent<AudioSource>(); }
    }

    public  bool isActive;
    Coroutine tutorialCoroutine;

    public void CheckSoundSettings()
    {
        isActive = PlayerPrefs.GetInt("Sound") == 1 ? true:false;
    }

    public void PlaySound(int soundId){
		if (isActive) {
            audioSource.Stop();
            audioSource.clip=sounds[soundId];
            audioSource.Play();
		}
	}


    bool characterTalking = false;
    int tutorialSoundIndex=1;

    public void PlayTutorialSounds(int soundID)
    {

        audioSource.DOKill();
        audioSource.DOFade(1f, 0f);
        characterTalking = true;
        PlaySound(tutorialSoundIndex);
        tutorialSoundIndex = (tutorialSoundIndex + 1) % sounds.Length;

        if (tutorialCoroutine != null)
        {
            StopCoroutine(tutorialCoroutine);
        }
        tutorialCoroutine = StartCoroutine(PlayTutorialSound());
        
    }

    public void StopTutorialSounds()
    {
        characterTalking = false;
        audioSource.DOKill();
        audioSource.DOFade(0f, 0.2f);
        if (tutorialCoroutine != null)
        {
            StopCoroutine(tutorialCoroutine);
        }
    }


    IEnumerator PlayTutorialSound()
    {
        while (characterTalking)
        {
            if (!audioSource.isPlaying)
            {
                yield return new WaitForSeconds(1f);
                if (!audioSource.isPlaying && characterTalking)
                {
                    PlaySound(tutorialSoundIndex);
                    tutorialSoundIndex = (tutorialSoundIndex + 1) % sounds.Length;
                }
            }
            yield return null;
        }
    }

    public void Stop()
    {
        if (audioSource.isPlaying)
        {
            audioSource.DOKill();
            audioSource.DOFade(0f, 0.5f).OnComplete(() => { audioSource.clip=null; audioSource.volume = 1f; });
        }
    }


    #region Other Sounds
    public void PlayRandomCoinSound(float volume=1f)
    {
        if (isActive)
        {
            audioSource.PlayOneShot(sounds[Random.Range(0, 5)], volume);
        }
    }

    public void PlayRandomDeathSound(float volume = 1f)
    {
        if (isActive)
        {
            audioSource.PlayOneShot(sounds[Random.Range(5, 12)], volume);
        }
    }
    #endregion


    #region Power Up Sounds
    public void PlayRandomArrowSound(float volume = 1f)
    {
        Debug.Log("Play Arrow Sound");
        if (isActive)
        {
            Debug.Log("Play Arrow Sound One Shot");
            audioSource.PlayOneShot(sounds[Random.Range(0, 3)], Random.Range(0.5f,1f));
        }
    }

    public void PlayPowerUpSoundOneShot(int id,float volume = 1f)
    {
        if (isActive)
        {
            audioSource.PlayOneShot(sounds[id], volume);
        }
    }

    #endregion

    #region Tutorial Sounds



    #endregion

}
