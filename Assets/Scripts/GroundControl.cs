using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundControl : MonoBehaviour {

    float speed = 5.0f;

    public bool verticalOffset = true;
	
	// Update is called once per frame
	void Update ()
    {
        if (Time.timeScale == 0.1f)
            return;

        if (Manager.Instance.currentGameState == Manager.GameStates.InGame)
        {
            if (Manager.Instance.obstacleSpeed == Manager.Instance.slowSpeed)
            {
                speed = 3.0f;
            }
            else if (Manager.Instance.obstacleSpeed == Manager.Instance.fastSpeed)
            {
                speed = 20.0f;
            }
            else
            {
                if (verticalOffset)
                    speed = 10.0f;
                else
                    speed = 13.0f;
            }

            float offset = Time.time * speed;
            if (verticalOffset)
                this.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0, -offset);
            else
                this.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(offset, 0);
        }
        

    }
}
