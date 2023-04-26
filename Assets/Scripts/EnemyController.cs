using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float aggroRange = 0f; 
    [SerializeField] private float distance = 1f;
    public string state;
    private Rigidbody2D rb;
    [SerializeField] private float wanderSpeed = 3f;
    [SerializeField] private float aggroSpeed = 2f;
    [SerializeField] private float wanderRange = 5f;
    [SerializeField] private EnemyMovement movement;
    
    private Vector3 startPosition,wanderPosition,targetPosition;
    Path path;
    bool reached = false;
    bool takingDamage = false;
    bool isAttacking = false;
    bool Knockbacked = false;
    Seeker seeker;
    [SerializeField] private float health = 5f;
    [SerializeField] private Bar_Controller healthBar;
    public Animator anim;
    

    float Timer = 0;

    public bool stunned = false;

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
            setState();
            string s = updateSprite();
            anim.CrossFade(s,0,0);
        }
        Timer += Time.deltaTime;
        
    }

    private void setState(){
        Transform cat = findCatnip(aggroRange);
        Transform player = findPlayer(aggroRange);
        
        if(state == "Knockbacked" || state == "Stunned" || state == "Attack"){
            return;
        }
        if(cat){
            state = "Cated";
            movement.setTarget(cat.position);
        }
        else if(player){
            state = "Aggro";
            movement.setTarget(player.position);
            StartCoroutine(Attack(player));
        } else{
            state = "Wander";
            Wander();
        }
    }

    private void Wander(){
        movement.setTarget(wanderPosition);
        movement.speed = wanderSpeed;
        float dis = Vector2.Distance((Vector2)(rb.position),(Vector2)(wanderPosition));
        if ((dis < 0.1f || movement.reachedEndOfPath) || Timer > 5f)
        {
            Timer = 0f;
            wanderPosition.x = Random.Range(startPosition.x - wanderRange, startPosition.x + wanderRange);
            wanderPosition.y = Random.Range(startPosition.y - wanderRange, startPosition.y + wanderRange);
        }
    }

    private void Approach(Transform target){
        targetPosition = (Vector2)target.position;
        movement.setTarget(targetPosition);
        movement.speed = aggroSpeed;
        float dis = Vector2.Distance((Vector2)(rb.position),(Vector2)(target.position));
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

    private Transform findCatnip(float radius){
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
    
        foreach (Collider2D c in colliders){       
            if (c.tag == "catnip"){
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
            state = "Attack";
            yield return new WaitForSeconds(.5f);
            state = "Aggro";
            yield return new WaitForSeconds(2f);
            isAttacking = false;
        }
        yield return null;
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

    private string updateSprite(){
        
        if (state == "Knockbacked") return "Enemy_Idle";
        if (state == "Attack") return "Enemy_Attack";
        return "Enemy_Move";
    }

}
