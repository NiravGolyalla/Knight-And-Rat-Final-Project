using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemyController : MonoBehaviour
{
    public float aggroRange = 10f;
    public float chaseSpeed = 3f;
    public float attackDistance = 1f;
    public float knockbackForce = 2f;
    public float damageInterval = 1f;
    public int health = 5;
    public float catnipRange = 5f;

    private Transform player;
    private Rigidbody2D rb;
    private Animator anim;
    private bool isChasing = false;
    private bool isAttacking = false;
    private bool isKnockbacked = false;
    private float damageTimer = 0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
            return;
        }

        if (isKnockbacked)
        {
            anim.SetBool("IsChasing", false);
            return;
        }

        if (IsCatnipInRange())
        {
            isChasing = false;
            anim.SetBool("IsChasing", false);
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= aggroRange)
        {
            isChasing = true;
        }

        if (isChasing && !isAttacking)
        {
            ChasePlayer();
        }

        if (distanceToPlayer <= attackDistance && !isAttacking)
        {
            StartCoroutine(Attack());
        }

        UpdateAnimation();
    }

    private void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * chaseSpeed;
    }

    private IEnumerator Attack()
    {
        isAttacking = true;
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(damageInterval);
        isAttacking = false;
    }

    public void TakeDamage()
    {
        health -= 1;
        StartCoroutine(Knockback());
    }

    private IEnumerator Knockback()
    {
        isKnockbacked = true;
        Vector2 knockbackDirection = (transform.position - player.position).normalized;
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.5f);
        rb.velocity = Vector2.zero;

        isKnockbacked = false;
    }

    private void UpdateAnimation()
    {
        anim.SetBool("IsChasing", isChasing && !isKnockbacked);
    }

    private bool IsCatnipInRange()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, catnipRange);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("catnip"))
            {
                return true;
            }
        }

        return false;
    }
}
