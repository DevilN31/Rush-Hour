using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObstacleTruck : MonoBehaviour {

    public enum TruckStates
    {
        idle,
        initiateSequence,
        startTruckMovement,
        stopRedBlinking
    }

    public bool initiateSequence = false;
    public float waitTime = 0.0f;

    public GameObject redLine;
    private int frameCount = 0;
    public TruckStates currentState;

    bool blinkRed = false;
    bool moveTruck = false;

    public float spawnTime = 0.0f;
    int currentLane = 0;
    float truckSpeed = 0.4f;

    public GameObject fireTruck;
    public GameObject policeCar;

    int currentObstacle = 0; //0 = firetruck , 1= policecar
    public Texture blueTexture;
    public Texture redTexture;

    public float minTime = 10.0f;
    public float maxTime = 20.0f;

    public float resetDistance = 130.0f;
    public float initialSpawnZ = -38.0f;
	// Use this for initialization
	void Start ()
    {
        currentState = TruckStates.idle;
        spawnTime = Random.Range(minTime, maxTime);
    }

    public void SetInitParameters()
    {
        currentState = TruckStates.initiateSequence;
        currentLane = Random.Range(0, SpawnScript.instance.allLanes.Count);
        currentObstacle = Random.Range(0, 2);
        truckSpeed = 0.4f;

        if (currentObstacle == 0)
        {
            for (int i = 0; i < redLine.transform.childCount; i++)
            {
                redLine.transform.GetChild(i).GetComponent<Renderer>().material.mainTexture = redTexture;
            }
            
        }
        else
        {
            for (int i = 0; i < redLine.transform.childCount; i++)
            {
                redLine.transform.GetChild(i).GetComponent<Renderer>().material.mainTexture = blueTexture;
            }
        }

        fireTruck.transform.position = new Vector3(SpawnScript.instance.allLanes[currentLane].position.x, fireTruck.transform.position.y, fireTruck.transform.position.z);
        policeCar.transform.position = new Vector3(SpawnScript.instance.allLanes[currentLane].position.x, policeCar.transform.position.y, policeCar.transform.position.z);
        redLine.transform.position = new Vector3(SpawnScript.instance.allLanes[currentLane].position.x, redLine.transform.position.y, redLine.transform.position.z);

        /*
        if (currentLane == 0)
        {
            fireTruck.transform.position = new Vector3(-8, fireTruck.transform.position.y, fireTruck.transform.position.z);
            policeCar.transform.position = new Vector3(-8, policeCar.transform.position.y, policeCar.transform.position.z);
            redLine.transform.position = new Vector3(-8, redLine.transform.position.y, redLine.transform.position.z);
        }
        else if (currentLane == 1)
        {
            fireTruck.transform.position = new Vector3(-4, fireTruck.transform.position.y, fireTruck.transform.position.z);
            policeCar.transform.position = new Vector3(-4, policeCar.transform.position.y, policeCar.transform.position.z);
            redLine.transform.position = new Vector3(-4, redLine.transform.position.y, redLine.transform.position.z);
        }
        else if (currentLane == 2)
        {
            fireTruck.transform.position = new Vector3(0, fireTruck.transform.position.y, fireTruck.transform.position.z);
            policeCar.transform.position = new Vector3(0, policeCar.transform.position.y, policeCar.transform.position.z);
            redLine.transform.position = new Vector3(0, redLine.transform.position.y, redLine.transform.position.z);
        }
        else if (currentLane == 3)
        {
            fireTruck.transform.position = new Vector3(4, fireTruck.transform.position.y, fireTruck.transform.position.z);
            policeCar.transform.position = new Vector3(4, policeCar.transform.position.y, policeCar.transform.position.z);
            redLine.transform.position = new Vector3(4, redLine.transform.position.y, redLine.transform.position.z);
        }
        else if (currentLane == 4)
        {
            fireTruck.transform.position = new Vector3(8, fireTruck.transform.position.y, fireTruck.transform.position.z);
            policeCar.transform.position = new Vector3(8, policeCar.transform.position.y, policeCar.transform.position.z);
            redLine.transform.position = new Vector3(8, redLine.transform.position.y, redLine.transform.position.z);
        }
        */
    }
	
    public void ResetTruck()
    {
        fireTruck.transform.position = new Vector3(0, this.transform.position.y, initialSpawnZ);
        policeCar.transform.position = new Vector3(0, this.transform.position.y, initialSpawnZ);

        GameObject.Find("GameController").GetComponent<SpawnScript>().timeElapsed = 0.0f;
        currentState = TruckStates.idle;
        spawnTime = Random.Range(minTime, maxTime);
        moveTruck = false;
        redLine.SetActive(false);
    }
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            currentState = TruckStates.initiateSequence;
            currentObstacle = 0;
        }

        if (SpawnScript.instance.allLanes.Count > 2)
        {
            if (Manager.Instance.currentGameState == Manager.GameStates.InGame || Manager.Instance.currentGameState == Manager.GameStates.GameOver)
            {

                blinkRed = (currentState == TruckStates.initiateSequence) || (currentState == TruckStates.startTruckMovement);
                moveTruck = (currentState == TruckStates.startTruckMovement) || (currentState == TruckStates.stopRedBlinking);

                frameCount++;
                if (blinkRed && frameCount % 3 == 0)
                {
                    redLine.gameObject.SetActive(!redLine.gameObject.activeSelf);
                    frameCount = 0;
                }

                if (blinkRed)
                {
                    if (currentObstacle == 0)
                        SoundManager.Instance.PlayAmbulanceSiren();
                    else if (currentObstacle == 1)
                        SoundManager.Instance.PlayPoliceSiren();
                }

                if (blinkRed)
                {
                    waitTime += Time.deltaTime;
                    if (waitTime > 2.5f)
                    {
                        currentState = TruckStates.startTruckMovement;
                        truckSpeed = 0.3f;
                        waitTime = 0;
                    }
                }

                if (moveTruck)
                {
                    if (currentObstacle == 0)
                    {
                        fireTruck.transform.Translate(0, 0, truckSpeed);
                    }
                    else
                    {
                        policeCar.transform.Translate(0, 0, truckSpeed);
                    }

                    if ((fireTruck.transform.position.z > 0.0f || policeCar.transform.position.z > 0.0f) && currentState == TruckStates.startTruckMovement)
                    {
                        currentState = TruckStates.stopRedBlinking;
                        truckSpeed = 0.3f;
                        redLine.gameObject.SetActive(false);
                    }
                }

                if ((fireTruck.transform.position.z > resetDistance || policeCar.transform.position.z > resetDistance))
                {
                    if (currentObstacle == 0)
                        fireTruck.transform.position = new Vector3(0, this.transform.position.y, initialSpawnZ);
                    else
                        policeCar.transform.position = new Vector3(0, this.transform.position.y, initialSpawnZ);

                    GameObject.Find("GameController").GetComponent<SpawnScript>().timeElapsed = 0.0f;
                    currentState = TruckStates.idle;
                    spawnTime = Random.Range(minTime, maxTime);
                }
            }
        }
    }
}
