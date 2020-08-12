using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelProgress : MonoBehaviour
{
    public static LevelProgress Instance;

    public Text SwipeToStart;
    public Text SwipeUpToBoost;
    public Text SwipeDownToBrake;

    public Button RestartLevelButton;
    public Slider LevelProgressSlider;
    public GameObject gameOverCanvas; // NATI - added this to be turned off after game reset
    public GameObject PauseGameCanvas; 

    public Image FadeImage;

    public float Timeforlevel;
    public float LevelBarMultiplier = 1f;
    [Space (5)]
    [Header("Level Transmition Animation")]
    public float TimeStartFadeout;
    public float TimeStartFadein;
    public float TimeCallStartLevel;

    public float FadeoutDuration;
    public float FadeinDuration;
    public float FadeMultiplier;

    public float Timer = 0;

    public Animator EndLevelCamAnim;

    public int LevelNumber = 0;
    public Text LevelNum;
    public GameObject PauseGameButton;

    private float FadeJumpIntervals = 0.01f;

    public GameObject MainMenuCanvas; // NATI - Where do you use this?
    public GameObject GameFinishedCanvas;

    bool multiplyBarMultiplier; // NATI

    public bool CanStartDriving = false;

    public bool GamePaused;
    bool unpauseBoost;
    bool unpauseBrake;

    private void Awake()
    {
        if(PlayerPrefs.HasKey("Level Number"))
        {
            LevelNumber = PlayerPrefs.GetInt("Level Number");
            LevelNum.text = "Level: " + LevelNumber.ToString();
        }
        else
        {
            PlayerPrefs.SetInt("Level Number", LevelNumber);
            LevelNum.text = "Level: " + LevelNumber.ToString();
        }

        if(PlayerPrefs.HasKey("number Of Lanes"))
        {
            SpawnScript.instance.numberOfLanes = PlayerPrefs.GetInt("number Of Lanes");
        }
        Instance = this;
    }

    private void Start()
    {
        EndLevelCamAnim.Play("Base Layer.StartLevelAnim", 0);
        GameFinishedCanvas.SetActive(false);
        //CanStartDriving = true;
        StartNextLevel(false);
    }

    void Update()
    {
        if (Manager.Instance.currentGameState == Manager.GameStates.InGame)
        {
            LevelProgressSlider.value += (Time.deltaTime * LevelBarMultiplier) / Timeforlevel;


            if(LevelProgressSlider.value >= 1)
            {
                FinishLevel();
            }
        }

        if (Manager.Instance.obstacleSpeed == Manager.Instance.fastSpeed && !multiplyBarMultiplier) // NATI - change when player speeds up
        {
            multiplyBarMultiplier = true;
            LevelBarMultiplier *= 2; 
        }
        
        else if (Manager.Instance.obstacleSpeed == Manager.Instance.defaultSpeed && multiplyBarMultiplier) // NATI - goes back to normal
        {
            LevelBarMultiplier /= 2;
            multiplyBarMultiplier = false;
        }
        #region First level tutorial
        if (LevelNumber == 1)
        {
            if (PlayerControl.ShowBoosstMessage)
            {
                if (LevelProgressSlider.value >= 0.3f)
                {
                    SwipeUpToBoost.gameObject.SetActive(true);
                    Time.timeScale = 0;
                    GamePaused = true;
                    unpauseBoost = true;
                    Debug.Log("Show boost");

                }
            }
            else
            {
                if (unpauseBoost)
                {
                    SwipeUpToBoost.gameObject.SetActive(false);
                    Time.timeScale = 1;
                    GamePaused = false;
                    unpauseBoost = false;
                    Debug.Log("Hide boost");

                }
            }

            if (PlayerControl.ShowBrakeMessage)
            {
                if (LevelProgressSlider.value >= 0.7f)
                {
                    SwipeDownToBrake.gameObject.SetActive(true);
                    Time.timeScale = 0;
                    GamePaused = true;
                    unpauseBrake = true;
                    Debug.Log("Show brake");
                }
            }
            else
            {
                if (unpauseBrake)
                {
                    SwipeDownToBrake.gameObject.SetActive(false);
                    Time.timeScale = 1;
                    GamePaused = false;
                    unpauseBrake = false;
                    Debug.Log("Hide brake");

                }
            }
        }
        #endregion
    }

    public void StartNextLevel(bool restart) ////// CHANGE ALL LEVEL VALUES HERE SUCH AS DIFFICULTY.
    {
        if (!EnvironmentControl.instance.GameFinished)
        {
            NumLaneOrFinishedGame();
        }

        if (!restart)
        {
            if (EnvironmentControl.instance.GameFinished)
            {
                //LevelNumber = Random.Range(0, 12);

                Timeforlevel = Random.Range(25, 45);
                Debug.Log(LevelNumber);

                SpawnScript.instance.waitForSpawn = Random.Range(0.3f, 0.7f);
                Debug.Log(SpawnScript.instance.waitForSpawn);
                PlayerPrefs.SetFloat("Wait For Spawn", SpawnScript.instance.waitForSpawn);

                SpawnScript.instance.numberOfLanes = Random.Range(3, 6);
                EnvironmentControl.instance.RandomizeBuilding = Random.Range(1, 5);
            }

            if (LevelNumber % 3 == 0) // NATI - change difficulty every 3rd level
                SpawnScript.instance.UpdateWaitTime();
        }


        PlayerControl.CanBeHit = true;
        SpawnScript.instance.SpawnLanes();



        // Instantiate(Manager.Instance.allCarPrefabs[Manager.Instance.currentCar]);
        //  Camera.main.transform.GetComponent<CameraFollow>().ResetCamera();
        StartCoroutine(SpawnPlayer());

        if (LevelNumber != 1)
        {
            Manager.FirstGame = false;
        }
        else
        {
            Manager.FirstGame = true;
        }

        //Manager.Instance.scoreText.gameObject.SetActive(true);

        EndLevelCamAnim.Play("Base Layer.StartLevelAnim", 0);
        //Debug.Log("Check");
        Timer = 0;
        LevelProgressSlider.value = 0;
    }

    public void FinishLevel()
    {
        CanStartDriving = false;
        SoundManager.Instance.StopMusic();
        Manager.Instance.currentGameState = Manager.GameStates.GameOver;
        LevelProgressSlider.gameObject.SetActive(false);
        LevelNum.gameObject.SetActive(false);
        PauseGameButton.SetActive(false);
        StartCoroutine(FadeNextLevel(false));
    }

    public IEnumerator FadeNextLevel(bool restart)
    {
        if (!restart)
        {
            LevelNumber++;
            PlayerPrefs.SetInt("Level Number", LevelNumber);
            LevelNum.text = "Level: " + LevelNumber.ToString();

            EndLevelCamAnim.Play("Base Layer.EndLevelAnim", 0);

            yield return new WaitForSeconds(TimeStartFadein);

            Manager.Instance.currentGameState = Manager.GameStates.MainMenu;

            StartCoroutine(FadeIn());

            yield return new WaitForSeconds(TimeStartFadeout);

            StartCoroutine(FadeOut());

            yield return new WaitForSeconds(TimeCallStartLevel);

            LevelProgressSlider.gameObject.SetActive(true);

            //if (EnvironmentControl.instance.GameFinished)
            //{
            // LevelNum.gameObject.SetActive(false);
            //}
            //else
            //{
            LevelNum.gameObject.SetActive(true);
            PauseGameButton.SetActive(true);
            //}

            StartNextLevel(false);
            gameOverCanvas.SetActive(false);
        }
        else
        {
            Manager.Instance.currentGameState = Manager.GameStates.MainMenu;

            StartCoroutine(FadeIn());

            yield return new WaitForSeconds(TimeStartFadeout);

            StartCoroutine(FadeOut());

            yield return new WaitForSeconds(TimeCallStartLevel);

            StartCoroutine(ClearBuildings());
            LevelProgressSlider.gameObject.SetActive(true);

            //if (EnvironmentControl.instance.GameFinished)
            //{
            //    LevelNum.gameObject.SetActive(false);
            //}
            //else
            //{
            LevelNum.gameObject.SetActive(true);
            PauseGameButton.SetActive(true);
            //}
            CanStartDriving = true;
            SwipeToStart.gameObject.SetActive(true);

            StartNextLevel(true);
            gameOverCanvas.SetActive(false);
        }

    }

    public IEnumerator FadeIn()
    {
        Color C = FadeImage.color;
        Timer = 0;
        while (Timer <= FadeinDuration)
        {
            yield return new WaitForSeconds(FadeJumpIntervals);
            Timer += Time.deltaTime * FadeMultiplier;
            C.a = Mathf.Clamp01(Timer / FadeinDuration);

            FadeImage.color = C;
        }

        RestartGame();
    }

    public IEnumerator FadeOut()
    {
        Color C = FadeImage.color;
        Timer = 0;

        while (Timer <= TimeStartFadeout)
        {
            yield return new WaitForSeconds(FadeJumpIntervals);
            Timer += Time.deltaTime * FadeMultiplier;
            C.a = 1 - Mathf.Clamp01(Timer / TimeStartFadeout);

            FadeImage.color = C;
        }
    }

    public void RestartGame()
    {
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            Destroy(GameObject.FindGameObjectWithTag("Player").gameObject);
        }

        GameObject.Find("ObstacleTruckController").GetComponent<ObstacleTruck>().ResetTruck();

        StartCoroutine(RestartAfterFrame());
    }

    IEnumerator RestartAfterFrame()
    {
        yield return new WaitForEndOfFrame();

        Manager.Instance.RestartGame();
       // Camera.main.transform.GetComponent<CameraFollow>().ResetCamera();
        SoundManager.Instance.StartMusic();
        //Manager.Instance.currentGameState = Manager.GameStates.InGame;
    }

    public IEnumerator SpawnPlayer() // NATI - added this to control player spawn here
    {
        Instantiate(Manager.Instance.allCarPrefabs[Manager.Instance.currentCar]);
        yield return new WaitForEndOfFrame();
        Camera.main.transform.GetComponent<CameraFollow>().ResetCamera();

    }

    IEnumerator ClearBuildings()
    {
        yield return null;

        for (int i = 0; i < EnvironmentControl.instance.buildingsLeft.childCount; i++)
        {
            Destroy(EnvironmentControl.instance.buildingsLeft.GetChild(i).gameObject);
        }

        for (int i = 0; i < EnvironmentControl.instance.buildingsRight.childCount; i++)
        {
            Destroy(EnvironmentControl.instance.buildingsRight.GetChild(i).gameObject);
        }
    }

    public void NumLaneOrFinishedGame()
    {
        if (LevelProgress.Instance.LevelNumber <= 3) // Suburb
        {
            Timeforlevel = 25;
            SpawnScript.instance.numberOfLanes = 2;

            PlayerPrefs.SetInt("number Of Lanes", SpawnScript.instance.numberOfLanes);

        }
        else if (LevelProgress.Instance.LevelNumber <= 6) // City
        {

            Timeforlevel = 30;
            SpawnScript.instance.numberOfLanes = 4;

            PlayerPrefs.SetInt("number Of Lanes", SpawnScript.instance.numberOfLanes);
        }
        else if (LevelProgress.Instance.LevelNumber <= 9) // Desert
        {

            Timeforlevel = 35;
            SpawnScript.instance.numberOfLanes = 3;

            PlayerPrefs.SetInt("number Of Lanes", SpawnScript.instance.numberOfLanes);
        }
        else if (LevelProgress.Instance.LevelNumber <= 12) // Beach
        {

            Timeforlevel = 40;
            SpawnScript.instance.numberOfLanes = 3;

            PlayerPrefs.SetInt("number Of Lanes", SpawnScript.instance.numberOfLanes);

        }
        else if (LevelProgress.Instance.LevelNumber > 12)
        {
            PlayerPrefs.SetInt("Finished Game", 1);
            EnvironmentControl.instance.GameFinished = true;
            GameFinishedCanvas.SetActive(true);
            //LevelNumber = Random.Range(0, 12);
            LevelProgressSlider.gameObject.SetActive(false);

            LevelNum.gameObject.SetActive(false);
            PauseGameButton.gameObject.SetActive(false);
        }
    }

    [ContextMenu("Reset Player Prefs")]
    public void ResetPlayerPreft()
    {
        PlayerPrefs.SetInt("Level Number", 1);
        PlayerPrefs.SetInt("Finished Game", 0);
        PlayerPrefs.SetFloat("Wait For Spawn", 1.5f);
    }

    public IEnumerator RandomizeGame()
    {
        Manager.Instance.currentGameState = Manager.GameStates.MainMenu;

        StartCoroutine(FadeIn());

        yield return new WaitForSeconds(TimeStartFadeout);

        StartCoroutine(FadeOut());

        yield return new WaitForSeconds(TimeCallStartLevel);

        StartCoroutine(ClearBuildings());
        LevelProgressSlider.gameObject.SetActive(true);

        //if (EnvironmentControl.instance.GameFinished)
        //{
        //LevelNum.gameObject.SetActive(false);
        //}
        //else
        //{
        LevelNum.gameObject.SetActive(true);
        PauseGameButton.SetActive(true);

        //}

        StartNextLevel(true);
        GameFinishedCanvas.SetActive(false);
    }

    public void ConnectToRandomizer()
    {
        StartCoroutine(RandomizeGame());
    }


    public void PauseGame()
    {
        Time.timeScale = 0;
        GamePaused = true;
        PauseGameCanvas.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        GamePaused = false;
        PauseGameCanvas.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
