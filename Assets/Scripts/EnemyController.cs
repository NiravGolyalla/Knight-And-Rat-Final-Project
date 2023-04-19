using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float aggroRange = 0f; 
    [SerializeField] private float distance = 1f;
    [SerializeField] private string state;
    private Rigidbody2D rb;
    [SerializeField] private float wanderSpeed = 3f;
    [SerializeField] private float aggroSpeed = 2f;
    [SerializeField] private float wanderRange = 5f;
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private EnemyMovement movement;
    private Vector3 startPosition,wanderPosition,targetPosition;
    Path path;
    int currentWayPoint = 0;
    bool reached = false;
    bool takingDamage = false;
    bool isAttacking = false;
    bool Knockbacked = false;
    Seeker seeker;
    [SerializeField] private float health = 5f;
    [SerializeField] private Bar_Controller healthBar;
    public Animator anim;

    Coroutine att;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
        startPosition = rb.position;
        wanderPosition = rb.position;
        healthBar.setMaxValue(health);
        healthBar.setValue(health);
        state = "Wander";
        Wander();
        
    }
    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale != 0){
            if(healthBar.getValue()<= 0f){
                Destroy(gameObject);
            }
            Transform player = findPlayer(aggroRange);
            if(player){
                state = "Aggro";
                Approach(player);
            } else{
                state = "Wander";
                Wander();
            }
        }
        // if(isAttacking){
        //     if(rb.velocity!= Vector2.zero){
        //         anim.CrossFade("Enemy_Move",0,0);
        //     } else{
        //         anim.CrossFade("Enemy_Idle",0,0);
        //     }
        // }
        
    }

    private void Wander(){
        float dis = Vector2.Distance((Vector2)(rb.position),(Vector2)(wanderPosition));
        movement.speed = wanderSpeed;
        if ((dis < 0.1f || movement.reachedEndOfPath) && !reached)
        {
            reached = true;
            wanderPosition.x = Random.Range(startPosition.x - wanderRange, startPosition.x + wanderRange);
            wanderPosition.y = Random.Range(startPosition.y - wanderRange, startPosition.y + wanderRange);
            movement.setTarget(wanderPosition);
            reached = false;
        }
    }

    private void Approach(Transform target){
        Vector2 direction = (Vector2)target.position - rb.position;
        targetPosition = (Vector2)target.position - direction.normalized * distance;
        movement.setTarget(targetPosition);
        movement.speed = aggroSpeed;
        float dis = Vector2.Distance((Vector2)(rb.position),(Vector2)(target.position));
        if(dis-0.1f < distance){
            att = StartCoroutine(Attack(target));
        }
    }

    private Transform findPlayer(float radius){
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
    
        foreach (Collider2D c in colliders){       
            if (c.tag == "Rat" || c.tag == "Knight"){
                return c.transform;
            }
        }
        return null;
    }

    private void OnDrawGizmos(){
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(startPosition,0.2f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(wanderPosition,0.2f);
    }

    public void takeDamage(){
        if(!takingDamage){
            // if(att != null) StopCoroutine(att); 
            // isAttacking = false;
            takingDamage = true;
            healthBar.setValue(healthBar.getValue()-2f);
            
            Vector2 direction = ((Vector2)targetPosition - rb.position).normalized;
            rb.AddForce(-direction*10f,ForceMode2D.Impulse);
            Invoke("delay",1f);
            takingDamage = false;
        }
    }
    public IEnumerator Attack(Transform player){
        if(!isAttacking){
            isAttacking = true;
            // anim.CrossFade("Enemy_Attack",0,0);
            StartCoroutine(player.parent.gameObject.GetComponent<PlayerController>().TakeDamage(2f));
            yield return new WaitForSeconds(2f);
            isAttacking = false;
        }
    }

    void OnCollisionEnter2D(Collision2D r){
        if(r.transform.tag == "Player")
            r.otherRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
    }
     
    void OnCollisionExit2D(Collision2D r){
        if(r.transform.tag == "Player")
            r.otherRigidbody.constraints = RigidbodyConstraints2D.None;
            r.otherRigidbody.freezeRotation = true;
    }

    void delay(){
        rb.velocity = Vector2.zero;
    }

}
