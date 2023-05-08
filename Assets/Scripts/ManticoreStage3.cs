using System.Collections;
using UnityEngine;

public class ManticoreStage3 : MonoBehaviour
{
    public Animator animator;
    public float maxHealth = 100f;
    public GameObject player;
    public float chaseSpeed = 5f;
    public float attackRange = 2f;
    public float catnipDetectionRange = 5f;
    public LayerMask catnipLayer;
    public LayerMask playerLayer;
    private bool isChasing = false;
    private bool isStunned = false;
    public float damage = 10f;

    private float currentHealth;
    private bool isFacingRight;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (!isStunned && currentHealth > 0)
        {
            ChasePlayer();
            CheckForCatnip();
        }
    }

    void ChasePlayer()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer > attackRange)
        {
            isChasing = true;
            animator.SetBool("Running", true);
            Vector2 direction = (player.transform.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, chaseSpeed * Time.deltaTime);

            if (direction.x > 0 && !IsFacingRight() || direction.x < 0 && IsFacingRight())
            {
                Flip();
            }
        }
        else if (isChasing)
        {
            isChasing = false;
            animator.SetBool("Running", false);
            animator.SetTrigger("RunningAttack");

            if (IsPlayerInRange())
            {
                Attack();
            }
        }
    }

    void CheckForCatnip()
    {
        Collider2D catnipCollider = Physics2D.OverlapCircle(transform.position, catnipDetectionRange, catnipLayer);

        if (catnipCollider != null)
        {
            isChasing = false;
            StartCoroutine(StunManticore());
        }
    }

    IEnumerator StunManticore()
    {
        isStunned = true;
        animator.SetBool("Running", false);
        animator.SetTrigger("RunningToIdle");
        yield return new WaitForSeconds(1f);
        isStunned = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Sword"))
        {
            TakeDamage(damage);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            StartCoroutine(Death());
        }
    }

    private IEnumerator Death()
    {
        animator.SetTrigger("Die");

        yield return new WaitForSeconds(1f);

        Destroy(gameObject);
    }

    public bool IsFacingRight()
    {
        return isFacingRight;
    }

    public void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    public bool IsPlayerInRange()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, attackRange, playerLayer);
        return playerCollider != null;
    }

    public void Attack()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, attackRange, playerLayer);

        if (playerCollider != null)
        {
            GameObject target = playerCollider.gameObject;
            if (target.CompareTag("Knight") || target.CompareTag("Rat"))
            {
                PlayerController playerController = target.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.TakeDamage(damage);
                }
            }
        }
    }

}
