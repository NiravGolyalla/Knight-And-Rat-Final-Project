using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Enemy"){
            other.gameObject.GetComponent<EnemyController>().takeDamage();
        }
    }
}
