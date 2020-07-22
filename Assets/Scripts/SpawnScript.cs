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
    //[Header("Spawn Timers")]
    //public float defaultSpawnTime = 0.7f;
    //public float fastSpawnTime = 0.4f;

    public float waitForSpawn; // NATI: variable for ObstacleSpawn Courutine
    public bool addDifficulty;

    public float DistanceToSpawnObstacle = 20f;

    private bool canSpawn = true;

    private bool DoOnce = true;

    private void Awake()
    {
        if (instance != null)
            Destroy(this);

        instance = this;

        canSpawn = true;
    }

    void Update()
    {
        
        //if(addDifficulty) // NATI: Adds Difficulty
        //{
            //if(defaultSpawnTime > 0.4) /// Limit
            //{
            //defaultSpawnTime -= 0.2f;
           // Debug.Log("This");
            //addDifficulty = false;
            //waitForSpawn += 0.5f;               
            //}
        //}  

        if (Manager.Instance.currentGameState == Manager.GameStates.InGame)
        {
           // spawnTimer += Time.deltaTime;
            timeElapsed += Time.deltaTime;

            //if (Manager.Instance.obstacleSpeed == Manager.Instance.defaultSpeed)
            //{
            //spawnCycle = defaultSpawnTime;
            //Debug.Log("here");
            //}
            //else
            //{
            //spawnCycle = fastSpawnTime;
            //}

            //if (spawnTimer > spawnCycle && (Manager.Instance.obstacleSpeed == Manager.Instance.defaultSpeed || Manager.Instance.obstacleSpeed == Manager.Instance.fastSpeed))
            //{
            /*
            int randomObstacle = Random.Range(0, allObstaclePatterns.Length);
            GameObject obstacle = Instantiate(allObstaclePatterns[randomObstacle]) as GameObject;
            obstacle.transform.SetParent(allObstacles);
            */

            if (canSpawn)
            {
                canSpawn = false;
                StartCoroutine(SpawnObstacles(waitForSpawn));
            }

            if (Manager.Instance.score % 300 == 0) //&& Manager.Instance.score != 0 && DoOnce) // NATI: Updates bool addDifficulty
            {
                UpdateWaitTime();
            }

            //spawnTimer = 0;
            //}

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

    public void UpdateWaitTime()
    {
        if(waitForSpawn > 0.2f)
        waitForSpawn -= 0.1f;

        Debug.Log("Add defficulty");

       // DoOnce = false;
    }
    IEnumerator SpawnObstacles(float spawnTimer) // NATI: new control function
    {
        int numberOfLanes = Random.Range(0, allLanes.Length);  //Randon number of cars spawn
        for (int i = 0; i< numberOfLanes; i++)
        {
            int randomObstacle = Random.Range(0, allObstaclesPrefabs.Length);
            int randomLane = Random.Range(0, allLanes.Length); //Randon number of lanes to select where obstacles will spawn

            if ((allLanes[randomLane].childCount == 0))
            {
                Instantiate(allObstaclesPrefabs[randomObstacle], allLanes[randomLane].position, Quaternion.identity, allLanes[randomLane]);
            }
            else
            {
                if (Vector3.Distance(allLanes[randomLane].position, allLanes[randomLane].GetChild(allLanes[randomLane].childCount - 1).transform.position) > DistanceToSpawnObstacle)
                {
                    Instantiate(allObstaclesPrefabs[randomObstacle], allLanes[randomLane].position, Quaternion.identity, allLanes[randomLane]);
                }
            }

            yield return new WaitForSeconds(spawnTimer);
        }

        canSpawn = true;
        yield return null;
    }
}