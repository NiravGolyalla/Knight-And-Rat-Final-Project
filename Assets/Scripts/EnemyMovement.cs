using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyMovement : MonoBehaviour
{
    //General Vars
    Rigidbody2D rb;
    public float speed = 2f;

    //Type of movement vars
    public static bool movementType = true;
    
    // A* PathFinding vars
    Path path;
    int currentWaypoint = 0;
    public bool reachedEndOfPath = false;
    Seeker seeker;
    private Vector3 target;
    public float nextWaypointeDistance = 5f;

    
    //Steering Behavior 
    Vector2 currentVelocity = Vector2.zero;
    private float[] weights;
    public float[] Weights{get{return weights;}}

    public float obstacleRadius = .7f;
    public float playerSpace = 1.5f;
    public float searchRadius = 2f;



    bool recalc = false;    
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
    }

    void FixedUpdate()
    {
        if(movementType){behaviorBasedMovementLoop();} else{previousMovementLoop();}  
    }
    //behavior movement
    void behaviorBasedMovementLoop(){
        CalculateMove();
    }

    void CalculateMove(){
        Vector2 steering = Vector2.zero;
        Vector2 tar = pathSeekBehavior() * 1f;
        // steering += ((Vector2)target - rb.position).normalized * speed * Time.deltaTime;
        steering += pathSeekBehavior();
        steering += avoidObstaclesBehavior("Obstacle",obstacleRadius) * 2f;
        steering += avoidObstaclesBehavior("Player",playerSpace) * 1.5f;
        // print(Mathf.Abs(((Vector2)target - rb.position).magnitude));
        rb.position = Vector2.MoveTowards(rb.position, rb.position + steering , Time.deltaTime*speed);
        // rb.position = (Mathf.Abs(((Vector2)target - rb.position).magnitude) <= 2f) ? rb.position : Vector2.MoveTowards(rb.position, rb.position + steering , Time.deltaTime*speed);
        // rb.position = rb.position;
    }

    Vector2 pathSeekBehavior(){
        StartCoroutine(calculatePath());
        Vector2 desiredVector = Vector2.zero; 
        if(path != null){
            desiredVector = (Vector2)path.vectorPath[currentWaypoint];
            if(Vector2.Distance(rb.position,desiredVector) < nextWaypointeDistance){
                currentWaypoint++;
                if(currentWaypoint >= path.vectorPath.Count){
                    currentWaypoint--;
                }        
            }
        }
        return target != null ? (desiredVector - rb.position).normalized : Vector2.zero;
    }

    Vector2 avoidObstaclesBehavior(string name,float radius){
        Vector2 desiredVector = Vector2.zero;
        float nAvoid = 0;
        List<Transform> contextColliders = GetNearbyObjects(name);
        // print(contextColliders.Count);
        foreach (Transform item in contextColliders)
        {
            ;
            Vector2 closestPoint = item.gameObject.GetComponent<Collider2D>().ClosestPoint(rb.position);
            if ((closestPoint - rb.position).magnitude < radius)
            {
                nAvoid++;
                desiredVector += (Vector2)(rb.position - closestPoint);
            }
        }
        if (nAvoid > 0)
            desiredVector /= nAvoid;

        return desiredVector.normalized;
    }
    List<Transform> GetNearbyObjects(string name)
    {
        List<Transform> context = new List<Transform>();
        Collider2D[] contextColliders = Physics2D.OverlapCircleAll(transform.position, searchRadius);
        int x = LayerMask.NameToLayer(name);
        foreach (Collider2D c in contextColliders)
        {
            print(x + " " + c.gameObject.layer);
            if (c != GetComponent<Collider2D>() && x == c.gameObject.layer)
            {
                context.Add(c.transform);
            }
        }
        return context;
    }

    //A* path calculation
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
    
    //OLD STUFF
    void OldMove(){
        rb.position = Vector2.MoveTowards(rb.position, (Vector2)path.vectorPath[currentWaypoint], 1f * speed * Time.deltaTime);
    }

    void previousMovementLoop(){
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

        OldMove();  

        float distance = Vector2.Distance(rb.position,path.vectorPath[currentWaypoint]);

        if(distance < nextWaypointeDistance){
            currentWaypoint++;
        }
    }





}
