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

    private Transform player;
    private bool isFacingRight = true;
    private float fireballTimer = 0f;
    private float fireballChance = 0.3f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= chaseDistance)
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

    void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < minDistanceFromPlayer)
        {
            transform.position -= new Vector3(direction.x, direction.y, 0) * moveSpeed * Time.deltaTime;
        }
        else if (distanceToPlayer > maxDistanceFromPlayer)
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

        Vector3 direction = (player.position - transform.position).normalized;

        if (direction.x > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (direction.x < 0 && isFacingRight)
        {
            Flip();
        }
        fireball.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);

        fireball.GetComponent<Rigidbody2D>().velocity = direction * fireballSpeed;
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
