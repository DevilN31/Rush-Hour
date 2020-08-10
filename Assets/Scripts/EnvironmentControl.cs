using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentControl : MonoBehaviour
{
    public static EnvironmentControl instance;

    [SerializeField] float spawnCycle = 0.08f;
    float spawnTimer = 0.0f;
    float lightsSpawnCycle = 0.8f;
    float lightsSpawnTimer = 0;

    [Header("All building prefabs")]
    public GameObject[] suburbBuildingsPrefabs;
    public GameObject[] cityBuildingsPrefabs;
    public GameObject[] desertBuildingsPrefabs;
    public GameObject[] beachPrefabs;

    [Header("Buildings parent transform")]
    public Transform allBuildings;
    public Transform buildingsLeft; // Nati
    public Transform buildingsRight; // Nati
    public Transform rightSideWalk;
    public float buildingOffsetX = 10; // NATI
    [Header("Street Light")]
    public GameObject streetLight;
    public Transform streetLightsLeft; // NATI
    public Transform streetLightsRight;// NATI
    public float streetLightOffsetX = 5; // NATI

    private int CheckGameFinished; // Avishy 10.8
    [HideInInspector]
    public bool GameFinished; // Avishy 10.8
    private void Awake()
    {
        instance = this;

        if (PlayerPrefs.HasKey("Finished Game"))
        {
            if(PlayerPrefs.GetInt("Finished Game") == 1)
            {
                GameFinished = true;
            }
            else
            {
                GameFinished = false;
            }
        }
        else
        {
            PlayerPrefs.SetInt("Finished Game", 0);
        }
    }
    /*
    private void Start()
    {
        SetAncorPositions(SpawnScript.instance.allLanes[0].position.x, SpawnScript.instance.allLanes[SpawnScript.instance.allLanes.Count - 1].position.x);
    }
    */
    void Update()
    {
        if (Manager.Instance.currentGameState == Manager.GameStates.InGame)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer > spawnCycle)
            {
                /*
                int randomBuilding = Random.Range(0, allBuildingsPrefabs.Length);
                GameObject building = Instantiate(allBuildingsPrefabs[randomBuilding]);
                building.transform.SetParent(allBuildings);

                randomBuilding = Random.Range(0, allBuildingsPrefabs.Length);
                GameObject building2 = Instantiate(allBuildingsPrefabs[randomBuilding]);
                building2.transform.position = new Vector3(building2.transform.position.x * -1, building2.transform.position.y, building2.transform.position.z);
                building2.transform.localRotation = Quaternion.Euler(0, -90, 0);
                building2.transform.SetParent(allBuildings);
                spawnTimer = 0;
                */

                if (!GameFinished)
                {
                    ChooseBuilding();
                    Debug.Log("Normal");
                }
                else
                {
                    ChooseBuilding();
                }
                spawnTimer = 0;
            }

            if(LevelProgress.Instance.LevelNumber < 9)
            {
                lightsSpawnTimer += Time.deltaTime;
                if (lightsSpawnTimer > lightsSpawnCycle) // Spawns Street Lights
                {
                    lightsSpawnTimer = 0;
                    /*
                    GameObject stLight1 = Instantiate(streetLight) as GameObject;
                    stLight1.transform.SetParent(GameObject.Find("StreetLights").transform);
                    stLight1.transform.position = new Vector3(stLight1.transform.position.x, stLight1.transform.position.y, 180);
                    stLight1.transform.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

                    GameObject stLight2 = Instantiate(streetLight) as GameObject;
                    stLight2.transform.SetParent(GameObject.Find("StreetLights").transform);
                    stLight2.transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));
                    stLight2.transform.GetChild(0).transform.localRotation = Quaternion.Euler(new Vector3(90, -90, 0));
                    stLight2.transform.position = new Vector3(stLight2.transform.position.x * -1, stLight2.transform.position.y, 180);
                    */

                    GameObject stLight1 = Instantiate(streetLight, streetLightsLeft.position,
                        streetLightsLeft.rotation, streetLightsLeft);
                    stLight1.transform.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

                    GameObject stLight2 = Instantiate(streetLight, streetLightsRight.position,
                        streetLightsRight.rotation, streetLightsRight);

                }
            }
        }
    }

    public void SetAncorPositions(float leftLaneX, float rightLaneX) // NATI - sets the position for the Empty GameObjects that hold buildings and street lights
    {
        buildingsLeft.position = new Vector3(leftLaneX - buildingOffsetX, buildingsLeft.position.y, buildingsLeft.position.z);
        streetLightsLeft.position = new Vector3(leftLaneX - streetLightOffsetX, streetLightsLeft.position.y, streetLightsLeft.position.z);

        buildingsRight.position = new Vector3(rightLaneX + buildingOffsetX, buildingsRight.position.y, buildingsRight.position.z);
        streetLightsRight.position = new Vector3(rightLaneX + streetLightOffsetX, streetLightsRight.position.y, streetLightsRight.position.z);
        rightSideWalk.position = new Vector3(rightLaneX + 2, rightSideWalk.position.y, rightSideWalk.position.z);
    }


    public void ChooseBuilding()
    {
        if (LevelProgress.Instance.LevelNumber <= 3) // Suburb
        {
            spawnCycle = 0.25f;
            SpawnScript.instance.numberOfLanes = 2;
            PlayerPrefs.SetInt("number Of Lanes", SpawnScript.instance.numberOfLanes);

            int randomBuilding = Random.Range(0, suburbBuildingsPrefabs.Length);
            GameObject building = Instantiate(suburbBuildingsPrefabs[randomBuilding], buildingsLeft.position,
                buildingsLeft.rotation, buildingsLeft);

            randomBuilding = Random.Range(0, suburbBuildingsPrefabs.Length);
            GameObject building2 = Instantiate(suburbBuildingsPrefabs[randomBuilding], buildingsRight.position,
                buildingsRight.rotation, buildingsRight);
        }
        else if (LevelProgress.Instance.LevelNumber <= 6) // City
        {
            spawnCycle = 0.15f;
            SpawnScript.instance.numberOfLanes = 4;

            PlayerPrefs.SetInt("number Of Lanes", SpawnScript.instance.numberOfLanes);
            int randomBuilding = Random.Range(0, cityBuildingsPrefabs.Length);
            GameObject building = Instantiate(cityBuildingsPrefabs[randomBuilding], buildingsLeft.position,
                buildingsLeft.rotation, buildingsLeft);

            randomBuilding = Random.Range(0, cityBuildingsPrefabs.Length);
            GameObject building2 = Instantiate(cityBuildingsPrefabs[randomBuilding], buildingsRight.position,
                buildingsRight.rotation, buildingsRight);
        }
        else if (LevelProgress.Instance.LevelNumber <= 9) // Desert
        {
            spawnCycle = 0.19f;
            SpawnScript.instance.numberOfLanes = 3;
            PlayerPrefs.SetInt("number Of Lanes", SpawnScript.instance.numberOfLanes);

            int randomBuilding = Random.Range(0, desertBuildingsPrefabs.Length);
            GameObject building = Instantiate(desertBuildingsPrefabs[randomBuilding], buildingsLeft.position,
                desertBuildingsPrefabs[randomBuilding].transform.rotation, buildingsLeft);

            randomBuilding = Random.Range(0, desertBuildingsPrefabs.Length);
            GameObject building2 = Instantiate(desertBuildingsPrefabs[randomBuilding], buildingsRight.position,
                desertBuildingsPrefabs[randomBuilding].transform.rotation, buildingsRight);
        }
        else if (LevelProgress.Instance.LevelNumber <= 12) // Beach
        {
            spawnCycle = 1f;
            SpawnScript.instance.numberOfLanes = 3;
            PlayerPrefs.SetInt("number Of Lanes", SpawnScript.instance.numberOfLanes);

            GameObject building = Instantiate(beachPrefabs[0], buildingsLeft.position,
                buildingsLeft.rotation, buildingsLeft);
            building.transform.localRotation = Quaternion.Euler(0, -90, 0);

            GameObject building2 = Instantiate(beachPrefabs[1], buildingsRight.position - new Vector3(7, 0, 0),
               Quaternion.Euler(0, 0, 0), buildingsRight);
        }
    }
}
