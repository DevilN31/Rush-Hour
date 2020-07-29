using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentControl : MonoBehaviour {

    [SerializeField]float spawnCycle = 0.08f;
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
    public float buildingOffsetX = 10;
    [Header("Street Light")]
    public GameObject streetLight;
    public Transform streetLightsLeft;
    public Transform streetLightsRight;
    public float streetLightOffsetX = 5;

    private void Start()
    {
        SetAncorPositions(SpawnScript.instance.allLanes[0].position.x, SpawnScript.instance.allLanes[SpawnScript.instance.allLanes.Count - 1].position.x);
    }
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

                if (Manager.Instance.score < 300) // Suburb
                {
                    spawnCycle = 0.17f;

                    int randomBuilding = Random.Range(0, suburbBuildingsPrefabs.Length);
                    GameObject building = Instantiate(suburbBuildingsPrefabs[randomBuilding], buildingsLeft.position,
                        buildingsLeft.rotation, buildingsLeft);

                    randomBuilding = Random.Range(0, suburbBuildingsPrefabs.Length);
                    GameObject building2 = Instantiate(suburbBuildingsPrefabs[randomBuilding], buildingsRight.position,
                        buildingsRight.rotation, buildingsRight);
                }
                else if (Manager.Instance.score > 300 && Manager.Instance.score < 700) // City
                {
                    spawnCycle = 0.15f;

                    int randomBuilding = Random.Range(0, cityBuildingsPrefabs.Length);
                    GameObject building = Instantiate(cityBuildingsPrefabs[randomBuilding], buildingsLeft.position,
                        buildingsLeft.rotation, buildingsLeft);

                    randomBuilding = Random.Range(0, cityBuildingsPrefabs.Length);
                    GameObject building2 = Instantiate(cityBuildingsPrefabs[randomBuilding], buildingsRight.position,
                        buildingsRight.rotation, buildingsRight);
                }
                else if (Manager.Instance.score > 700 && Manager.Instance.score < 1000) // Desert
                {
                    spawnCycle = 0.19f;

                    int randomBuilding = Random.Range(0, desertBuildingsPrefabs.Length);
                    GameObject building = Instantiate(desertBuildingsPrefabs[randomBuilding], buildingsLeft.position,
                        desertBuildingsPrefabs[randomBuilding].transform.rotation, buildingsLeft);

                    randomBuilding = Random.Range(0, desertBuildingsPrefabs.Length);
                    GameObject building2 = Instantiate(desertBuildingsPrefabs[randomBuilding], buildingsRight.position,
                        desertBuildingsPrefabs[randomBuilding].transform.rotation, buildingsRight);
                }
                else if (Manager.Instance.score > 1000) // Beach
                {
                    spawnCycle = 1f;

                    int randomBuilding = Random.Range(0, beachPrefabs.Length);
                    GameObject building = Instantiate(beachPrefabs[randomBuilding], buildingsLeft.position,
                        buildingsLeft.rotation, buildingsLeft);
                    building.transform.localRotation = Quaternion.Euler(0, 205, 0);

                    randomBuilding = Random.Range(0, beachPrefabs.Length);
                    GameObject building2 = Instantiate(beachPrefabs[randomBuilding], buildingsRight.position,
                       Quaternion.Euler(0,120,0), buildingsRight);
                }
                ////else
                ////{

                ////}
                spawnTimer = 0;
            }

            lightsSpawnTimer += Time.deltaTime;
            if (lightsSpawnTimer > lightsSpawnCycle)
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

                GameObject stLight1 = Instantiate(streetLight,streetLightsLeft.position,
                    streetLightsLeft.rotation,streetLightsLeft);
                stLight1.transform.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

                GameObject stLight2 = Instantiate(streetLight,streetLightsRight.position,
                    streetLightsRight.rotation,streetLightsRight);
                
            }
        }
        
    }

    void SetAncorPositions(float leftLaneX,float rightLaneX)
    {
        buildingsLeft.position = new Vector3(leftLaneX - buildingOffsetX, buildingsLeft.position.y, buildingsLeft.position.z);
        streetLightsLeft.position = new Vector3(leftLaneX - streetLightOffsetX, streetLightsLeft.position.y, streetLightsLeft.position.z);

        buildingsRight.position = new Vector3(rightLaneX + buildingOffsetX, buildingsRight.position.y, buildingsRight.position.z);
        streetLightsRight.position = new Vector3(rightLaneX + streetLightOffsetX, streetLightsRight.position.y, streetLightsRight.position.z);
        rightSideWalk.position = new Vector3(rightLaneX + 2, rightSideWalk.position.y, rightSideWalk.position.z);
    }
}
