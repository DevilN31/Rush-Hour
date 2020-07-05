using UnityEngine;
using System.Collections;

public class SpawnScript : MonoBehaviour
{

    float spawnCycle = 0.75f;
    float spawnTimer = 0.0f;

    [HideInInspector]
    public float timeElapsed = 0;

    [Header("Spawn Lanes")]
    public Transform[] allLanes;
    [Header("Obstacle Pattern Prefabs")]
    public GameObject[] allObstaclePatterns;
    [Header("Obstacle Parent Transform")]
    public Transform allObstacles;
    [Header("Obstacle Truck Controller")]
    public ObstacleTruck obstacleTruck;
    [Header("Spawn Timers")]
    public float defaultSpawnTime = 0.70f;
    public float fastSpawnTime = 0.4f;

    void Update()
    {
        if (Manager.Instance.currentGameState == Manager.GameStates.InGame)
        {
            spawnTimer += Time.deltaTime;
            timeElapsed += Time.deltaTime;

            if (Manager.Instance.obstacleSpeed == Manager.Instance.defaultSpeed)
            {
                spawnCycle = defaultSpawnTime;
            }
            else
            {
                spawnCycle = fastSpawnTime;
            }
            if (spawnTimer > spawnCycle && (Manager.Instance.obstacleSpeed == Manager.Instance.defaultSpeed || Manager.Instance.obstacleSpeed == Manager.Instance.fastSpeed))
            {
                int randomObstacle = Random.Range(0, allObstaclePatterns.Length);
                GameObject obstacle = Instantiate(allObstaclePatterns[randomObstacle]) as GameObject;
                obstacle.transform.SetParent(allObstacles);

                spawnTimer = 0;
            }

            if (timeElapsed > obstacleTruck.spawnTime && obstacleTruck.currentState == 0)
            {
                obstacleTruck.SetInitParameters();
            }



            foreach (Transform child in allObstacles)
            {
                if (child.childCount == 0)
                {
                    Destroy(child.gameObject);
                }
            }
        }
        
    }
}