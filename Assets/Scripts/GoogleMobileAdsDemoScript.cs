using System;
using UnityEngine;
using GoogleMobileAds.Api;

// Example script showing how to invoke the Google Mobile Ads Unity plugin.
public class GoogleMobileAdsDemoScript : MonoBehaviour
{
    public static GoogleMobileAdsDemoScript instance;
    public InterstitialAd interstitial;
    private RewardBasedVideoAd rewardBasedVideo;
    private BannerView bannerView;
    public bool interstitialRequested;

    public GiftType giftType;

    public float interstitialTimer;

    public Action<GiftType> OnRewardedAdCompleted;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        
    }

    public void Start()
    {
        InvokeRepeating("CheckBanner", 5f, 5f);
#if UNITY_ANDROID
        string appId = "ca-app-pub-4426570100779360~9146065035";
#elif UNITY_IPHONE
        string appId = "ca-app-pub-4426570100779360~9696729379";
#else
        string appId = "unexpected_platform";
#endif
        MobileAds.Initialize(appId);
        
        this.rewardBasedVideo = RewardBasedVideoAd.Instance;

        // RewardBasedVideoAd is a singleton, so handlers should only be registered once.
        this.rewardBasedVideo.OnAdLoaded += this.HandleRewardBasedVideoLoaded;
        this.rewardBasedVideo.OnAdFailedToLoad += this.HandleRewardBasedVideoFailedToLoad;
        this.rewardBasedVideo.OnAdOpening += this.HandleRewardBasedVideoOpened;
        this.rewardBasedVideo.OnAdStarted += this.HandleRewardBasedVideoStarted;
        this.rewardBasedVideo.OnAdRewarded += this.HandleRewardBasedVideoRewarded;
        this.rewardBasedVideo.OnAdClosed += this.HandleRewardBasedVideoClosed;
        this.rewardBasedVideo.OnAdLeavingApplication += this.HandleRewardBasedVideoLeftApplication;

        this.RequestRewardBasedVideo();
        this.RequestInterstitial();
        this.RequestBanner();
        interstitialTimer = 30f;
    }

    private void FixedUpdate()
    {
        interstitialTimer += Time.deltaTime;
    }

    #region Banner Fields

    private void CheckBanner()
    {
        if (this.bannerView != null)
        {
            if (PlayerPrefs.GetInt("NoAds") == 1)
            {
                this.bannerView.Hide();
            }
            else
            {
                this.bannerView.Show();
            }
        }
    }

    private void RequestBanner()
    {
        // These ad units are configured to always serve test ads.
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = "ca-app-pub-4426570100779360/5757484368";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-4426570100779360/1459146705";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Clean up banner ad before creating a new one.
        if (this.bannerView != null)
            {
                this.bannerView.Destroy();
            }
        if (PlayerPrefs.GetInt("NoAds") == 0)
        {
            // Create a 320x50 banner at the top of the screen.
            this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

            // Register for ad events.
            this.bannerView.OnAdLoaded += this.HandleAdLoaded;
            this.bannerView.OnAdFailedToLoad += this.HandleAdFailedToLoad;
            this.bannerView.OnAdOpening += this.HandleAdOpened;
            this.bannerView.OnAdClosed += this.HandleAdClosed;
            this.bannerView.OnAdLeavingApplication += this.HandleAdLeftApplication;
            // Load a banner ad.
            this.bannerView.LoadAd(this.CreateAdRequest());
        }
    }

    #region Banner callback handlers

    public void HandleAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
    }

    public void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: " + args.Message);
    }

    public void HandleAdOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
    }

    public void HandleAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
    }

    public void HandleAdLeftApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeftApplication event received");
    }

    #endregion

    #endregion

    public bool IsRewardedVideoReady()
    {
        if (rewardBasedVideo.IsLoaded()==false)
        {
            RequestRewardBasedVideo();
        }
        return rewardBasedVideo.IsLoaded();
    }

    private void RequestRewardBasedVideo()
    {
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = "ca-app-pub-4426570100779360/7832983368";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-4426570100779360/8950623915";
#else
        string adUnitId = "unexpected_platform";
#endif
        this.rewardBasedVideo.LoadAd(this.CreateAdRequest(), adUnitId);
    }

    public void ShowRewardBasedVideo(GiftType giftType)
    {


        if (this.rewardBasedVideo.IsLoaded())
        {
            this.giftType = giftType;
            // this.rewardBasedVideo.Show();

            HandleRewardBasedVideoRewarded(null, null);

// #if UNITY_EDITOR
//             HandleRewardBasedVideoRewarded(null, null);
// #endif
        }
        else
        {
            MonoBehaviour.print("Reward based video ad is not ready yet");
        }
    }


    #region RewardBasedVideo callback handlers

    public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoLoaded event received");
    }

    public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        RequestRewardBasedVideo();
        MonoBehaviour.print(
            "HandleRewardBasedVideoFailedToLoad event received with message: " + args.Message);
    }

    public void HandleRewardBasedVideoOpened(object sender, EventArgs args)
    {
        SoundAndMusic.instance.FadeOut();
        MonoBehaviour.print("HandleRewardBasedVideoOpened event received");
    }

    public void HandleRewardBasedVideoStarted(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoStarted event received");
    }

    public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
    {
        RequestRewardBasedVideo();
        MonoBehaviour.print("HandleRewardBasedVideoClosed event received");
    }

    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        switch (giftType)
        {
            case GiftType.ForHint:
                GameObject.Find("GameManager").GetComponent<GameManager>().IncreaseHintCount(1);
                break;
            case GiftType.ForExtraSeconds:
                GameObject.Find("GameManager").GetComponent<GameManager>().GetExtraSeconds();
                break;
            case GiftType.ForInGameHint:
                GameObject.Find("GameManager").GetComponent<GameManager>().SkipLevel();
                break;
            case GiftType.ForOffer:
                GameObject.Find("GameManager").GetComponent<GameManager>().GetOfferGifts();
                break;
        }
        if (OnRewardedAdCompleted != null)
        {
            OnRewardedAdCompleted(giftType);
        }

        //string type = args.Type;
        //double amount = args.Amount;
        //MonoBehaviour.print(
        //    "HandleRewardBasedVideoRewarded event received for " + amount.ToString() + " " + type);
    }

    public void HandleRewardBasedVideoLeftApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoLeftApplication event received");
    }

    #endregion

    // Returns an ad request with custom ad targeting.
    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder().Build();
    }


    private void RequestInterstitial()
    {
        if (PlayerPrefs.GetInt("NoAds") == 1)
        {
            return;
        }

        // These ad units are configured to always serve test ads.
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = "ca-app-pub-4426570100779360/5709382905";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-4426570100779360/1398409280";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Clean up interstitial ad before creating a new one.
        if (this.interstitial != null)
        {
            this.interstitial.Destroy();
        }

        // Create an interstitial.
        this.interstitial = new InterstitialAd(adUnitId);

        // Register for ad events.
        this.interstitial.OnAdLoaded += this.HandleInterstitialLoaded;
        this.interstitial.OnAdFailedToLoad += this.HandleInterstitialFailedToLoad;
        this.interstitial.OnAdOpening += this.HandleInterstitialOpened;
        this.interstitial.OnAdClosed += this.HandleInterstitialClosed;
        this.interstitial.OnAdLeavingApplication += this.HandleInterstitialLeftApplication;

        // Load an interstitial ad.
        this.interstitial.LoadAd(this.CreateAdRequest());
        interstitialRequested = true;
    }


    public void ShowInterstitial()
    {
        if (this.interstitial.IsLoaded() && PlayerPrefs.GetInt("NoAds")==0)
        {
            if (interstitialTimer >= RemoteDataController.instance.interstitialTimer)
            {
                this.interstitial.Show();
                interstitialTimer = 0f;
            }
        }
        else
        {
            if (interstitialRequested == false)
            {
                this.RequestInterstitial();
            }
            MonoBehaviour.print("Interstitial is not ready yet");
        }
    }

    #region Interstitial callback handlers

    public void HandleInterstitialLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleInterstitialLoaded event received");
        interstitialRequested = false;
    }

    int counter = 10;
    public void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        interstitialRequested = false;
        MonoBehaviour.print(
            "HandleInterstitialFailedToLoad event received with message: " + args.Message);
        counter--;
        if (counter > 0)
        {
            this.RequestInterstitial();
        }
    }

    public void HandleInterstitialOpened(object sender, EventArgs args)
    {
        SoundAndMusic.instance.FadeOut();
        MonoBehaviour.print("HandleInterstitialOpened event received");
    }

    public void HandleInterstitialClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleInterstitialClosed event received");
        if (interstitialRequested == false)
        {
            this.RequestInterstitial();
        }
    }

    public void HandleInterstitialLeftApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleInterstitialLeftApplication event received");
    }

    #endregion

}
public enum GiftType
{
    ForHint,
    ForExtraSeconds,
    ForInGameHint,
    UnlockMovies,
    UnlockCultures,
    UnlockMixed,
    ForOffer
}
