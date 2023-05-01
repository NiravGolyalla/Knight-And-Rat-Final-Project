using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManticoreBoss : MonoBehaviour
{
    public GameObject fireballPrefab;
    public GameObject minionPrefab;
    public float speed = 2f;
    public float fireballSpeed = 5f;
    public float fireballInterval = 0.5f;
    public float minionInterval = 2f;
    public float powerfulShotInterval = 5f;
    public float powerfulShotSpeed = 7f;
    public float fireballDamage = 0.2f;
    public float powerfulShotDamage = 0.33f;

    private float fireballTimer = 0f;
    private float minionTimer = 0f;
    private float powerfulShotTimer = 0f;
    private bool isMovingUp = true;
    private PlayerController playerController;

    public Color powerfulShotColor = Color.blue;

    private void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        // Move up and down
        if (isMovingUp)
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
            if (transform.position.y >= 5f)
            {
                isMovingUp = false;
            }
        }
        else
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
            if (transform.position.y <= 3f)
            {
                isMovingUp = true;
            }
        }

        // Shoot fireball
        fireballTimer += Time.deltaTime;
        if (fireballTimer >= fireballInterval)
        {
            fireballTimer = 0f;
            ShootFireball();
        }

        // Spawn minion
        minionTimer += Time.deltaTime;
        if (minionTimer >= minionInterval)
        {
            minionTimer = 0f;
            SpawnMinion();
        }

        // Shoot powerful shot
        powerfulShotTimer += Time.deltaTime;
        if (powerfulShotTimer >= powerfulShotInterval)
        {
            powerfulShotTimer = 0f;
            ShootPowerfulShot();
        }
    }

    private void ShootFireball()
    {

        GameObject fireball = Instantiate(fireballPrefab, transform.position, Quaternion.identity);
        fireball.GetComponent<Rigidbody2D>().velocity = (playerController.transform.position - transform.position).normalized * fireballSpeed;
        fireball.GetComponent<FireShot>().damage = fireballDamage;
        Destroy(fireball, 5f);
    }


    private void SpawnMinion()
    {
        Vector2 spawnPosition = Random.insideUnitCircle.normalized * 2f + (Vector2)transform.position;
        Instantiate(minionPrefab, spawnPosition, Quaternion.identity);
    }

    private void ShootPowerfulShot()
    {
        GameObject powerfulShot = Instantiate(fireballPrefab, transform.position, Quaternion.identity);
        powerfulShot.GetComponent<SpriteRenderer>().color = powerfulShotColor;
        powerfulShot.GetComponent<Rigidbody2D>().velocity = (playerController.transform.position - transform.position).normalized * powerfulShotSpeed;
        powerfulShot.GetComponent<PowerfulShot>().damage = powerfulShotDamage;
        Destroy(powerfulShot, 5f);
    }

    private class FireShot : MonoBehaviour
    {
        public float damage;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                PlayerController playerController = collision.GetComponent<PlayerController>();
                playerController.TakeDamage(damage);
                Destroy(gameObject);
            }
            else if (collision.CompareTag("Wall"))
            {
                Destroy(gameObject);
            }
        }
    }

    private class PowerfulShot : MonoBehaviour
    {
        public float damage;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                PlayerController playerController = collision.GetComponent<PlayerController>();
                playerController.TakeDamage(damage);
                Destroy(gameObject);
            }
            else if (collision.CompareTag("Wall"))
            {
                Destroy(gameObject);
            }
        }
    }
}