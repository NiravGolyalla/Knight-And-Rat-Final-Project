using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverController : MonoBehaviour
{
    bool on = false;
    private bool withinRange;
    public Sprite turnedOff;
    public Sprite turnedOn;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && withinRange)
        {
            if (on)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = turnedOn;
            } else
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = turnedOff;
            }
            on = !on;
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        // Checks if the player is in range to activate the lever
        // Only knights can activate levers
        if (other.tag == "Knight")
        {
            withinRange = true;
        }
       
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Knight")
        {
            withinRange = false;
        }
    }
}
