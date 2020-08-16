    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float movementSpeed = 10;
    public float sprint;
    float horizontal;
    float vertical;
    Vector3 direction;

    private void Start()
    {
        sprint = movementSpeed * 2;
    }

    void Update()
    {
        
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        direction = vertical * transform.forward + horizontal * transform.right;


        if (horizontal != 0 || vertical != 0)
        {
            transform.position += direction * movementSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.E))
        {
            transform.position += transform.up * movementSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            transform.position += -transform.up * movementSpeed * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
            movementSpeed = sprint;

         if(Input.GetKeyUp(KeyCode.LeftShift))
            movementSpeed = sprint/2;

        

    }
}
