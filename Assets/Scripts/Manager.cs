using UnityEngine;
using UnityEngine.UI;
//using GoogleMobileAds.Api;
using System.Collections.Generic;
//using UnityEngine.SocialPlatforms.GameCenter;
//using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class Manager : Singleton<Manager>
{
    protected Manager() { } // guarantee this will be always a singleton only - can't use the constructor!

    //InterstitialAd interstitial;
    //BannerView bannerView;
    private bool showAds = true;
    private Color lerpedColor;
    private bool changeFogColor;
    float t = 0;
    int fogIndex = 0;
    int frameCount = 0;
    //float totalTimeElapsed = 0;

    
    public Vector3 buildingSlowSpeed { get { return new Vector3(0, 0, -0.4f); } }
    public Vector3 buildingDefaultSpeed { get { return new Vector3(0, 0, -1.3f); } }
    public Vector3 buildingsFastSpeed { get { return new Vector3(0, 0, -1.8f); } }

    public Vector3 defaultSpeed { get { return new Vector3(0, 0, -60); } }
    public Vector3 slowSpeed { get { return new Vector3(0, 0, -1); } }
    public Vector3 fastSpeed { get { return new Vector3(0, 0, -100); } }
    public Vector3 gameOverSpeed { get { return new Vector3(0, 0, 20); } }

    public int defaultHealth { get { return 3; } }
    

    public enum GameStates
    {
        MainMenu,
        InGame,
        GameOver,
    }
    [HideInInspector]
    public GameStates currentGameState = GameStates.MainMenu;
    [HideInInspector]
    public Vector3 obstacleSpeed = new Vector3(0, 0, -30);
    [HideInInspector]
    public Vector3 buildingsSpeed = new Vector3(0, 0, -1.3f);
    [HideInInspector]
    public int highScore;
    [HideInInspector]
    public int score = 0;
    [HideInInspector]
    public int currentCar = 0;
    [HideInInspector]
    public bool isMainMenuButtonPressed = false;
    [HideInInspector]
    public bool inShop = false;

    [Header("Game Over")]
    public GameObject gameOverCanvas;

    [Header("Fog Colors")]
    public Color[] allFogColors;
    public int fogChangeScore = 20;
    [Header("Player Cars")]
    public GameObject[] allCarPrefabs;
    [Header("Score")]
    public Text scoreText;


    public static bool FirstGame = true; ////// In Future this will decide if the player has started a "run" after launch
                                         ////// If its the app has been opened.

    //[Header("Vungle")]
    //public string VUNGLE_ANDROID_APP_ID = "58ad5e5065d21e700e00005b";
    //public string VUNGLE_IOS_APP_ID = "56c42e2675fc0c6b5b00004d";
    //[Header("Ad Mob")]
    //public string ADMOB_IOS_BANNER_ID = "ca-app-pub-5620319074562916/5568055189";
    //public string ADMOB_ANDROID_BANNER_ID = "ca-app-pub-5620319074562916/2116387188";
    //public string ADMOB_IOS_INTERSTITIAL_ID = "ca-app-pub-5620319074562916/7044788387";
    //public string ADMOB_ANDROID_INTERSTITIAL_ID = "";

    //[Header("Game Center")]
    //public string GAMECENTER_LEADERBOARD_STRING = "com.test.highscore";
    //public string GOOGLEPLAY_LEADERBOARD_STRING = "";

    //[Header("Score Increment")]
    //public int scoreFrameCount = 20;

    public SwipeStates moveDir = 0; //0 = idle , 1 = left , 2 = right , 3 = up , 4 = down
    public enum SwipeStates
    {
        Idle,
        Left,
        Right,
        Up,
        Down
    }

   


    void Awake()
    {
        currentCar = PlayerPrefs.GetInt("currentCar", 0);
        highScore = PlayerPrefs.GetInt("highScore", 0);

        Instantiate(allCarPrefabs[currentCar]);// WTF IS THIS?!?!?!?
        //go.name = "Car";// Let's change "WTF IS THIS" to "Car"

        gameOverCanvas.gameObject.SetActive(false);
        currentGameState = GameStates.MainMenu;

        //showAds = PlayerPrefs.GetInt("ShowAds" , 1) == 1 ? true : false;

//        if (showAds)
//        {
//            RequestBanner();
//            RequestInterstitial();
//#if UNITY_IPHONE
//            Vungle.init(VUNGLE_ANDROID_APP_ID, VUNGLE_IOS_APP_ID);
//#endif
//        }

        buildingsSpeed = buildingDefaultSpeed;

//#if UNITY_IPHONE
//        Social.localUser.Authenticate(ProcessAuthentication);
//#elif UNITY_ANDROID
//        //PlayGamesPlatform.Activate();
//        LogIn();
//#endif
    }

    //public void LogIn()
    //{
    //    Social.localUser.Authenticate((bool success) =>
    //    {
    //        if (success)
    //        {
    //            Debug.Log("Login Sucess");
    //        }
    //        else
    //        {
    //            Debug.Log("Login failed");
    //        }
    //    });
    //}

    //void ProcessAuthentication(bool success)
    //{
    //    if (success)
    //    {
    //        Debug.Log("Authenticated, checking achievements");

    //    }
    //    else
    //        Debug.Log("Failed to authenticate");
    //}

    void Update()
    {
        if (Manager.Instance.currentGameState == Manager.GameStates.InGame)
        {
            //int toCount = scoreFrameCount;
            if (obstacleSpeed == fastSpeed)
            {
                score += 2;
            }
            //toCount = scoreFrameCount/2;
            else if (obstacleSpeed == defaultSpeed)
            {
                score++;
            }
            else if (obstacleSpeed == slowSpeed)
            {
                score--;
            }

            //toCount = scoreFrameCount + 10;
            //if (frameCount % toCount == 0 && frameCount != 0)
            //{
            //frameCount = 0;
            //}

            //frameCount++;

            //totalTimeElapsed += Time.deltaTime;
            scoreText.text = score.ToString();

            if (score % fogChangeScore == 0 && score != 0 && !changeFogColor)
            {
                t = 0;
                changeFogColor = true;
            }
        }

        if (changeFogColor)
        {
            ChangeFog();
        }

    }


    void ChangeFog()
    {
        lerpedColor = Color.Lerp(RenderSettings.fogColor, allFogColors[fogIndex], t);
        
        if (t < 1 )
        {
            t += Time.deltaTime;
            RenderSettings.fogColor = lerpedColor;
        }
        else
        {
            changeFogColor = false;
            t = 0;
            fogIndex++;
            if (fogIndex == allFogColors.Length)
                fogIndex = 0;
        }
        
    }

    public void RestartGame()
    {
        score = 0;
        scoreText.text = "0";

        //totalTimeElapsed = 0;

        //SpawnScript.instance.defaultSpawnTime = 0.75f;
        SpawnScript.instance.waitForSpawn = 1f;

        for (int i = 0; i < SpawnScript.instance.allLanes.Length; i++) 
        {
            foreach (Transform child in SpawnScript.instance.allLanes[i])
                Destroy(child.gameObject); 
        }

        //instantiate car
        GameObject go = Instantiate(allCarPrefabs[currentCar]);
        //go.name = "Car";
    }

    public void ShowGameOver()
    {
        //highScore = 3;
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("highScore", highScore);

            //SubmitHighScore(highScore);

        }
        gameOverCanvas.GetComponent<GameOverMenu>().UpdateScore();
        //currentGameState = GameStates.GameOver;

        gameOverCanvas.gameObject.SetActive(true);
    }

    //    public void RequestBanner()
    //    {
    //#if UNITY_ANDROID
    //        string adUnitId = ADMOB_ANDROID_BANNER_ID;
    //#elif UNITY_EDITOR
    //        string adUnitId = "unused";
    //#elif UNITY_IPHONE
    //        string adUnitId = ADMOB_IOS_BANNER_ID;
    //#else
    //        string adUnitId = "unexpected_platform";
    //#endif
    //        // Create a 320x50 banner at the top of the screen.
    //        //bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
    //        //// Create an empty ad request.
    //        //AdRequest request = new AdRequest.Builder().Build();
    //        //// Load the banner with the request.
    //        //bannerView.LoadAd(request);
    //    }

    //    public void RequestInterstitial()
    //    {
    //#if UNITY_ANDROID
    //        string adUnitId = ADMOB_ANDROID_INTERSTITIAL_ID;
    //#elif UNITY_EDITOR
    //        string adUnitId = "unused";
    //#elif UNITY_IPHONE
    //        string adUnitId = ADMOB_IOS_INTERSTITIAL_ID;
    //#else
    //        string adUnitId = "unexpected_platform";
    //#endif

    //        // Initialize an InterstitialAd.
    //        //interstitial = new InterstitialAd(adUnitId);
    //        //// Create an empty ad request.
    //        ////AdRequest request = new AdRequest.Builder().AddTestDevice("5F650210F4E6AA46A8FDC9522A96A360").Build();
    //        //AdRequest request = new AdRequest.Builder().Build();
    //        //// Load the interstitial with the request.
    //        //interstitial.LoadAd(request);
    //    }

    //    public void ShowInterstitial()
    //    {
    //        if (showAds)
    //        {

    //#if UNITY_IPHONE
    //            if (interstitial.IsLoaded())
    //            {
    //                interstitial.Show();
    //            }
    //            else
    //            {
    //                Dictionary<string, object> options = new Dictionary<string, object>();
    //                options["orientation"] = VungleAdOrientation.All;
    //                Vungle.playAdWithOptions(options);
    //            }

    //#elif UNITY_ANDROID
    //            //if (interstitial.IsLoaded())
    //            //{
    //            //    interstitial.Show();
    //            //}
    //#endif

    //        }
    //    }


    //void OnApplicationQuit()
    //{
    //    if (showAds)
    //    {
    //        //interstitial.Destroy();
    //        //bannerView.Destroy();
    //    }

    //}

    //#if UNITY_IPHONE
    //    void OnApplicationPause(bool pauseStatus)
    //    {
    //        if (pauseStatus)
    //        {
    //            Vungle.onPause();
    //        }
    //        else
    //        {
    //            Vungle.onResume();
    //        }
    //    }

    //#endif
