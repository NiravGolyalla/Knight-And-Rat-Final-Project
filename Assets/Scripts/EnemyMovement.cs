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
    [SerializeField] private Transform graphics;

    
    //Steering Behavior 
    Vector2 currentVelocity = Vector2.zero;
    private float[] weights = {1f,.5f,10f,.4f,.4f,1f,.2f};
    public float[] Weights{get{return weights;}}

    public float obstacleRadius = .7f;
    public float playerSpace = 1.5f;
    public float enemySpace = 0.5f;
    public float searchRadius = 2f;
    private int dir;
    bool recalc = false;
    private EnemyController ec;    
    private float wanderAngle = 0f;
    public Vector2 center;
    public float knockStrength = 2f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
        ec = GetComponent<EnemyController>();
        dir = Random.Range(0, 2) * 2 - 1;
        center = rb.position;
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
        float extra = 0f;
        float distance = Vector2.Distance((Vector2)target,rb.position);
        // print(ec.state);
        if(ec.state != "Stunned"){
            if(ec.state != "Knockbacked"){
                if(ec.state == "Aggro"){
                    steering += pathSeekBehavior() * weights[0];
                    steering += perpendicularBehavior() * weights[1];
                } else if(ec.state == "Attack"){
                    steering += seekBehavior() * weights[0];
                } else{
                    steering += pathSeekBehaviorWander() * weights[0];
                }
                // steering += avoidObstaclesBehavior("Player",playerSpace) * weights[4];
                // steering += wanderBehavior() * weights[6];
                // steering += spawnBehavior();
            }else{
                steering += knockBacked()* weights[5];
                extra = knockStrength;

            }
            steering += avoidObstaclesBehavior("Obstacle",obstacleRadius) * weights[2];
            steering += avoidObstaclesBehavior("Enemy",enemySpace) * weights[3];
                
            rb.position = Vector2.MoveTowards(rb.position, rb.position + steering , Time.deltaTime*speed+Time.deltaTime*extra);
            float dis = ((Vector2)target-rb.position).x;
            // print(target);
            if(dis != 0){
                graphics.localScale = new Vector3(Mathf.Sign(dis),1f,1f);    
            }   
            currentVelocity = rb.position + steering;
        }
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
                    reachedEndOfPath = true;
                } else{
                    reachedEndOfPath = false;
                }
            }
        }
        float distance = 0;
        if(target!= null){
            distance = Vector2.Distance((Vector2)target,rb.position);
        }
        return target != null ? (desiredVector - rb.position).normalized  * Mathf.Floor((distance-playerSpace))/(playerSpace*5f) : Vector2.zero;
    }

    Vector2 pathSeekBehaviorWander(){
        StartCoroutine(calculatePath());
        Vector2 desiredVector = Vector2.zero; 
        if(path != null){
            desiredVector = (Vector2)path.vectorPath[currentWaypoint];
            if(Vector2.Distance(rb.position,desiredVector) < nextWaypointeDistance){
                currentWaypoint++;
                if(currentWaypoint >= path.vectorPath.Count){
                    currentWaypoint--;
                    reachedEndOfPath = true;
                } else{
                    reachedEndOfPath = false;
                }
            }
        }
        float distance = 0;
        if(target!= null){
            distance = Vector2.Distance((Vector2)target,rb.position);
        }
        return target != null ? (desiredVector - rb.position).normalized  * (distance)/(5f) : Vector2.zero;
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

    Vector2 perpendicularBehavior(){
        float distance = 100f;
        if(target!= null){
            distance = Vector2.Distance((Vector2)target,rb.position);
        }
        Vector2 DesiredVector = Vector2.zero;
        if(target != null){
            Vector2 currVector = ((Vector2)target - rb.position).normalized;
            DesiredVector = Vector2.Perpendicular(currVector) * dir * (1f)/(distance+1f);
            // DesiredVector = -1 * new Vector2(Random.Range(perVector.x, currVector.x), Random.Range(perVector.y, currVector.y));
        }
        return DesiredVector;
    }

    Vector2 knockBacked(){
        return target != null ? -((Vector2)target - rb.position).normalized: Vector2.zero;
    }

    Vector2 wanderBehavior(){
        float wanderRadius = 10.0f;
        float wanderDistance = 5.0f;
        float wanderJitter = 10f;
        Vector2 wanderTarget;
        
        wanderTarget = Random.insideUnitCircle.normalized * wanderRadius;
        Vector2 wanderCenter = currentVelocity.normalized * wanderDistance;
        Vector2 displacement = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized * wanderJitter;
        wanderTarget += displacement;
        // wanderTarget = wanderTarget.normalized * wanderRadius;
        Vector2 target_ = wanderCenter + wanderTarget;

        Vector2 desiredVelocity = (target_ - rb.position);
        return desiredVelocity;
    }

    Vector2 seekBehavior(){
        Vector2 desiredVelocity = Vector2.zero;
        if(target!= null){
            float distance = Vector2.Distance((Vector2)target,rb.position);
            desiredVelocity = ((Vector2)target - rb.position).normalized * (distance)/(5f);
        }
        
        return desiredVelocity;
    }




    List<Transform> GetNearbyObjects(string name)
    {
        List<Transform> context = new List<Transform>();
        Collider2D[] contextColliders = Physics2D.OverlapCircleAll(transform.position, searchRadius);
        int x = LayerMask.NameToLayer(name);
        foreach (Collider2D c in contextColliders)
        {
            // print(x + " " + c.gameObject.layer);
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
        yield return null;
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
