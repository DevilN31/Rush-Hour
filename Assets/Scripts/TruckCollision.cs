using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckCollision : MonoBehaviour {

    public ObstacleTruck obstacleTruck;
    void OnCollisionEnter(Collision item)
    {
        if ((obstacleTruck.currentState == ObstacleTruck.TruckStates.startTruckMovement || obstacleTruck.currentState == ObstacleTruck.TruckStates.stopRedBlinking))
        {
            if (item.gameObject.tag == "Obstacle")
            {
                item.gameObject.transform.GetComponent<Rigidbody>().useGravity = true;
                Destroy(item.gameObject, 2);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerControl>().TakeHitByTruck();
        }
    }
}
