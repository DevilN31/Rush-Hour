using UnityEngine;
using System.Collections;

public class ObstacleScript : MonoBehaviour
{
    public bool hasCollided = false;

    public Vector3 carVelocity = new Vector3(0, 0, -50);

    private Rigidbody rb;
    public int canChangeLane = 0; // 0 = no , 1 = left , 2 = right
    float destXPos = 0;
    bool changingLane = false;
    float zPosLaneChange = 0;
    float changeLaneSpeed = 0;

    public GameObject indicator;

    void Start()
    {
        carVelocity = Manager.Instance.obstacleSpeed;

        rb = transform.GetComponent<Rigidbody>();
        rb.velocity = carVelocity;

        if (canChangeLane == 1)
            destXPos = transform.localPosition.x - 4;
        else if (canChangeLane == 2)
            destXPos = transform.localPosition.x + 4;

        if (canChangeLane != 0)
        {
            zPosLaneChange = Random.Range(10, 60);
            if (canChangeLane == 1)
                changeLaneSpeed = -0.05f;
            else
                changeLaneSpeed = 0.05f;

            InvokeRepeating("BlinkIndicator", 0, 0.2f);

        }
    }

    void BlinkIndicator()
    {
        indicator.SetActive(!indicator.activeSelf);
    }

    void Update()
    {
        if (Manager.Instance.currentGameState == Manager.GameStates.GameOver )
        {
            Manager.Instance.obstacleSpeed = Manager.Instance.gameOverSpeed;
        }
        

        if (carVelocity != Manager.Instance.obstacleSpeed)
        {
            carVelocity = Manager.Instance.obstacleSpeed ;
            rb.velocity = carVelocity;
        }
        
       

        if (this.transform.localPosition.z < -zPosLaneChange && canChangeLane != 0 && changingLane == false)
        {
            changingLane = true;
            SoundManager.Instance.PlayHorn();
        }

        if (changingLane)
        {
            this.transform.Translate(changeLaneSpeed, 0, 0);
            if (Mathf.Abs(this.transform.localPosition.x - destXPos) < 0.01f)
            {
                canChangeLane = 0;
                changingLane = false;
                CancelInvoke("BlinkIndicator");
                indicator.SetActive(false);
            }
        }

        if (this.transform.localPosition.z < -130.0f || transform.localPosition.y > 30.0f || transform.localPosition.x > 14.0f || transform.localPosition.x < -14.0f)
        {
            Destroy(this.gameObject);
        }        
    }


    void OnCollisionEnter(Collision other)
    {
        //Debug.Log("Collision : " + other.gameObject.name);
        if (other.gameObject.tag == "Obstacle")
        {
            other.gameObject.transform.GetComponent<Rigidbody>().useGravity = true;
            Destroy(this.gameObject,2f);
        }

        if (other.gameObject.tag == "Player")
        {
            other.gameObject.transform.GetComponent<Rigidbody>().useGravity = true;
            Destroy(this.gameObject, 2f);
        }
    }

}