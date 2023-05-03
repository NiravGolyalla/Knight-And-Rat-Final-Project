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

    public bool isActivated() { return activate; }
     private void OnTriggerStay2D(Collider2D other) {
        // Handles the case if the knight transforms into a rat
        // Pressure plate can not be activated by a rat
        // print(other.tag);
        if (other.tag == "Knight")
        {
            activate = true;
        }
        else if(other.tag == "Barrel")
        {
            activate = true;
            other.GetComponent<BarrelController>().solved = true;
        }
        //activate = (other.tag == "Knight" || other.tag == "Barrel") ? true: false;
        else {activate = false;}
        // print(activate);
        if (activate) {
            gameObject.GetComponent<SpriteRenderer>().sprite = pressurePlateOn;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        activate = false;
        gameObject.GetComponent<SpriteRenderer>().sprite = pressurePlateOff;
        if(other.tag == "Barrel"){
            other.GetComponent<BarrelController>().solved = false;
        }
    }
}
