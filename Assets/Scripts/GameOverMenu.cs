using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour {

    [Header("Score labels")]
    public Text scoreText;
    public Text highScoreText;
    public void UpdateScore()
    {
        //scoreText.text = Manager.Instance.score.ToString(); // NATI
        //highScoreText.text = "Best: " + Manager.Instance.highScore.ToString(); // NATI

        scoreText.gameObject.SetActive(false); // NATI
        highScoreText.gameObject.SetActive(false); // NATI
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
        /*
        Manager.Instance.RestartGame();
        Camera.main.transform.GetComponent<CameraFollow>().ResetCamera();
        SoundManager.Instance.StartMusic();
        Manager.Instance.currentGameState = Manager.GameStates.MainMenu;
        this.transform.gameObject.SetActive(false);
        */
        //LevelProgress.Instance.LevelNumber = 0;
        //SpawnScript.instance.waitForSpawn = 1.5f;
        LevelProgress.Instance.LevelProgressSlider.gameObject.SetActive(false);
        LevelProgress.Instance.LevelNum.gameObject.SetActive(false);

        StartCoroutine(LevelProgress.Instance.FadeNextLevel(true));
    }

    public void LeaderboardPressed()
    {
        //Manager.Instance.ShowLeaderboard();
    }

}
