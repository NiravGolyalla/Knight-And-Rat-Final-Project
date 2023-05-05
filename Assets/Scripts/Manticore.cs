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

    void Start()
    {
        UpdateTarget();
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

            if (target != null)
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
}

