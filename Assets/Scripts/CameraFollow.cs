using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    Transform target;
    Vector3 originalPosition;

    public enum CameraStates
    {
        idle,
        startBringNear,
        bringNear,
        startSendFar,
        sendFar
    }

    public CameraStates currentState = CameraStates.idle;

    private bool isCoroutineRunning = false;

    void Awake()
    {
        originalPosition = transform.position;
        Debug.Log(originalPosition);
    }

	void Update ()
    {
        if (Manager.Instance.currentGameState == Manager.GameStates.InGame)
        {
            //if (target == null)
            //{
            //    target = GameObject.FindGameObjectWithTag("Player").transform;
            //}

            //Camera x movement follow target
            if (target)
            {
                float step = 15 * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, transform.position.y, transform.position.z), step);
            }


            if (currentState == CameraStates.idle)
            {
                //bring camera back to original position once speed is back to normal

                transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, originalPosition.y, originalPosition.z), Time.deltaTime * 2);
                Manager.Instance.obstacleSpeed = Manager.Instance.defaultSpeed;
                Manager.Instance.buildingsSpeed = Manager.Instance.buildingDefaultSpeed;
            }
            else if (currentState == CameraStates.startBringNear) //slow down car camera stuff
            {
                if (!isCoroutineRunning)
                {
                    currentState = CameraStates.bringNear;
                    StartCoroutine(CameraDelay());
                }
                else
                {
                    currentState = CameraStates.bringNear;
                }
            }
            else if (currentState == CameraStates.bringNear)
            {
                Vector3 destination = new Vector3(0, 4.74f, -20.53f);
                transform.position = Vector3.Lerp(transform.position, new Vector3(target.position.x, destination.y, destination.z), Time.deltaTime * 5);
            }
            else if (currentState == CameraStates.startSendFar && !isCoroutineRunning) //speed up car camera stuff
            {
                currentState = CameraStates.sendFar;
                StartCoroutine(CameraDelay());
            }
            else if (currentState == CameraStates.sendFar)
            {
                Vector3 destination = new Vector3(0, 4.74f, 0.0f);
                transform.position = Vector3.Lerp(transform.position, new Vector3(target.position.x, destination.y, transform.position.z), Time.deltaTime * 5);
            }
        }
		
    }

    public void ResetCamera()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        if (target != null)
        {
            currentState = CameraStates.idle;
            isCoroutineRunning = false;
            transform.position = originalPosition;
        }
    }

    IEnumerator CameraDelay()
    {
        isCoroutineRunning = true;
        yield return new WaitForSeconds(0.7f);
        if (isCoroutineRunning)
        {
            if (currentState == CameraStates.bringNear)
            {
                SoundManager.Instance.canPlayBrakeSound = true;
                currentState = CameraStates.idle;
                isCoroutineRunning = false;
            }
            else if (currentState == CameraStates.startBringNear)
            {
                isCoroutineRunning = false;
            }
            else if (currentState == CameraStates.sendFar)
            {
                currentState = CameraStates.idle;
                isCoroutineRunning = false;
            }
            else if (currentState == CameraStates.startSendFar)
            {
                isCoroutineRunning = false;
            }
        }
        
    }
}
