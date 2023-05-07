using UnityEngine;
using System.Collections;

public class Manticore : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float chaseDistance = 3f;
    public float attackDistance = 1f;
    public float minDistanceFromPlayer = 2f;
    public float maxDistanceFromPlayer = 5f;
    public Animator animator;
    public GameObject fireballPrefab;
    public Transform firePoint;
    public float fireballSpeed = 10f;

    public GameObject cutsceneTrigger;

    private Transform target;
    private bool isFacingRight = true;
    private float fireballTimer = 0f;
    private float fireballChance = 0.3f;
    private bool isCutsceneActive = true;

    public float catnipDetectionRange = 10f;
    public float catnipChaseSpeedMultiplier = 3f;
    public float catnipConsumptionDuration = 1f;
    public float descendDuration = 1.5f;
    public float ascendDuration = 0.3f;
    public int health = 120;
    public int damagePerHit = 20;

    private bool isChasingCatnip = false;
    private bool isConsumingCatnip = false;
    private GameObject detectedCatnip = null;
    private CircleCollider2D hitboxCollider;

    public float invincibilityDuration = 0.2f;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private bool hasDescended = false;


    void Start()
    {
        UpdateTarget();
        hitboxCollider = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;

    }

    void Update()
    {
        if (isCutsceneActive)
        {
            animator.SetBool("isFlying", true);
        }
        else
        {
            UpdateTarget();
            DetectCatnip();

            if (detectedCatnip != null && !isConsumingCatnip)
            {
                StartCoroutine(ChaseCatnip(detectedCatnip));
            }

            if (target != null && !isChasingCatnip && !isConsumingCatnip)
            {
                animator.SetBool("isFlying", false);
                float distanceToTarget = Vector2.Distance(transform.position, target.position);

                if (distanceToTarget <= chaseDistance)
                {
                    ChasePlayer();
                }

                fireballTimer += Time.deltaTime;

                if (fireballTimer >= 1f)
                {
                    fireballTimer = 0f;
                    StartCoroutine(ShootFireballs());
                }
            }
        }
    }

    void DetectCatnip()
    {
        if (detectedCatnip == null)
        {
            GameObject[] catnips = GameObject.FindGameObjectsWithTag("catnip");
            float closestDistance = Mathf.Infinity;

            foreach (GameObject catnip in catnips)
            {
                float distance = Vector2.Distance(transform.position, catnip.transform.position);
                if (distance < catnipDetectionRange && distance < closestDistance)
                {
                    closestDistance = distance;
                    detectedCatnip = catnip;
                }
            }
            hasDescended = false;
        }
        else
        {
            CatnipController catnipController = detectedCatnip.GetComponent<CatnipController>();
            if (catnipController.done)
            {
                detectedCatnip = null;
            }
        }
    }

    void ChasePlayer()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        float distanceToTarget = Vector2.Distance(transform.position, target.position);

        if (distanceToTarget < minDistanceFromPlayer)
        {
            transform.position -= new Vector3(direction.x, direction.y, 0) * moveSpeed * Time.deltaTime;
        }
        else if (distanceToTarget > maxDistanceFromPlayer)
        {
            transform.position += new Vector3(direction.x, direction.y, 0) * moveSpeed * Time.deltaTime;
        }
        else
        {
            transform.position += Vector3.zero;
        }

        if (direction.x > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (direction.x < 0 && isFacingRight)
        {
            Flip();
        }
    }

    IEnumerator ShootFireballs()
    {
        animator.SetTrigger("shoot");

        yield return new WaitForSeconds(0.5f);

        GameObject fireball = Instantiate(fireballPrefab, firePoint.position, firePoint.rotation);
        fireball.transform.position = firePoint.position;

        Vector3 direction = (target.position - firePoint.position).normalized; // Updated to use firePoint.position

        if (direction.x > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (direction.x < 0 && isFacingRight)
        {
            Flip();
        }

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        fireball.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        fireball.GetComponent<Rigidbody2D>().velocity = direction * fireballSpeed;
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public bool IsCutsceneActive()
    {
        return isCutsceneActive;
    }

    public void EndCutscene()
    {
        isCutsceneActive = false;
        animator.SetBool("isFlying", false);
    }

    void UpdateTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Rat");
        GameObject[] knightTargets = GameObject.FindGameObjectsWithTag("Knight");
        GameObject[] allTargets = new GameObject[targets.Length + knightTargets.Length];
        targets.CopyTo(allTargets, 0);
        knightTargets.CopyTo(allTargets, targets.Length);

        float closestDistance = Mathf.Infinity;
        GameObject closestTarget = null;

        foreach (GameObject possibleTarget in allTargets)
        {
            if (possibleTarget.activeInHierarchy)
            {
                float distance = Vector2.Distance(transform.position, possibleTarget.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = possibleTarget;
                }
            }
        }

        if (closestTarget != null)
        {
            target = closestTarget.transform;
        }
        else
        {
            target = null;
        }
    }
    IEnumerator ChaseCatnip(GameObject catnip)
    {
        animator.SetBool("isChasingCatnip", true);
        animator.SetBool("isFlying", true);
        isChasingCatnip = true;
        fireballTimer = 0f; 
        Vector2 direction = (catnip.transform.position - transform.position).normalized;
        transform.position += new Vector3(direction.x, direction.y, 0) * moveSpeed * catnipChaseSpeedMultiplier * Time.deltaTime;

        if (Vector2.Distance(transform.position, catnip.transform.position) <= 0.1f)
        {
            StartCoroutine(ConsumeCatnip());
        }

        yield return null;
        isChasingCatnip = false;
        animator.SetBool("isChasingCatnip", false);
        animator.SetBool("isFlying", false);
    }

    IEnumerator ConsumeCatnip()
    {
        if (hasDescended) yield break;
        isConsumingCatnip = true;

        // Disable fireball shooting
        fireballTimer = -1f;

        // Start descending
        float elapsed = 0;
        Vector3 startScale = transform.localScale;
        Vector3 targetScale = startScale * 0.8f; // You can modify the scaling value here

        while (elapsed < descendDuration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / descendDuration;
            transform.localScale = Vector3.Lerp(startScale, targetScale, progress);
            yield return null;
        }

        // Add hitbox
        hitboxCollider.enabled = true;
        hasDescended = true;
        // Consume catnip
        yield return new WaitForSeconds(catnipConsumptionDuration);
        // Remove hitbox
        hitboxCollider.enabled = false;

        // Start ascending
        elapsed = 0;
        while (elapsed < ascendDuration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / ascendDuration;
            transform.localScale = Vector3.Lerp(targetScale, startScale, progress);
            yield return null;
        }

        // Reset scale
        transform.localScale = startScale;

        // Resume fireball shooting
        fireballTimer = 0f;

        isConsumingCatnip = false;
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Sword"))
        {
            TakeDamage(damagePerHit);
        }
    }

    public void TakeDamage(int damage)
    {
        if (!isConsumingCatnip || !hitboxCollider.enabled) return;

        health -= damage;
        StartCoroutine(ShowDamageTaken());

    }

    IEnumerator ShowDamageTaken()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(invincibilityDuration);
        spriteRenderer.color = originalColor;
    }

}