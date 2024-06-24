using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Lean.Localization;
using TapticPlugin;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private Image mask;
    [Header("Settings")]
    #region Settings
    [SerializeField] private SettingsButton[] settingsButtons;
    [SerializeField] private Transform settingsPanel,forceUpdatePanel;
    private bool musicIsActive, soundIsActive, hapticIsActive;
    #endregion

    #region Sprites
    [SerializeField] private Sprite settingsButtonActiveSprite, settingsButtonPassiveSprite;
    #endregion

    [System.Serializable]
    public struct SettingsButton
    {
        public Image buttonBackground;
        public Text buttonText;
        public string description;
    }

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        SoundAndMusic.instance.FadeIn();
    }

    public void SelectGameMode(string gameMode)
    {
        GameManager.selectedGameMode = gameMode;
        switch (gameMode)
        {
            case "Mixed":
                GameManager.mixedMode = true;
                break;
            default:
                GameManager.mixedMode = false;
                break;
        }

        SceneManager.LoadScene("Gameplay");
    }

    private void Initialize()
    {
        musicIsActive = (PlayerPrefs.GetInt("Music", 1) == 1);
        soundIsActive = (PlayerPrefs.GetInt("Sound", 1) == 1);
        hapticIsActive = (PlayerPrefs.GetInt("Haptic", 1) == 1);
        //notificationsIsActive = (PlayerPrefs.GetInt(PlayerprefKeys.notifications, 1) == 1);

        CheckMusic();
        CheckSound();
        CheckHaptic();
        SoundAndMusic.instance.CheckSettings();
    }

    public void ChangeHapticSetting()
    {
        hapticIsActive = !hapticIsActive;
        int hapticState = (hapticIsActive) ? 1 : 0;
        PlayerPrefs.SetInt("Haptic", hapticState);
        CheckHaptic();
        SoundAndMusic.instance.CheckSettings();
        SoundAndMusic.instance.PlaySoundEffectOneShot(SoundEffectType.ButtonClick);
    }

    public void ChangeMusicSetting()
    {
        musicIsActive = !musicIsActive;
        int musicState = (musicIsActive) ? 1 : 0;
        PlayerPrefs.SetInt("Music", musicState);
        CheckMusic();
        TapticManager.Impact(ImpactFeedback.Light);
        SoundAndMusic.instance.CheckSettings();
        SoundAndMusic.instance.PlaySoundEffectOneShot(SoundEffectType.ButtonClick);
    }

    public void ChangeSoundSetting()
    {
        soundIsActive = !soundIsActive;
        int soundState = (soundIsActive) ? 1 : 0;
        PlayerPrefs.SetInt("Sound", soundState);
        CheckSound();
        TapticManager.Impact(ImpactFeedback.Light);
        SoundAndMusic.instance.CheckSettings();
        SoundAndMusic.instance.PlaySoundEffectOneShot(SoundEffectType.ButtonClick);
    }

    #region Settings
    private void SetToggleButtonState(int index, bool isActive) //Music & Sound & Haptic...
    {
        
        settingsButtons[index].buttonBackground.sprite = (isActive) ? settingsButtonActiveSprite : settingsButtonPassiveSprite;
        settingsButtons[index].buttonText.text = LeanLocalization.GetTranslationText(settingsButtons[index].description) + ((isActive) ? LeanLocalization.GetTranslationText("On") : LeanLocalization.GetTranslationText("Off"));
    }

    private void CheckSound() //Set UI Object
    {
        SetToggleButtonState(0, soundIsActive);
    }
    private void CheckMusic() //Set UI Object
    {
        SetToggleButtonState(1, musicIsActive);
    }

    private void CheckHaptic() //Set UI Object
    {
        // SetToggleButtonState(2, hapticIsActive);
        TapticManager.isActive = hapticIsActive;
    }

    #endregion

    
    public void Restore()
    {
        TapticManager.Impact(ImpactFeedback.Light);
    }

    public void PrivacyPolicy()
    {
        TapticManager.Impact(ImpactFeedback.Light);
        Application.OpenURL("https://www.gitberry.com/privacy-policy");
    }



    public void SetSettingsPanelState(bool isActive)
    {
        SetMaskState(isActive);
        settingsPanel.DOKill();
        if (isActive)
        {
            settingsPanel.gameObject.SetActive(true);
            settingsPanel.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        }
        else
        {
            settingsPanel.DOScale(Vector3.zero, 0.2f).SetEase(Ease.Linear).OnComplete(() => settingsPanel.gameObject.SetActive(false));
        }
        SoundAndMusic.instance.PlaySoundEffectOneShot(SoundEffectType.ButtonClick);
    }

    public void SetForceUpdatePanelState(bool isActive)
    {
        SetMaskState(isActive);
        forceUpdatePanel.DOKill();
        if (isActive)
        {
            forceUpdatePanel.gameObject.SetActive(true);
            forceUpdatePanel.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        }
        else
        {
            forceUpdatePanel.DOScale(Vector3.zero, 0.2f).SetEase(Ease.Linear).OnComplete(() => forceUpdatePanel.gameObject.SetActive(false));
        }
        SoundAndMusic.instance.PlaySoundEffectOneShot(SoundEffectType.ButtonClick);
    }


    public void SetMaskState(bool isActive)
    {
        SoundAndMusic.instance.PlaySoundEffectOneShot(SoundEffectType.ButtonClick);
        TapticManager.Impact(ImpactFeedback.Light);
        mask.DOKill();
        if (isActive)
        {
            mask.raycastTarget = true;
            mask.DOFade(0.8f, 0.5f);
        }
        else
        {
            mask.raycastTarget = false;
            mask.DOFade(0f, 0.2f);
        }
    }

    public void ShowLeaderboard(){
        LeaderboardController.instance.ShowLeaderboard();
    }

    public void UpdateGame()
    {
#if UNITY_ANDROID
        string urlString = "market://details?id=" + "com.gitberry.hiddenmaster";
        Application.OpenURL(urlString);
#elif UNITY_IOS
        Application.OpenURL("itms-apps://itunes.apple.com/app/id1532648388");
#endif
    }



}
