using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public bool exitFrame = false;
    void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Enemy"){
            other.gameObject.GetComponent<EnemyController>().takeDamage();
        }
        if(other.tag == "BARRELS"){
            other.GetComponent<CNBController>().GotHit();
        }
    }
}
