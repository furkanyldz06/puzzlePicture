using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using UnityEngine;

public class LeaderboardController : Singleton<LeaderboardController>
{
    #if UNITY_ANDROID
        string leaderboardID = "CgkIz7GRnsMFEAIQAA";

    #elif UNITY_IOS
        string leaderboardID = "hiddenmasterleaders"; 
    #endif

    void Start()
    {
        #if UNITY_ANDROID
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            .AddOauthScope("profile")
            .Build();

            PlayGamesPlatform.DebugLogEnabled = true;
                
            // Initialize and activate the platform
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.Activate();
        #elif UNITY_IOS

        #endif

        if(PlayerPrefs.GetInt("isSilentLoginActive", 0) == 1){
            Authenticate();
        }
    }

    public void ShowLeaderboard(){
        int completedPuzzleCount = PlayerPrefs.GetInt("CompletedPuzzleCount");
        int lastPostedScore = PlayerPrefs.GetInt("LastPostedScore");

        bool userAuthenticated=false;

        #if UNITY_ANDROID
            userAuthenticated = PlayGamesPlatform.Instance.localUser.authenticated;
        #elif UNITY_IOS
            userAuthenticated = Social.localUser.authenticated;
        #endif

        if (!userAuthenticated){
            if(completedPuzzleCount != lastPostedScore && completedPuzzleCount != 0){
                Debug.Log("Process 1");
                Authenticate(PostScoreOnLeaderBoard);
            }
            else{
                Debug.Log("Process 2");
                Authenticate(ShowNativeLeaderboard);
            }
        }
        else{
            if(completedPuzzleCount != lastPostedScore && completedPuzzleCount != 0){
                Debug.Log("Process 3");
                PostScoreOnLeaderBoard();
            }
            else{
                Debug.Log("Process 4");
                ShowNativeLeaderboard();
            }
        }
    }

    private void Authenticate(System.Action callback = null) {
        #if UNITY_ANDROID
            PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptOnce, (result)=>{
                if(result == SignInStatus.Success){
                    Debug.Log("Login Successful");
                    PlayerPrefs.SetInt("isSilentLoginActive", 1);

                    if(callback != null){
                        callback();
                    }
                    
                }
                else{
                    Debug.Log("Login Fail");
                }
            });
        #elif UNITY_IOS
            Social.localUser.Authenticate((bool success) => {
                if(success)
                {
                    Debug.Log("Login Successful");
                    PlayerPrefs.SetInt("isSilentLoginActive", 1);

                    if(callback != null){
                        callback();
                    }
                }
                else
                {
                    Debug.Log("Login Fail");
                }
            });
        #endif
    }

    private void PostScoreOnLeaderBoard(){
        int score = PlayerPrefs.GetInt("CompletedPuzzleCount");

        #if UNITY_ANDROID
            PlayGamesPlatform.Instance.ReportScore(score, leaderboardID, (bool success) => {
                if(success){      
                    Debug.Log("Score reporting is succeeded.");

                    PlayerPrefs.SetInt("LastPostedScore", score);  

                    ShowNativeLeaderboard();
                }
                            
                else{
                    Debug.Log("Score reporting is failed");
                }
            });
        #elif UNITY_IOS
            Social.ReportScore(score, leaderboardID, (bool success) => {
                if(success){      
                    Debug.Log("Score reporting is succeeded.");

                    PlayerPrefs.SetInt("LastPostedScore", score);

                    ShowNativeLeaderboard();
                }
                            
                else{
                    Debug.Log("Score reporting is failed");
                }
            });
        #endif
    }

    private void ShowNativeLeaderboard(){
        #if UNITY_ANDROID
            PlayGamesPlatform.Instance.ShowLeaderboardUI();
        #elif UNITY_IOS
            Social.ShowLeaderboardUI();
        #endif
    }
}
