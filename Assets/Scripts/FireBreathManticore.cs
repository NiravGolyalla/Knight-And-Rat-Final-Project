using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBreathManticore : MonoBehaviour
{
    public Animator animator;
    private Manticore manticore;
    private bool stage2Started = false;
    public GameObject targetObject;
    public float runSpeed = 10f;

    public GameObject firePoint;
    public GameObject fireProjectilePrefab;
    public int fireProjectileCount = 20;
    public float fireProjectileInterval = 0.05f;
    public float fireProjectileAngleRange = 90f;

    void Start()
    {
        manticore = GetComponent<Manticore>();
    }

    void Update()
    {
        if (manticore.health <= 60 && !stage2Started)
        {
            manticore.stage = 2;
            stage2Started = true;
            StartCoroutine(Stage2Sequence());
        }
    }

    IEnumerator Stage2Sequence()
    {
        // Check if Manticore is initially facing right and store this information
        bool initiallyFacingRight = manticore.IsFacingRight();

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

        if (initiallyFacingRight)
        {
            manticore.Flip();
        }
        // Ensure Manticore is facing right before the fire breath attack
        animator.SetTrigger("FireBreath");
        yield return StartCoroutine(FireBreathAttack());
        yield return new WaitForSeconds(3f); // Manticore tired for 3 seconds
        animator.SetTrigger("Tired");
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
        
        for (int i = 0; i < 2; i++) // Two instances of the fire breath attack
        {
            float startAngle = (i == 0) ? -45f : 45f;
            float angleStep = fireProjectileAngleRange / (fireProjectileCount - 1);

            for (int j = 0; j < fireProjectileCount; j++)
            {
                float angle = startAngle + angleStep * j;
                SpawnFireProjectile(angle);
                yield return new WaitForSeconds(fireProjectileInterval);
            }

            yield return new WaitForSeconds(2.75f); // Time between fire breath attacks
        }
    }

    void SpawnFireProjectile(float angle)
    {
        float radians = angle * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
        GameObject fireProjectile = Instantiate(fireProjectilePrefab, firePoint.transform.position, Quaternion.identity);
        fireProjectile.GetComponent<Rigidbody2D>().velocity = direction * runSpeed;
        Destroy(fireProjectile, 5f);
    }

}