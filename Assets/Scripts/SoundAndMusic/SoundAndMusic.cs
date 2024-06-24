using UnityEngine;
using System.Collections;
using System.IO;
using TapticPlugin;
using System.Collections.Generic;

public class SoundAndMusic : Singleton<SoundAndMusic> {


    private MusicManager musicManager;
    private SoundManager[] soundManagers;

    private Dictionary<int, SoundManager> soundManagerDictionary = new Dictionary<int, SoundManager>();

    public override void Awake () {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        base.Awake();
        if (instance == this)
        {
            musicManager = GetComponentInChildren<MusicManager>();
            musicManager.Initialize();

            soundManagers = GetComponentsInChildren<SoundManager>();
            for (int i = 0; i < soundManagers.Length; i++)
            {
                soundManagers[i].Initialize(soundManagerDictionary);
            }
        }
    }
    private void Start()
    {
        MaxSdk.SetSdkKey("3K_4TtvTTHUGO5AET5unTp0kS9hBxB5hQTkg4Mn_bBCMvTXGVkalNff83Acr_a4DoN1hl5pDUJHgq5fOQZRAQy");
        MaxSdk.InitializeSdk();

        MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) => {
            // AppLovin SDK is initialized, start loading ads
        };
    }

    public void CheckSettings()
    {
        musicManager.CheckMusicState();
        for (int i = 0; i < soundManagers.Length; i++)
        {
            soundManagers[i].CheckSoundSettings();
        }
    }

    public void FadeIn()
    {
        musicManager.SetMusicsState(true);
    }
    public void FadeOut()
    {
        musicManager.SetMusicsState(false);
    }

    #region Music
    public void PlayMusic(int id)
    {
        musicManager.PlaySound(id);
    }

    public void SetMusicState(bool gameplay)
    {
        CheckSettings();
        musicManager.SetMusicsState(gameplay);
    }

    public void SetMusicVolume(float volume)
    {
        musicManager.SetMusicVolume(volume);
    }

    #endregion

    #region Sounds
    public void PlaySoundEffect(SoundEffectType soundEffectType)
    {
        if (soundEffectType == SoundEffectType.ButtonClick || soundEffectType == SoundEffectType.Buy)
        {
            TapticManager.Impact(ImpactFeedback.Light);
        }
        GetSoundManager((int)soundEffectType).PlaySound((int)soundEffectType);
    }

    public void TryToPlaySoundEffect(SoundEffectType soundEffectType)
    {
        GetSoundManager((int)soundEffectType).TryToPlaySound((int)soundEffectType);
    }

    public void PlaySoundEffectOneShot(SoundEffectType soundEffectType, float volume=1f)
    {
        if (soundEffectType == SoundEffectType.ButtonClick || soundEffectType == SoundEffectType.Buy)
        {
            TapticManager.Impact(ImpactFeedback.Light);
        }
        GetSoundManager((int)soundEffectType).PlaySoundEffectOneShot((int)soundEffectType,volume);
    }

    private SoundManager GetSoundManager(int id)
    {
        return soundManagerDictionary[id];
    }
    #endregion

    //[ContextMenu("PlayDebug")]
    //public void PlayDebug() {
    //    PlaySoundEffectOneShot(SoundEffectType.SplitObject);
    //    PlaySoundEffectOneShot(SoundEffectType.Hammer);

    //}

}

public enum SoundEffectType
{
    BackgroundMusic = 0,

    ButtonClick = 10,
    Wrong = 11,
    Buy = 12,

    Win = 50,
    FindObject = 51,
    GameOver = 52,

    BubblePop = 100,

    BoosterPop1 = 101,
    BoosterPop2 = 102,
    BoosterPop3 = 103,

    Beehive = 104,
    DonutBit = 105,
    SplitObject = 106,

    Hammer = 200,
    FireworkWhistle = 201,
    FireworkHit = 202,
    Pinwheel = 203,

}
