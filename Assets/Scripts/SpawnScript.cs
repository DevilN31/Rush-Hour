using UnityEngine;
using System.Collections;
//using Boo.Lang;

public class SpawnScript : MonoBehaviour
{
    public static SpawnScript instance;

    float spawnCycle = 0.75f;
    float spawnTimer = 0.0f;

    [HideInInspector]
    public float timeElapsed = 0;

    [Header("Spawn Lanes")]
    public Transform[] allLanes; 
    [Header("Obstacle Pattern Prefabs")]
    public GameObject[] allObstaclePatterns; // NATI: not in use
    public ObstacleScript[] allObstaclesPrefabs; // NATI: That's where all the obstacle prefabs are
    [Header("Obstacle Parent Transform")]
    public Transform allObstacles; // NATI: not in use
    [Header("Obstacle Truck Controller")]
    public ObstacleTruck obstacleTruck;
    [Header("Spawn Timers")]
    public float defaultSpawnTime = 0.70f;
    public float fastSpawnTime = 0.4f;

    public float waitForSpawn; // NATI: variable for ObstacleSpawn Courutine
    public bool addDifficulty = false;

    private void Awake()
    {
        if (instance != null)
            Destroy(this);

        instance = this;
    }

    void Update()
    {
        
        if(addDifficulty) // NATI: Adds Difficulty
        {
            defaultSpawnTime -= 0.2f;
            waitForSpawn += 0.5f;
            addDifficulty = false;
        }  

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
                /*
                int randomObstacle = Random.Range(0, allObstaclePatterns.Length);
                GameObject obstacle = Instantiate(allObstaclePatterns[randomObstacle]) as GameObject;
                obstacle.transform.SetParent(allObstacles);
                */
                StartCoroutine(SpawnObstacles(waitForSpawn));

                if(Manager.Instance.score % 100 == 0) // NATI: Updates bool addDifficulty
                {
                    addDifficulty = true;
                    Debug.Log("Add defficulty");
                }

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

    IEnumerator SpawnObstacles(float spawnTimer) // NATI: new control function
    {
        int numberOfLanes = Random.Range(0, allLanes.Length); //Randon number of lanes to select where obstacles will spawn 
        for(int i = 0; i< numberOfLanes; i++)
        {
            int randomObstacle = Random.Range(0, allObstaclesPrefabs.Length);
            int randomLane = Random.Range(0, allLanes.Length);
            Instantiate(allObstaclesPrefabs[randomObstacle],allLanes[randomLane].position, allLanes[randomLane].rotation, allLanes[randomLane]);
            yield return new WaitForSeconds(spawnTimer);
        }
    }
}