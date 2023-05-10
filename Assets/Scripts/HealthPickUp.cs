using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    public float healthAmount = 5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Rat"))
        {
            PlayerController.instantance.Heal(healthAmount);
            FindAnyObjectByType<AudioManager>().Play("PickUp");
            Destroy(gameObject);
        }
    }

}
