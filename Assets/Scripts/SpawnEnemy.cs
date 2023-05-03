using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject enemy;
    public void Spawn(){
        Instantiate(enemy, Vector3.zero, Quaternion.identity);
    }

    public void ChangeMode(){
        EnemyMovement.movementType = !EnemyMovement.movementType; 
    }

    void Update(){
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.I)){
            Spawn();
        }
    }
}
