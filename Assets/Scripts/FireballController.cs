using UnityEngine;

public class FireballController : MonoBehaviour
{
    public float damage = 2f;
    public float lifeTime = 5f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Rat") || collision.gameObject.CompareTag("Knight"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                ApplyDamage(playerController, damage);
            }
            Destroy(gameObject);
        }
    }

    private void ApplyDamage(PlayerController playerController, float damageAmount)
    {
        StartCoroutine(playerController.TakeDamage(damageAmount));
    }
}
