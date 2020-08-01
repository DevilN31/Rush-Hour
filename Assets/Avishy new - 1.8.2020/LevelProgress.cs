using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelProgress : MonoBehaviour
{
    public static LevelProgress Instance;
    public Slider LevelProgressSlider;
    public Image FadeImage;

    public float Timeforlevel;
    public float LevelBarMultiplier = 1f;

    public float TimeStartFadeout;
    public float TimeStartFadein;
    public float TimeCallStartLevel;

    public float FadeoutDuration;
    public float FadeinDuration;
    public float FadeMultiplier;

    public float Timer = 0;

    public Animator EndLevelCamAnim;

    public int LevelNumber = 0;

    private float FadeJumpIntervals = 0.01f;


    public GameObject MainMenuCanvas;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        EndLevelCamAnim.Play("Base Layer.StartLevelAnim", 0);

        StartNextLevel();
    }

    public void StartNextLevel() ////// CHANGE ALL LEVEL VALUES HERE SUCH AS DIFFICULTY.
    {
        LevelNumber++;

        if (LevelNumber != 1)
        {
            Manager.FirstGame = false;
        }
        else
        {
            Manager.FirstGame = true;
        }

        Manager.Instance.scoreText.gameObject.SetActive(true);

        EndLevelCamAnim.Play("Base Layer.StartLevelAnim", 0);

        Debug.Log("Check");

        Timer = 0;
        LevelProgressSlider.value = 0;
    }

    public void FinishLevel()
    {
        Manager.Instance.currentGameState = Manager.GameStates.GameOver;

        StartCoroutine(FadeNextLevel());
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
    }

    public IEnumerator FadeNextLevel()
    {
        EndLevelCamAnim.Play("Base Layer.EndLevelAnim", 0);

        yield return new WaitForSeconds(TimeStartFadein);

        Manager.Instance.currentGameState = Manager.GameStates.MainMenu;

        StartCoroutine(FadeIn());

        yield return new WaitForSeconds(TimeStartFadeout);

        StartCoroutine(FadeOut());

        yield return new WaitForSeconds(TimeCallStartLevel);


        StartNextLevel();
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
        Camera.main.transform.GetComponent<CameraFollow>().ResetCamera();
        SoundManager.Instance.StartMusic();
        //Manager.Instance.currentGameState = Manager.GameStates.InGame;
    }


}
