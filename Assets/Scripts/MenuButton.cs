using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    public enum CategoryButtonState
    {
        Active,
        Passive
    }

    public MenuController menuController;
    public GiftType giftType;
    public CategoryButtonState buttonState;
    public string categoryName;


    #region UI Objects
    public GameObject rewaredSticker;
    public GameObject lockImage;
    #endregion

    private void OnEnable()
    {
        GoogleMobileAdsDemoScript.instance.OnRewardedAdCompleted += CheckUnlockCondition;
    }

    private void OnDisable()
    {
        GoogleMobileAdsDemoScript.instance.OnRewardedAdCompleted -= CheckUnlockCondition;
    }

    private void Start()
    {
        buttonState = (PlayerPrefs.GetInt(categoryName + "Unlocked") == 1 ? CategoryButtonState.Active : CategoryButtonState.Passive);
        CheckButtonState();
    }

    private void CheckUnlockCondition(GiftType giftType)
    {
        if(this.giftType== giftType)
        {
            PlayerPrefs.SetInt(categoryName + "Unlocked", 1);
            buttonState = CategoryButtonState.Active;
            CheckButtonState();
        }
    }

    public void CheckButtonState()
    {
        if (buttonState == CategoryButtonState.Active)
        {
            rewaredSticker.gameObject.SetActive(false);
            lockImage.gameObject.SetActive(false);
        }
        else
        {
            rewaredSticker.gameObject.SetActive(true);
            lockImage.gameObject.SetActive(true);
        }
    }

    public void Click()
    {
        if (buttonState == CategoryButtonState.Active)
        {
            menuController.SelectGameMode(categoryName);
        }
        else
        {
            GoogleMobileAdsDemoScript.instance.ShowRewardBasedVideo(this.giftType);
            GameDistribution.Instance.ShowRewardedAd();
            // switch (giftType)
            // {
            //     case GiftType.ForHint:
            //         GameObject.Find("GameManager").GetComponent<GameManager>().IncreaseHintCount(1);
            //         break;
            //     case GiftType.ForExtraSeconds:
            //         GameObject.Find("GameManager").GetComponent<GameManager>().GetExtraSeconds();
            //         break;
            //     case GiftType.ForInGameHint:
            //         GameObject.Find("GameManager").GetComponent<GameManager>().SkipLevel();
            //         break;
            //     case GiftType.ForOffer:
            //         GameObject.Find("GameManager").GetComponent<GameManager>().GetOfferGifts();
            //         break;
            // }
        }
    }


}
