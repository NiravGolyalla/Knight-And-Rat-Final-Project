using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBreathManticore : MonoBehaviour
{
    public Animator animator;
    public Manticore manticore;
    private bool stage2Started = false;
    public GameObject targetObject;
    public float runSpeed = 10f;

    public GameObject firePoint;
    public GameObject fireProjectilePrefab;
    public int fireProjectileCount = 20;
    public float fireProjectileInterval = 0.05f;
    public float fireProjectileAngleRange = 90f;

    public GameObject bouncePoint;
    public int hits = 0;
    private CircleCollider2D hitboxCollider;

    void Start()
    {
        manticore = GetComponent<Manticore>();
        hitboxCollider = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        if (manticore.health <= 150 && !stage2Started)
        {
            manticore.stage = 2;
            stage2Started = true;
            hits = 0;
            StartCoroutine(Stage2Sequence());
        }
    }

    IEnumerator Stage2Sequence()
    {
        // Check if Manticore is initially facing right and store this information
        bool initiallyFacingRight = manticore.IsFacingRight();
        hits = 0;

        // If Manticore is initially facing left, flip the sprite before the tired animation
        if (!initiallyFacingRight)
        {
            manticore.Flip();
        }

        animator.SetTrigger("Tired");

        yield return new WaitForSeconds(4f);

        // If Manticore is initially facing right, flip the sprite before the run animation
        if (initiallyFacingRight)
        {
            manticore.Flip();
        }

        animator.SetBool("Running", true);
        yield return StartCoroutine(RunTowardsTarget());
        animator.SetBool("Running", false);

        if (initiallyFacingRight)
        {
            manticore.Flip();
        }
        hits = 0;
        if (!initiallyFacingRight)
        {
                manticore.Flip();
        }
        StartCoroutine(RepeatedFireBreathAttack());
        animator.SetTrigger("Tired");
        hitboxCollider.enabled = false;
        yield return new WaitForSeconds(4f);
    }
    IEnumerator RepeatedFireBreathAttack()
    {
        while (hits < 2)
        { 
            animator.SetTrigger("FireBreath");
            yield return StartCoroutine(FireBreathAttack());
            yield return new WaitForSeconds(0.25f);
        }
    }
    IEnumerator RunTowardsTarget()
    {
        // Wait for the running animation to start
        yield return new WaitForSeconds(0.1f);

        // Get the animator state info
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        float t = 0f;
        Vector3 startPosition = transform.position;
        float distance = Vector2.Distance(startPosition, targetObject.transform.position);

        while (Vector2.Distance(transform.position, targetObject.transform.position) > 0.1f)
        {
            // Check if the running animation is playing
            if (stateInfo.IsName("ManticoreRun"))
            {
                t += Time.deltaTime * runSpeed / distance;
                transform.position = Vector2.Lerp(startPosition, targetObject.transform.position, t);
            }
            else
            {
                // Update the animator state info
                stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            }

            yield return null;
        }
    }
    IEnumerator FireBreathAttack()
    {
        // Turn on the hitbox collider
        hitboxCollider.enabled = true;
        
        // Spawn fire projectiles
        float startAngle = -45f;
        float angleStep = fireProjectileAngleRange / (fireProjectileCount - 1);

        int missingFireballsStartIndex = Random.Range(0, fireProjectileCount - 5);

        for (int j = 0; j < fireProjectileCount; j++)
        {
            if (j < missingFireballsStartIndex || j >= missingFireballsStartIndex + 5)
            {
                float angle = startAngle + angleStep * j;
                SpawnFireProjectile(angle);
            }
            yield return new WaitForSeconds(fireProjectileInterval);
        }

        // Wait for some time before turning off the collider
        yield return new WaitForSeconds(0.55f);
        
        // Turn off the hitbox collider
         hitboxCollider.enabled = false;
        
    }

    void SpawnFireProjectile(float angle)
    {
        float radians = angle * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
        GameObject fireProjectile = Instantiate(fireProjectilePrefab, firePoint.transform.position, Quaternion.identity);
        fireProjectile.GetComponent<Rigidbody2D>().velocity = direction * runSpeed;
        Destroy(fireProjectile, 5f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Sword") && hitboxCollider.enabled && stage2Started)
        {
            Debug.Log("Sword hit Manticore");
            // Take damage from the sword
            manticore.TakeDamage2(20);
            
            // Start knockback coroutine for the player
            StartCoroutine(KnockbackPlayer(collision.gameObject));
        }
    }

    IEnumerator KnockbackPlayer(GameObject player)
    {
        Vector3 originalPosition = player.transform.position;
        float knockbackTime = 0.25f;
        float elapsedTime = 0f;

        while (elapsedTime < knockbackTime)
        {
            player.transform.position = Vector3.Lerp(originalPosition, bouncePoint.transform.position, elapsedTime / knockbackTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        player.transform.position = bouncePoint.transform.position;
    }

}
