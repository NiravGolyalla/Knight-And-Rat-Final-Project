using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballController : MonoBehaviour
{
    public float damage = 2f;
    public float lifeTime = 5f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Rat") || collider.gameObject.CompareTag("Knight"))
        {
            PlayerController playerController = collider.gameObject.GetComponentInParent<PlayerController>();
            if (playerController != null)
            {
                StartCoroutine(ApplyDamage(playerController, damage));
            }
            Destroy(gameObject);
        }
    }

    private IEnumerator ApplyDamage(PlayerController playerController, float damageAmount)
    {
            playerController.takingDamage = true;
            float take = PlayerController.isKnightController ? playerController.dmgTakeK * damageAmount : playerController.dmgTakeR * damageAmount;
            playerController.healthBar.setValue(playerController.healthBar.getValue() - take);
            playerController.health -= take;
            playerController.UpdateHealth(damage); // Call the UpdateHealth() method on the player controller to update the UI
            yield return new WaitForSeconds(0.5f);
            playerController.takingDamage = false;
        
    }
}
