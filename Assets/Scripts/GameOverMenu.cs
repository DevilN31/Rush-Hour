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
        scoreText.text = Manager.Instance.score.ToString();
        highScoreText.text = "Best: " + Manager.Instance.highScore.ToString();
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
        StartCoroutine(LevelProgress.Instance.FadeNextLevel());
       
    }

    public void LeaderboardPressed()
    {
        //Manager.Instance.ShowLeaderboard();
    }
}
