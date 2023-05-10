using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateController : MonoBehaviour
{

    public bool activate;
    int count;

    public Sprite pressurePlateOn;
    public Sprite pressurePlateOff;

    void Update(){
        gameObject.GetComponent<SpriteRenderer>().sprite = (activate) ? pressurePlateOn:pressurePlateOff;
    }

    public bool isActivated() { return activate; }
    private void OnTriggerStay2D(Collider2D other)
    {
        // Handles the case if the knight transforms into a rat
        // Pressure plate can not be activated by a rat
        // print(other.tag);
        if (other.tag == "Knight")
        {    
            activate = true;
        }
        else if (other.tag == "Barrel"){
            activate = true;
            other.GetComponent<BarrelController>().solved = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Knight" || other.tag == "Barrel")
        {
            activate = false;
        }
        if (other.tag == "Barrel")
        {
            other.GetComponent<BarrelController>().solved = false;
        }
    }
}
