using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float aggroRange = 0f; 
    [SerializeField] private string state;
    private Rigidbody2D rb;
    [SerializeField] private float wanderSpeed = 3f;
    [SerializeField] private float aggroSpeed = 2f;
    [SerializeField] private float wanderRange = 5f;
    [SerializeField] private string playerTag = "Player";
    private Vector2 startPosition,wanderPosition,targetPosition;
    Path path;
    int currentWayPoint = 0;
    bool reached = false;
    Seeker seeker;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
        startPosition = rb.position;
        wanderPosition = rb.position;
        state = "Wander";

        seeker.StartPath(startPosition,wanderPosition,OnPathComplete);
    }

    void OnPathComplete(Path p){
        if(!p.error){
            path = p;
            currentWayPoint = 0;
        }
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        if(path == null){
            return;
        }
        Transform player = findPlayer(aggroRange);
        if(player){
            state = "Aggro";
            Approach(player,1f);
        } else{
            state = "Wander";
            Wander();
        }
    }

    private void Move(Vector2 target, float speed){ 
        rb.position = Vector2.MoveTowards(rb.position, target, 1f * speed * Time.deltaTime);
    }

    private void Wander(){
      
        if (rb.position == wanderPosition)
        {
            wanderPosition.x = Random.Range(startPosition.x - wanderRange, startPosition.x + wanderRange);
            wanderPosition.y = Random.Range(startPosition.y - wanderRange, startPosition.y + wanderRange);
        }

        Move(wanderPosition,wanderSpeed);
    }

    private void Approach(Transform target,float distance){
        Vector2 direction = (Vector2)target.position - rb.position;
        targetPosition = (Vector2)target.position - direction.normalized * distance;
        Move(targetPosition,aggroSpeed);
    }

    private Transform findPlayer(float radius){
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
        
        foreach (Collider2D c in colliders){       
            //print(c);
            if (c.tag == playerTag){
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
}
