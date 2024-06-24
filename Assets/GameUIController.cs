using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

using System;
using TapticPlugin;
using Lean.Localization;
//using TapticPlugin;

public class GameUIController : MonoBehaviour
{
    [SerializeField] private Image mask;
    [SerializeField] private Text levelIndexText, winPanelLevelIndexText, timerText,versionNumberText;
    [SerializeField] private Image failImage;

    [Header("Canvas Groups")]
    #region Canvas Groups
    [SerializeField] private CanvasGroup winPanelCanvasGroup;
    #endregion


    [Header("PopUps")]
    #region PopUps
    [SerializeField] private Transform storePanel;
    [SerializeField] private Transform offerPanel;
    [SerializeField] private Transform gameOverPanel;
    #endregion


    [Header("Win Pop Up")]

    #region Win PopUp
    [SerializeField] private Image winCharacter;
    [SerializeField] private Image winLight;
    #endregion

    

    [Header("Settings")]
    #region Settings
    [SerializeField] private SettingsButton[] settingsButtons;
    [SerializeField] private Transform settingsPanel;
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

    private void Start()
    {
        // versionNumberText.text = "v" + Application.version;
        Initialize();
    }

    public void SetLevelIndex(int levelIndex)
    {
        // levelIndexText.text = LeanLocalization.GetTranslationText("Level").Replace("*", levelIndex.ToString());
        // winPanelLevelIndexText.text= LeanLocalization.GetTranslationText("Level").Replace("*", levelIndex.ToString());
    }

    public void SetLevelIndex()
    {
        levelIndexText.text = "";// LeanLocalization.GetTranslationText("Mixed", "MIXED");
        winPanelLevelIndexText.text = "";// LeanLocalization.GetTranslationText("Mixed", "MIXED");
    }


    int cachedSecond;
    public void SetTimer(float timer)
    {
        int d1 = ((int)timer * 100);
        int minutes1 = d1 / (60 * 100);
        int seconds1 = (d1 % (60 * 100)) / 100;
        int hours1 = minutes1 / 60;

        if (cachedSecond != seconds1)
        {
            timerText.text = (minutes1 % 60).ToString().PadLeft(2, '0') + ":" + (seconds1).ToString().PadLeft(2, '0');
        }
        cachedSecond = seconds1;
    }

    public void MissClick()
    {
        if (DOTween.IsTweening(timerText) == false)
        {
            timerText.DOColor(Color.red, 0.75f).SetLoops(2, LoopType.Yoyo);
        }
        MissClickAnimation();
    }

    public void MissClickAnimation()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPosition.z = 0f;
        failImage.transform.position = worldPosition;

        failImage.DOKill();
        failImage.transform.DOKill();
        failImage.transform.localEulerAngles = Vector3.zero;
        failImage.color = new Color(1f, 1f, 1f, 0f);


        failImage.DOFade(1f, 0.3f);
        failImage.transform.DOShakeRotation(1f, Vector3.forward*30f);
        failImage.DOFade(0f, 0.3f).SetDelay(1f);
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
        // settingsButtons[index].buttonText.text = LeanLocalization.GetTranslationText(settingsButtons[index].description) + ((isActive) ? LeanLocalization.GetTranslationText("On") : LeanLocalization.GetTranslationText("Off"));
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

    #region Set PopUp States

    public void SetOfferPanelState(bool isActive)
    {
        SetMaskState(isActive);
        offerPanel.DOKill();
        if (isActive)
        {
            offerPanel.gameObject.SetActive(true);
            offerPanel.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        }
        else
        {
            offerPanel.DOScale(Vector3.zero, 0.2f).SetEase(Ease.Linear).OnComplete(() => offerPanel.gameObject.SetActive(false));
        }
        SoundAndMusic.instance.PlaySoundEffectOneShot(SoundEffectType.ButtonClick);
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

    public void SetStorePanelState(bool isActive)
    {
        SetMaskState(isActive);
        storePanel.DOKill();
        if (isActive)
        {
            storePanel.gameObject.SetActive(true);
            storePanel.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        }
        else
        {
            storePanel.DOScale(Vector3.zero, 0.2f).SetEase(Ease.Linear).OnComplete(() => storePanel.gameObject.SetActive(false));
        }
        SoundAndMusic.instance.PlaySoundEffectOneShot(SoundEffectType.ButtonClick);
    }



    public void SetGameOverPanelState(bool isActive)
    {
        SetMaskState(isActive);
        gameOverPanel.DOKill();
        if (isActive)
        {
            gameOverPanel.gameObject.SetActive(true);
            gameOverPanel.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        }
        else
        {
            gameOverPanel.DOScale(Vector3.zero, 0.2f).SetEase(Ease.Linear).OnComplete(() => gameOverPanel.gameObject.SetActive(false));
        }
    }


    public void SetWinPanelState(bool isActive)
    {
        
        winPanelCanvasGroup.DOKill();
        if (isActive)
        {
            winPanelCanvasGroup.gameObject.SetActive(true);
            winPanelCanvasGroup.DOFade(1f, 1f);
            winPanelCanvasGroup.interactable = true;
            winPanelCanvasGroup.blocksRaycasts = true;

            winCharacter.color = new Color(1f, 1f, 1f, 0f);
            winCharacter.DOFade(1f, 1f);
            winCharacter.transform.DOScale(Vector3.one, 1.5f).SetEase(Ease.OutBack).SetDelay(0.1f);

            winLight.DOFade(0.2f, 0.5f).SetLoops(-1, LoopType.Yoyo);
            winLight.transform.DORotate(Vector3.forward * 30, 0.5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);

        }
        else
        {
            winPanelCanvasGroup.DOFade(0f, 0.2f);
            winPanelCanvasGroup.interactable = false;
            winPanelCanvasGroup.blocksRaycasts = false;
        }
        
    }



    #endregion

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



}
