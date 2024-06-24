using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScene : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(LoadFirstSceneAsync());
        GameDistribution.Instance.ShowAd();
    }

    IEnumerator LoadFirstSceneAsync()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Menu");
    }
}