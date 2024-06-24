using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class RemoteDataController : Singleton<RemoteDataController>
{


    public bool forceUpdateRequared;
    private string forceUpdateVersion = "0";

    public int interstitialTimer=30;
    public int interstitialTimerFirst = 15;


    private bool firebaseInitialized;
    IEnumerator Start()
    {

        InitializeFirebaseComponents();
        while (!firebaseInitialized)
        {
            yield return null;
        }
        try
        {
            GoogleMobileAdsDemoScript.instance.interstitialTimer = interstitialTimerFirst;
        }
        catch (System.Exception ex)
        {
        }
        yield return null;
        CheckForceUpdateState();
        Debug.Log("REMOTE DATA LOADED");
    }

    void InitializeFirebaseComponents()
    {
    }

   
    #region Check State
    private void CheckForceUpdateState()
    {
        float currentVersionValue = GetVersionValue(Application.version);
        float cloudVersionValue = GetVersionValue(forceUpdateVersion);
        forceUpdateRequared = (cloudVersionValue > currentVersionValue);

        if (forceUpdateRequared)
        {
            //TODO...
            if (Camera.main && Camera.main.GetComponent<MenuController>())
            {
                Camera.main.GetComponent<MenuController>().SetForceUpdatePanelState(true);
            }
        }

    }
    #endregion

    #region Version Check
    private float GetVersionValue(string versionString)
    {
        float versionValue = 0;
        string[] partialVersionCodes = versionString.Split('.');
        for (int i = 0; i < partialVersionCodes.Length; i++)
        {
            versionValue += int.Parse(partialVersionCodes[i]) * ((i == 0) ? 1 : (Mathf.Pow(10, i * -2)));
        }
        return versionValue;
    }
    #endregion


}