//    public void SubmitHighScore(int highScore)
//    {
//#if UNITY_IPHONE
//        if (Social.localUser.authenticated)
//            Social.ReportScore(highScore, GAMECENTER_LEADERBOARD_STRING, (bool success) =>
//            {
//                if (success)
//                {
//                    Debug.Log("Update Score Success");

//                }
//                else
//                {
//                    Debug.Log("Update Score Fail");
//                }
//            });
////#elif UNITY_ANDROID
////        if (Social.localUser.authenticated)
////        {
////            Social.ReportScore(highScore, GOOGLEPLAY_LEADERBOARD_STRING, (bool success) =>
////            {
////                if (success)
////                {
////                    Debug.Log("Update Score Success");

////                }
////                else
////                {
////                    Debug.Log("Update Score Fail");
////                }
////            });
////        }
//#endif
//    }


    //    public void ShowLeaderboard()
    //    {
    //#if UNITY_IPHONE
    //        Social.ShowLeaderboardUI();
    //#elif UNITY_ANDROID
    //        ((PlayGamesPlatform)Social.Active).ShowLeaderboardUI(GOOGLEPLAY_LEADERBOARD_STRING);
    //#endif
    //    }

    //    public void RemoveAds()
    //    {
    //        if (bannerView != null)
    //            bannerView.Destroy();

    //        if (interstitial != null)
    //            interstitial.Destroy();
    //    }
}
