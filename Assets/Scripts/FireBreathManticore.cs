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
    private bool stopFireBreathAttack = false;
    // public Tilemap bridgeTilemap;
    public GameObject grid;

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
            grid = GameObject.Find("Grid");

            Transform bridgeTransform = grid.transform.Find("Door");

            // bridgeTilemap = bridgeTransform.GetComponent<Tilemap>();
            bridgeTransform.gameObject.SetActive(false);
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
        stopFireBreathAttack = false;
        while (hits < 2 && !stopFireBreathAttack)
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

        int missingFireballsStartIndex = Random.Range(0, fireProjectileCount - 8);

        for (int j = 0; j < fireProjectileCount; j++)
        {
            if (j < missingFireballsStartIndex || j >= missingFireballsStartIndex + 8)
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

            // Find the parent GameObject with the "Knight" or "Rat" tag
            Transform currentParent = collision.transform.parent;
            GameObject playerContainer = null;

            while (currentParent != null)
            {
                if (currentParent.CompareTag("Knight") || currentParent.CompareTag("Rat"))
                {
                    playerContainer = currentParent.parent.gameObject;
                    break;
                }
                currentParent = currentParent.parent;
            }

            if (playerContainer != null)
            {
                // Start knockback coroutine for the player container
                StartCoroutine(KnockbackPlayer(playerContainer));
            }
            hits++;

            if (hits >= 2)
            {
        
                StartCoroutine(DestroyAfterAnimation());
            }

        }
    }

    IEnumerator KnockbackPlayer(GameObject player)
    {
        Vector3 originalPosition = player.transform.position;
        Vector3 peakPosition = originalPosition + (bouncePoint.transform.position - originalPosition) / 2 + Vector3.up * 2f; // Adjust the "2f" value to control the height of the bounce
        Vector3 finalPosition = bouncePoint.transform.position;

        float knockbackTime = 1f; // Increase the knockback time for a more subtle effect
        float elapsedTime = 0f;
        float t;

        while (elapsedTime < knockbackTime)
        {
            t = elapsedTime / knockbackTime;
            player.transform.position = CalculateBezierPoint(t, originalPosition, peakPosition, finalPosition);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        player.transform.position = finalPosition;
    }

    Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 B = uu * p0 + 2 * u * t * p1 + tt * p2;
        return B;
    }
    IEnumerator DestroyAfterAnimation()
    {
        animator.SetTrigger("Death");
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
        LevelManager.instance.LoadLevel("Start Menu");
    }

}
