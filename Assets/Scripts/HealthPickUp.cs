using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    public float healthAmount = 5f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollision2DEnter(Collision2D other) {
        if (other.gameObject.CompareTag("Knight") || other.gameObject.CompareTag("Rat")) {
            other.gameObject.GetComponent<PlayerController>().Heal(healthAmount);
            Destroy(gameObject);
        }
    }

}
