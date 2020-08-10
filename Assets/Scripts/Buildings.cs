using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buildings : MonoBehaviour {

    Rigidbody rb;
    Vector3 buildingSpeed;
    // Use this for initialization

    public int DistanceToDestroy;
	void Start ()
    {
        buildingSpeed = Manager.Instance.buildingDefaultSpeed;
	}

    // Update is called once per frame
    void Update()
    {

        if (Manager.Instance.currentGameState == Manager.GameStates.InGame)
        {
            if (buildingSpeed.z != Manager.Instance.buildingsSpeed.z)
            {
                buildingSpeed = Manager.Instance.buildingsSpeed;
            }

            transform.Translate(new Vector3(0, 0, buildingSpeed.z), Space.World);

            if (transform.position.z < DistanceToDestroy)
            {
                Destroy(transform.gameObject);
            }
        }
        
	}
}
