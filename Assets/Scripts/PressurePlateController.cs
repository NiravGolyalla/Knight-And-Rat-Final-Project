using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateController : MonoBehaviour
{
    
    bool activate;

    public Sprite pressurePlateOn;
    public Sprite pressurePlateOff;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
     private void OnTriggerStay2D(Collider2D other) {
        // Handles the case if the knight transforms into a rat
        // Pressure plate can not be activated by a rat
        activate = other.tag == "Knight" ? true: false;
        gameObject.GetComponent<SpriteRenderer>().sprite = pressurePlateOn;
    }

    private void OnTriggerExit2D(Collider2D other) {
        activate = false;
        gameObject.GetComponent<SpriteRenderer>().sprite = pressurePlateOff;
    }
}
