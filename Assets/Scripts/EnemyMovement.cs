using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyMovement : MonoBehaviour
{
    private Vector3 target;
    public float speed = 2f;
    public float nextWaypointeDistance = 0.01f;
    
    Path path;
    int currentWaypoint = 0;
    public bool reachedEndOfPath = false;
    public bool movementType = false;
    bool recalc = false; 
    public float[] setUpweights = {1,1,1};
    float[] weights;

    Vector2 currentVelocity;

    Seeker seeker;
    Rigidbody2D rb;

    public float neighborRadius = 1.5f;
    [Range(0f, 1f)]
    public float avoidanceRadiusMultiplier = 0.5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
        weights = setUpweights;
    }

    public void setTarget(Vector3 new_target){
        target = new_target;
    }


    void OnPathComplete(Path p){
        if(!(p.error)){
            path = p;
            currentWaypoint = 0;
        }
    }
    private IEnumerator calculatePath(){
        if(!recalc){
            recalc = true;
            seeker.StartPath(rb.position,target,OnPathComplete);
            yield return new WaitForSeconds(0.2f);
            recalc = false;
        }
    }

    void FixedUpdate()
    {
        StartCoroutine(calculatePath());
        if(path == null){
            return;
        }
        
        if(currentWaypoint >= path.vectorPath.Count){
            reachedEndOfPath = true;
            return;
        } else{
            reachedEndOfPath = false;
        }

        if(movementType){CalculateMove();} else{OldMove();}  

        float distance = Vector2.Distance(rb.position,path.vectorPath[currentWaypoint]);

        if(distance < nextWaypointeDistance){
            currentWaypoint++;
        }
    }

    void OldMove(){
        rb.position = Vector2.MoveTowards(rb.position, (Vector2)path.vectorPath[currentWaypoint], 1f * speed * Time.deltaTime);
    }

    void CalculateMove(){
        List<Transform> neighbors = GetNearbyObjects();
        print(neighbors.Count);

        Vector2 move = Vector2.zero;

        Vector2 dirMove = (Vector2)(path.vectorPath[currentWaypoint]-transform.position);
        
        Vector2 ranMove = (Vector2)Random.insideUnitSphere;
            
        Vector2 avoidanceMove = Vector2.zero;
        float nAvoid = 0;
        foreach (Transform item in neighbors)
        {
            if (Vector2.Distance(item.position,transform.position) < neighborRadius * avoidanceRadiusMultiplier)
            {
                nAvoid++;
                
                avoidanceMove += (Vector2)(transform.position-item.position);
            }
        }
        if (nAvoid > 0)
            avoidanceMove /= nAvoid;
        
        
        Vector2[] behaviors = {dirMove,ranMove,avoidanceMove};
        for (int i = 0; i < behaviors.Length; i++)
        {
            Vector2 partialMove = behaviors[i];
            // print(i);
            // print(partialMove);

            if (partialMove != Vector2.zero)
            {
                partialMove.Normalize();
                partialMove *= setUpweights[i];
                move += partialMove;

            }
        }

        move = move.normalized * speed;
        move = Vector2.SmoothDamp(Vector2.zero,move,ref currentVelocity, 0.5f);
        rb.position += move;
        // rb.position = Vector2.MoveTowards(rb.position, (Vector2)path.vectorPath[currentWaypoint], 1f * speed * Time.deltaTime);
    }

    List<Transform> GetNearbyObjects()
    {
        List<Transform> context = new List<Transform>();
        Collider2D[] contextColliders = Physics2D.OverlapCircleAll(transform.position, neighborRadius);
        foreach (Collider2D c in contextColliders)
        {
            if (c != GetComponent<Collider2D>())
            {
                context.Add(c.transform);
            }
        }
        return context;
    }



}
