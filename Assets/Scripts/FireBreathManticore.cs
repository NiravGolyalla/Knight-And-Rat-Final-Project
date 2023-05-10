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
    public float fireProjectileInterval = 0.5f;
    public float fireProjectileSpeedIncreasePerHit = 0.15f;
    public float fireProjectileAngleRange = 150f;
    public int missingFireballsCount = 5;

    public GameObject bouncePoint;
    public int hits = 0;
    private CircleCollider2D hitboxCollider;
    private bool stopFireBreathAttack = false;
    public GameObject grid;

    public Color initialFireColor;
    public Color finalFireColor = Color.red;
    public float arenaRightMost = 10f;

    void Start()
    {
        manticore = GetComponent<Manticore>();
        hitboxCollider = GetComponent<CircleCollider2D>();
        initialFireColor = fireProjectilePrefab.GetComponent<SpriteRenderer>().color;
    }

    void Update()
    {
        if (manticore.health <= 60 && !stage2Started)
        {
            manticore.stage = 2;
            stage2Started = true;
            hits = 0;
            grid = GameObject.Find("Grid");

            Transform bridgeTransform = grid.transform.Find("Door");
            bridgeTransform.gameObject.SetActive(false);
            StartCoroutine(Stage2Sequence());
        }
    }

    IEnumerator Stage2Sequence()
    {
        bool initiallyFacingRight = manticore.IsFacingRight();
        hits = 0;

        // If initially facing right.
        if (initiallyFacingRight)
        {
            animator.SetTrigger("Tired");
            yield return new WaitForSeconds(4f);

            manticore.Flip();
            animator.SetBool("Running", true);
            yield return StartCoroutine(RunTowardsTarget());
            animator.SetBool("Running", false);
        }
        // If initially not facing right.
        else
        {
            manticore.Flip();

            animator.SetTrigger("Tired");
            yield return new WaitForSeconds(4f);

            animator.SetBool("Running", true);
            yield return StartCoroutine(RunTowardsTarget());
            animator.SetBool("Running", false);
        }
        manticore.Flip();
        animator.SetTrigger("FireBreath");

        while (manticore.health > 0)
        {
            yield return StartCoroutine(FireBreathAttack());
        }
    }


    IEnumerator FireBreathAttack()
    {
        hitboxCollider.enabled = true;
        float startAngle = -90f;
        float angleStep = fireProjectileAngleRange / (fireProjectileCount - 1);
        int missingFireballsStartIndex = Random.Range(0, 11) + 20;


        for (int j = 0; j < fireProjectileCount; j++)
        {
            if (j < missingFireballsStartIndex || j >= missingFireballsStartIndex + missingFireballsCount)
            {
                float angle = startAngle + angleStep * j;
                SpawnFireProjectile(angle);
            }
        }

        yield return new WaitForSeconds(fireProjectileInterval - (hits * fireProjectileSpeedIncreasePerHit));
        hitboxCollider.enabled = false;
    }

    void SpawnFireProjectile(float angle)
    {
        float radians = angle * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
        GameObject fireProjectile = Instantiate(fireProjectilePrefab, firePoint.transform.position, Quaternion.identity);
        fireProjectile.GetComponent<Rigidbody2D>().velocity = direction * (runSpeed + (hits * fireProjectileSpeedIncreasePerHit));

        // Change fireball color
        fireProjectile.GetComponent<SpriteRenderer>().color = Color.Lerp(initialFireColor, finalFireColor, (float)hits / 10);

        Destroy(fireProjectile, 5f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Sword") && hitboxCollider.enabled && stage2Started && animator.GetCurrentAnimatorStateInfo(0).IsName("FireBreath"))
        {
            Debug.Log("Sword hit Manticore");
            manticore.TakeDamage2(20);

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
                StartCoroutine(KnockbackPlayer(playerContainer));
            }
            hits++;

            if (manticore.health <= 0)
            {
                StartCoroutine(DestroyAfterAnimation());
            }
        }
    }
    IEnumerator DestroyAfterAnimation()
    {      
        Camera.main.orthographicSize = 10f;  
        animator.SetTrigger("Death");
        yield return new WaitForSeconds(2f);    
        Destroy(gameObject);    
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

    IEnumerator RunTowardsTarget()
    {
        yield return new WaitForSeconds(0.1f);

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        float t = 0f;
        Vector3 startPosition = transform.position;
        float distance = Vector2.Distance(startPosition, targetObject.transform.position);

        while (Vector2.Distance(transform.position, targetObject.transform.position) > 0.1f)
        {
            if (stateInfo.IsName("ManticoreRun"))
            {
                t += Time.deltaTime * runSpeed / distance;
                transform.position = Vector2.Lerp(startPosition, targetObject.transform.position, t);
            }
            else
            {
                stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            }

            yield return null;
        }
    }
}
