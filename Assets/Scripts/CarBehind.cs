using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBehind : MonoBehaviour {

    bool bringNear = false;
    public GameObject Car;

    private void Start()
    {
        Car = GameObject.FindGameObjectWithTag("Player");
    }
    public void Reset()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, -35.0f);
        bringNear = false;
        Car = GameObject.FindGameObjectWithTag("Player");
    }
    public IEnumerator Delay()
    {
        bringNear = true;
        transform.position = new Vector3(Car.transform.position.x, transform.position.y, transform.position.z);
        yield return new WaitForSeconds(2);
        bringNear = false;
    }


    void Update()
    {
        if (Manager.Instance.currentGameState == Manager.GameStates.InGame)
        {
            if (bringNear)
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y, Car.transform.position.z - 6.5f), Time.deltaTime * 20);
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y, -35.0f), Time.deltaTime * 5);
            }
        }
        
    }
}
