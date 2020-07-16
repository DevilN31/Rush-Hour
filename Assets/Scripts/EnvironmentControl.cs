using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentControl : MonoBehaviour {

    [SerializeField]float spawnCycle = 0.08f;
    float spawnTimer = 0.0f;
    float lightsSpawnCycle = 0.8f;
    float lightsSpawnTimer = 0;

    [Header("All building prefabs")]
    public GameObject[] allBuildingsPrefabs;
    [Header("Buildings parent transform")]
    public Transform allBuildings;
    public Transform buildingsLeft; // Nati
    public Transform buildingsRight; // Nati
    [Header("Street Light")]
    public GameObject streetLight;
    public Transform streetLightsLeft;
    public Transform streetLightsRight;


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

                int randomBuilding = Random.Range(0, allBuildingsPrefabs.Length);
                GameObject building = Instantiate(allBuildingsPrefabs[randomBuilding],buildingsLeft.position,
                    buildingsLeft.rotation,buildingsLeft);

                randomBuilding = Random.Range(0, allBuildingsPrefabs.Length);
                GameObject building2 = Instantiate(allBuildingsPrefabs[randomBuilding],buildingsRight.position,
                    buildingsRight.rotation,buildingsRight);

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
}
