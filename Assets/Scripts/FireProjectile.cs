using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    public int damage = 50; // Half of the player's health

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Knight") || collision.CompareTag("Rat"))
        {
            collision.GetComponent<PlayerController>().TakeDamage(damage);
        }
    }
}
