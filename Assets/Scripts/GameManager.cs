using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TapticPlugin;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public bool isEmojiLevel;
    public GameUIController gameUIController;
    public DummyInputManager dummyInputManager;
    public LevelBuilder levelBuilder;
    public float timer;
    public GameMode gameMode;
    public ParticleSystem winParticle;

    public static string selectedGameMode;

    public int currentLevel;
    public static Action<GameMode> GameModeChanged;
    public static bool skipWithRewardedAd;

    public int levelCount;

    public RectTransform rewardedAdButton;
    public bool isRewardedAdButtonActive;

    [Header("Hint Fields")]
    #region Hint
    public Text hintCountText;
    public GameObject videoAdImage;
    public int hintCount;
    #endregion

    public GameObject noAdsButton;
    
    public static bool mixedMode;
    public static int mixedModeIndex;


    public static int completedGameCount;

    public override void Awake()
    {
        base.Awake();
        // if (instance == this)
        // {
        //     StoreController.instance.ResetInAppButtons();
        //     for (int i = 0; i < inAppBuyButtons.Length; i++)
        //     {
        //         inAppBuyButtons[i].Initialize();
        //     }
        // }
    }
    private void Start()
    {
        SoundAndMusic.instance.FadeIn();

        if(mixedMode)
        {
            int selectedGameModeIndex = (mixedModeIndex % 3);
            switch (selectedGameModeIndex)
            {
                case 0:
                    selectedGameMode = "Classic";
                    break;
                case 1:
                    selectedGameMode = "Movies";
                    break;
                case 2:
                    selectedGameMode = "Cultures";
                    break;
            }
        }

        switch (selectedGameMode)
        {
            case "Classic":
                levelCount = 51;
                break;
            case  "Movies":
                levelCount = 30;
                break;
            case "Cultures":
                levelCount = 20;
                break;

        }

        hintCount = PlayerPrefs.GetInt("HintCount",5);
        if (isEmojiLevel)
        {
            levelCount = 17;
            currentLevel = PlayerPrefs.GetInt("LastSubmittedLevel" + "Emoji");
        }
        else
        {
            currentLevel = PlayerPrefs.GetInt("LastSubmittedLevel" + selectedGameMode);
        }
        

        if (mixedMode)
        {
            gameUIController.SetLevelIndex();
        }
        else
        {
            gameUIController.SetLevelIndex(currentLevel + 1);
        }


        if (isEmojiLevel)
        {
            EmojiLevelData emojiLevelData = Resources.Load<EmojiLevelData>("EmojiLevels/" + ((currentLevel % levelCount) + 1).ToString());
            GetComponent<EmojiLevelBuilder>().BuildLevel(Win, emojiLevelData);
        }
        else
        {
            LevelData levelData = Resources.Load<LevelData>(selectedGameMode + "/"+((currentLevel % levelCount)+1).ToString());
            levelBuilder.BuildLevel(Win,levelData);
        }

        CheckHintState();
        CheckNoAds();
    }

    public void Restore()
    {
        TapticManager.Impact(ImpactFeedback.Light);
    }

    public void PrivacyPolicy()
    {
        TapticManager.Impact(ImpactFeedback.Light);
        Application.OpenURL("https://www.gitberry.com/privacy-policy");
    }

    public void CheckNoAds()
    {
        // if (PlayerPrefs.GetInt("NoAds") == 1)
        // {
        //     noAdsButton.SetActive(false);
        // }
        // else
        // {
        //     noAdsButton.SetActive(true);
        // }

    }

    public void IncreaseLastSubmittedLevel()
    {

        int completedPuzzleCount = PlayerPrefs.GetInt("CompletedPuzzleCount");
        PlayerPrefs.SetInt("CompletedPuzzleCount", completedPuzzleCount + 1);


        if (isEmojiLevel)
        {
            PlayerPrefs.SetInt("LastSubmittedLevel" + "Emoji", currentLevel + 1);
        }
        else
        {

            if (mixedMode)
            {
                mixedModeIndex++;
            }
            PlayerPrefs.SetInt("LastSubmittedLevel" + selectedGameMode, currentLevel + 1);
        }
    }

    public void Win()
    {
        skipWithRewardedAd = false;
        SoundAndMusic.instance.FadeOut();
        winParticle.Play();
        ChangeGameMode(GameMode.Completed);
        gameUIController.SetWinPanelState(true);
        IncreaseLastSubmittedLevel();
        TapticManager.Notification(NotificationFeedback.Success);
        SoundAndMusic.instance.PlaySoundEffectOneShot(SoundEffectType.Win);

        // GoogleMobileAdsDemoScript.instance.ShowInterstitial();
        GameDistribution.Instance.ShowAd();
    }

    #region Hint Operations

    public void IncreaseHintCount(int count)
    {
        hintCount += count;
        PlayerPrefs.SetInt("HintCount", hintCount);
        CheckHintState();
    }

    public void DecreaseHintCount(int count)
    {
        hintCount -= count;
        PlayerPrefs.SetInt("HintCount", hintCount);
        CheckHintState();
    }

    public void GetHint()
    {
        TapticManager.Impact(ImpactFeedback.Light);
        if (hintCount > 0)
        {
            if (isEmojiLevel)
            {
                if (GetComponent<EmojiLevelBuilder>().ReadyToUseHint()) {
                    DecreaseHintCount(1);
                    GetComponent<EmojiLevelBuilder>().GetHint();
                }
            }
            else
            {
                DecreaseHintCount(1);
                DummyInputManager.instance.ResetZoom();
                DOVirtual.DelayedCall(0.1f, () => {
                    levelBuilder.GetHint();
                });
            }
        }
        else
        {
            GoogleMobileAdsDemoScript.instance.ShowRewardBasedVideo(GiftType.ForHint);
            //Rewarded Video..
            GameDistribution.Instance.ShowRewardedAd();
            // GameObject.Find("GameManager").GetComponent<GameManager>().IncreaseHintCount(1);

        }
        
    }

    [SerializeField] private Animator zoomZoom;
    public void getZoom()
    {
        if (PlayerPrefs.GetInt("Zoom") == 0)
        {
            PlayerPrefs.SetInt("Zoom", 1);
            zoomZoom.SetTrigger("ZoomIn");
        }
        
        else getZoomOut();
    }
    
    public void getZoomOut()
    {
        PlayerPrefs.SetInt("Zoom", 0);
        zoomZoom.SetTrigger("ZoomOut");
    }
    
    public void CheckHintState()
    {
        if (hintCount > 0)
        {
            hintCountText.text = "x"+hintCount.ToString();
            hintCountText.transform.parent.gameObject.SetActive(true);
            videoAdImage.gameObject.SetActive(false);
        }
        else
        {
            hintCountText.transform.parent.gameObject.SetActive(false);
            videoAdImage.gameObject.SetActive(true);
        }
    }

    #endregion

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Pause()
    {
        if (gameMode != GameMode.Paused)
        {
            ChangeGameMode(GameMode.Paused);
            gameUIController.SetSettingsPanelState(true);

        }
    }

    public void OpenStore()
    {
        if (gameMode != GameMode.Paused)
        {
            ChangeGameMode(GameMode.Paused);
            gameUIController.SetStorePanelState(true);
        }
    }

    public void CallOffer()
    {
        if (gameMode != GameMode.Paused)
        {
            ChangeGameMode(GameMode.Paused);
            gameUIController.SetOfferPanelState(true);
        }
    }

    public void Resume()
    {
        ChangeGameMode(GameMode.Active);
        gameUIController.SetSettingsPanelState(false);
        gameUIController.SetStorePanelState(false);
        gameUIController.SetOfferPanelState(false);
    }

    public void RestartGame()
    {
        if (isEmojiLevel)
        {
            SceneManager.LoadScene("Gameplay");
        }
        else
        {
            completedGameCount++;
            if (completedGameCount % 2 == 0)
            {
                SceneManager.LoadScene("EmojiGameplay");
            }
            else
            {
                SceneManager.LoadScene("Gameplay");

            }
        }
    }

    public void SkipLevel()
    {
        skipWithRewardedAd = true;
        IncreaseLastSubmittedLevel();
        RestartGame();
    }

    public void GameOver()
    {
        skipWithRewardedAd = false;
        SoundAndMusic.instance.FadeOut();
        SoundAndMusic.instance.PlaySoundEffectOneShot(SoundEffectType.GameOver);
        ChangeGameMode(GameMode.Failed);
        gameUIController.SetGameOverPanelState(true);
        // GoogleMobileAdsDemoScript.instance.ShowInterstitial();
        GameDistribution.Instance.ShowAd();
    }

    public void ChangeGameMode(GameMode targetGameMode)
    {
        gameMode = targetGameMode;
        if (GameModeChanged != null)
        {
            GameModeChanged(gameMode);

            switch (gameMode)
            {
                case GameMode.Completed:
                case GameMode.Failed:
                case GameMode.Paused:
                    dummyInputManager.isActive = false;
                    break;
                default:
                    dummyInputManager.isActive = true;
                    break;
            }
        }
    }

    public void WatchRewardedVideoForExtraSeconds()
    {
        GameDistribution.Instance.ShowRewardedAd();
        // GameObject.Find("GameManager").GetComponent<GameManager>().GetExtraSeconds();

        GoogleMobileAdsDemoScript.instance.ShowRewardBasedVideo(GiftType.ForExtraSeconds);
    }

    public void WatchRewardedVideoForInGameHint()
    {
        GameDistribution.Instance.ShowRewardedAd();
        // GameObject.Find("GameManager").GetComponent<GameManager>().SkipLevel();

        GoogleMobileAdsDemoScript.instance.ShowRewardBasedVideo(GiftType.ForInGameHint);
    }

    public void WatchRewardedVideoForOffer()
    {
        GameDistribution.Instance.ShowRewardedAd();
        // GameObject.Find("GameManager").GetComponent<GameManager>().GetOfferGifts();
        GoogleMobileAdsDemoScript.instance.ShowRewardBasedVideo(GiftType.ForOffer);
    }

    public void SetRewardedAdButtonState(bool isActive)
    {
        if (skipWithRewardedAd == false)
        {
            rewardedAdButton.DOKill();
            if (isActive)
            {
                rewardedAdButton.DOAnchorPosX(0f, 1f);
            }
            else
            {
                rewardedAdButton.DOAnchorPosX(300f, 0.5f);
            }
            isRewardedAdButtonActive = isActive;
        }
    }


    public void GetOfferGifts()
    {
        IncreaseHintCount(3);
        Resume();
    }

    public void GetExtraSeconds()
    {
        SoundAndMusic.instance.FadeIn();
        timer += 60f;
        ChangeGameMode(GameMode.Active);
        gameUIController.SetGameOverPanelState(false);
    }

    public void MissClick()
    {
        if(PlayerPrefs.GetInt("Zoom") == 1)
            return;
        
        if (gameMode == GameMode.Active && !isEmojiLevel)
        {
            timer = Mathf.Max(0f, timer - 30f);
            gameUIController.MissClick();
            TapticManager.Notification(NotificationFeedback.Error);
            SoundAndMusic.instance.PlaySoundEffectOneShot(SoundEffectType.Wrong);
            //TODO UI Animation...
        }
    }

    private void Update()
    {
        if(gameMode == GameMode.Active && !isEmojiLevel)
        {
            if (timer > 0f)
            {
                timer -= Time.deltaTime;   
            }
            if (timer <= 0f && gameMode != GameMode.Failed)
            {
                GameOver();
            }
            gameUIController.SetTimer(timer);
        }
    }

}

public enum GameMode
{
    Active,
    Paused,
    Completed,
    Failed
}
