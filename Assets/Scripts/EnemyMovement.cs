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
    bool recalc = false; 

    Seeker seeker;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
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

        rb.position = Vector2.MoveTowards(rb.position, (Vector2)path.vectorPath[currentWaypoint], 1f * speed * Time.deltaTime);

        float distance = Vector2.Distance(rb.position,path.vectorPath[currentWaypoint]);

        if(distance < nextWaypointeDistance){
            currentWaypoint++;
        }
    }

}
