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
        Destroy(GameObject.Find("Car").gameObject);
        GameObject.Find("ObstacleTruckController").GetComponent<ObstacleTruck>().ResetTruck();

        StartCoroutine(RestartAfterFrame());
    }

    IEnumerator RestartAfterFrame()
    {
        yield return new WaitForEndOfFrame();

        Camera.main.transform.GetComponent<CameraFollow>().Reset();
        SoundManager.Instance.StartMusic();
        Manager.Instance.currentGameState = Manager.GameStates.InGame;
        this.transform.gameObject.SetActive(false);
        Manager.Instance.RestartGame();
    }

    public void LeaderboardPressed()
    {
        //Manager.Instance.ShowLeaderboard();
    }
}
