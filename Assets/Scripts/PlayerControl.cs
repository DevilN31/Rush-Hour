using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour {

    public List<Transform> allLanes;
    private float speed = 20.0f;

    public int currentLane = 0;
    private Vector3 destination = Vector3.zero;

    public Animator animator;

    Transform leftIndicator;
    Transform rightIndicator;
    Transform brakeLights;

    private Vector3 startPosition = Vector3.zero;
    private Vector3 endPosition = Vector3.zero;

    int moveDir = 0; //0 = idle , 1 = left , 2 = right , 3 = up , 4 = down

    int health = 0;
    CameraFollow cameraFollow;
    ParticleSystem windParticleSystem;
    ParticleSystem.EmissionModule em;

    GameObject healthCanvas;

    private bool isGameOver = false;

    //public GameObject redFlash;
    //GameObject spawnedRedFlash;

    bool isShaking = false;

    CarBehind carBehind;
    ParticleSystem accidentSmoke;
    ParticleSystem.EmissionModule accEm;

    Rigidbody RB;

    public float TorqueX, TorqueY, TorqueZ;

    public float ForceX, ForceY, ForceZ;

    void Start()
    {
        RB = GetComponent<Rigidbody>();
        RB.constraints = RigidbodyConstraints.FreezeAll;

        RB.maxAngularVelocity = 150f;

        leftIndicator = transform.Find("LeftIndicator");
        rightIndicator = transform.Find("RightIndicator");
        //brakeLights = transform.Find("BrakeLights");
        windParticleSystem = GameObject.Find("WindParticles").GetComponent<ParticleSystem>();
        em = windParticleSystem.emission;
        em.enabled = false;
        healthCanvas = GameObject.Find("HealthCanvas");

        if (Manager.FirstGame)
        {
            Manager.Instance.scoreText.gameObject.SetActive(false);
        }

        //spawnedRedFlash = Instantiate(redFlash);
        //spawnedRedFlash.SetActive(false);
        //spawnedRedFlash.transform.SetParent(transform);
        carBehind = GameObject.Find("Car Behind - New").GetComponent<CarBehind>() ;
        carBehind.Reset();

        accidentSmoke = transform.Find("AccidentSmoke").GetComponent<ParticleSystem>();
        accEm = accidentSmoke.emission;
        accEm.enabled = false;

        health = Manager.Instance.defaultHealth;

        StopIndicators();
        cameraFollow = Camera.main.GetComponent<CameraFollow>();
        cameraFollow.ResetCamera();
        
        allLanes = SpawnScript.instance.allLanes;
        currentLane = allLanes.Count / 2;
        transform.position = new Vector3(allLanes[currentLane].position.x,transform.position.y,transform.position.z);

    }

    void TakeHit()
    {
        health--;

        if (health >= 0)
        {
            accEm.enabled = true;
            //StartCoroutine(Flash());
            SoundManager.Instance.PlayCarHitMirror();
            healthCanvas.transform.GetChild(health).gameObject.SetActive(true);
        }

        if (Manager.Instance.obstacleSpeed == Manager.Instance.fastSpeed)
        {
            health = 0;
        }

        

        if (health == 0 && !isGameOver)
        {
            SoundManager.Instance.PlayCarFinalHit();
            Destroy(this.transform.Find("Platform").gameObject);
            RB.constraints = RigidbodyConstraints.None;
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.3f, this.transform.position.z);
            CollideLoseGame();
            StartCoroutine(GameOverDelay());

            ShowGameOver();
        }
    }

    private void CollideLoseGame()
    {
        Manager.Instance.currentGameState = Manager.GameStates.GameOver;
        gameObject.AddComponent<BoxCollider>();
        RB.constraints = RigidbodyConstraints.None;
        RB.useGravity = true;

        RB.AddForce(new Vector3(ForceX, ForceY, ForceZ) , ForceMode.Impulse);
        RB.AddTorque(new Vector3(TorqueX, TorqueY, TorqueZ), ForceMode.Impulse);
    }

    public void TakeHitByTruck()
    {
        health = 0;

        Transform platform = this.transform.Find("Platform");

        if (platform != null)
        {
            SoundManager.Instance.PlayCarFinalHit();
            Destroy(platform.gameObject);
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.3f, this.transform.position.z);
            CollideLoseGame();
            StartCoroutine(GameOverDelay());
        }
        
    }

    IEnumerator GameOverDelay()
    {
        yield return new WaitForSeconds(2.0f);
        
        ShowGameOver();
        yield return new WaitForSeconds(0.5f);

        Destroy(gameObject);

    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Obstacle" && !other.gameObject.transform.GetComponent<ObstacleScript>().hasCollided == true)
        {
            TakeHit();
            other.gameObject.transform.GetComponent<ObstacleScript>().hasCollided = true;
            other.gameObject.transform.GetComponent<Rigidbody>().useGravity = true;
        }

    }

    //IEnumerator Flash()
    //{
    //    if (!isShaking)
    //    {
    //        isShaking = true;
    //        StartCoroutine(StopShake());
    //    }
    //    //spawnedRedFlash.SetActive(true);
    //    //yield return new WaitForSeconds(0.1f);
    //    //spawnedRedFlash.SetActive(false);
        
    //}

    void FixedUpdate()
    {
        if (isShaking)
        {
            healthCanvas.transform.GetChild(2).GetComponent<RectTransform>().anchoredPosition3D = new Vector3(Mathf.Sin(Time.time * 50) * 5, 0, 0);
            healthCanvas.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition3D = new Vector3(Mathf.Sin(Time.time * 50) * 5, 0, 0);
            healthCanvas.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition3D = new Vector3(Mathf.Sin(Time.time * 50) * 5, 0, 0);
        }
    }

    IEnumerator StopShake()
    {
        yield return new WaitForSeconds(0.5f);
        isShaking = false;
    }
    void ShowGameOver()
    {
        
        //Manager.Instance.ShowInterstitial();
        healthCanvas.transform.GetChild(0).gameObject.SetActive(false);
        healthCanvas.transform.GetChild(1).gameObject.SetActive(false);
        healthCanvas.transform.GetChild(2).gameObject.SetActive(false);
        health = Manager.Instance.defaultHealth;
        startPosition = Vector3.zero;
        endPosition = Vector3.zero;
        cameraFollow.currentState =  CameraFollow.CameraStates.idle;
        Manager.Instance.obstacleSpeed = Manager.Instance.defaultSpeed;
        Manager.Instance.buildingsSpeed = Manager.Instance.buildingDefaultSpeed;
        Manager.Instance.ShowGameOver();
        em.enabled = false;
        isGameOver = false;
        SoundManager.Instance.StopMusic();
    }


    void BlinkLeft()
    {
        leftIndicator.gameObject.SetActive(true);
        rightIndicator.gameObject.SetActive(false);
    }

    void BlinkRight()
    {
        rightIndicator.gameObject.SetActive(true);
        leftIndicator.gameObject.SetActive(false);
    }

    void StopIndicators()
    {
        rightIndicator.gameObject.SetActive(false);
        leftIndicator.gameObject.SetActive(false);
    }


    //void CheckBrakeLights()
    //{
    //    if (cameraFollow.currentState == CameraFollow.CameraStates.bringNear)
    //    {
    //        brakeLights.Find("LeftBrakePointLight/LeftBrake").gameObject.SetActive(true);
    //        brakeLights.Find("RightBrakePointLight/RightBrake").gameObject.SetActive(true);
    //    }
    //    else
    //    {
    //        brakeLights.Find("LeftBrakePointLight/LeftBrake").gameObject.SetActive(false);
    //        brakeLights.Find("RightBrakePointLight/RightBrake").gameObject.SetActive(false);

    //    }
    //}

    void CheckParticleSystem()
    {
        if (cameraFollow.currentState == CameraFollow.CameraStates.sendFar)
        {
            em.enabled = true;
        }
        else
        {
            em.enabled = false;
        }
    }

    void StartGame() ///// Changed by avishy - moved to levelProgress script
    {
        Manager.Instance.currentGameState = Manager.GameStates.InGame; //// ADDED BY AVISHY - 1.8.2020

        if (LevelProgress.Instance.MainMenuCanvas.activeInHierarchy) //// ADDED BY AVISHY - 1.8.2020
        {
            LevelProgress.Instance.MainMenuCanvas.SetActive(false); //// ADDED BY AVISHY - 1.8.2020

            LevelProgress.Instance.LevelProgressSlider.gameObject.SetActive(true); //// ADDED BY AVISHY - 1.8.2020

            LevelProgress.Instance.LevelProgressSlider.value = 0; //// ADDED BY AVISHY - 1.8.2020

            SoundManager.Instance.PlaySwipeUpDown();
        }

        //Manager.FirstGame = false;
        //Manager.Instance.scoreText.gameObject.SetActive(true);
        //SoundManager.Instance.PlaySwipeUpDown();
        //LevelProgress.Instance.StartNextLevel();
        //Manager.Instance.currentGameState = Manager.GameStates.InGame;
    }

    void Update()
    {
        if (Manager.Instance.currentGameState == Manager.GameStates.MainMenu && !Manager.Instance.inShop)
        {
            if ((Input.GetMouseButtonUp(0) && Input.mousePosition.y > 200 && Input.mousePosition.y < Screen.height - 200 ))
            {
                //LevelProgress.Instance.StartNextLevel(); //// ADDED BY AVISHY - 1.8.2020

                StartGame();
            }
        }

        if (Manager.Instance.currentGameState == Manager.GameStates.InGame)
        {
            CheckParticleSystem();
            //CheckBrakeLights();

            if (Input.GetKeyDown(KeyCode.S) || Manager.Instance.moveDir == Manager.SwipeStates.Down)
            {
                if (cameraFollow.currentState == CameraFollow.CameraStates.bringNear)
                {
                    StartCoroutine(carBehind.Delay());
                    TakeHit();
                }
                cameraFollow.currentState = CameraFollow.CameraStates.startBringNear;
                Manager.Instance.obstacleSpeed = Manager.Instance.slowSpeed;
                Manager.Instance.buildingsSpeed = Manager.Instance.buildingSlowSpeed;
                SoundManager.Instance.PlayBrakeSound();
                SoundManager.Instance.PlaySwipeUpDown();
                Manager.Instance.moveDir = Manager.SwipeStates.Idle;
            }
            else if ((Input.GetKeyDown(KeyCode.W) || Manager.Instance.moveDir == Manager.SwipeStates.Up) && Manager.Instance.obstacleSpeed != Manager.Instance.slowSpeed)
            {
                em.enabled = true;
                cameraFollow.currentState = CameraFollow.CameraStates.startSendFar;
                Manager.Instance.obstacleSpeed = Manager.Instance.fastSpeed;
                SoundManager.Instance.PlaySwipeUpDown();
                Manager.Instance.moveDir = Manager.SwipeStates.Idle;
            }

            else if (Input.GetKeyDown(KeyCode.A) || Manager.Instance.moveDir == Manager.SwipeStates.Left)
            {
                BlinkLeft();
                //go left
                currentLane--;
                if (currentLane >= 0)
                {
                    animator.SetTrigger("RotateLeft");
                }
                currentLane = Mathf.Clamp(currentLane, 0, 4);

                destination = allLanes[currentLane].position;
                destination = new Vector3(destination.x, transform.position.y, transform.position.z);
                Manager.Instance.moveDir = Manager.SwipeStates.Idle;
                SoundManager.Instance.PlaySwipeLeftRight();

            }
            else if (Input.GetKeyDown(KeyCode.D) || Manager.Instance.moveDir == Manager.SwipeStates.Right)
            {
                BlinkRight();
                //go right
                if(currentLane < SpawnScript.instance.allLanes.Count - 1)
                currentLane++;

                if (currentLane <= SpawnScript.instance.allLanes.Count-1)
                {
                    animator.SetTrigger("RotateRight");
                }
                currentLane = Mathf.Clamp(currentLane, 0, 4);

                destination = allLanes[currentLane].position;
                destination = new Vector3(destination.x, transform.position.y, transform.position.z);

                Manager.Instance.moveDir = Manager.SwipeStates.Idle;
                SoundManager.Instance.PlaySwipeLeftRight();
            }

            if (destination != Vector3.zero)
            {
                float step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, destination, step);

                if (transform.position.x == 0 || transform.position.x == 4 || transform.position.x == 8 || transform.position.x == -4 || transform.position.x == -8)
                    transform.rotation = Quaternion.Euler(Vector3.zero);

                float diff = Mathf.Abs(transform.position.x - destination.x);
                if (diff <= 0.05f)
                {
                    StopIndicators();
                }
            }
        }

        if(Manager.Instance.currentGameState == Manager.GameStates.GameOver)
        {
            transform.Translate(new Vector3(0, 0, 1.5f), Space.World);
        }
    }
}
