using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public List<Spot> path;
    private int pathIndex = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = rb.position;
        wanderPosition = rb.position;
        path = new List<Spot>();
        pathIndex = 0;
        state = "Wander";
    }


    // Update is called once per frame
    void Update()
    {
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
        // A star failed rn
        // if (rb.position == wanderPosition)
        // {   
        //     if(path == null){
        //         print(path);
        //         return;
        //     }
        //     if(!(pathIndex < path.Count)){
        //         print("imposter");
        //         Vector3 str = new Vector3(rb.position.x,rb.position.y,0);
        //         Vector3 end = new Vector3(0,0,0);
        //         end.x = Random.Range(startPosition.x - wanderRange, startPosition.x + wanderRange);
        //         end.y = Random.Range(startPosition.y - wanderRange, startPosition.y + wanderRange);
        //         print(str);
        //         print(end);
        //         path.Clear();
        //         path = GenerateGrid.Instance.GeneratePath(str,end);
        //         pathIndex = 0;
        //     }
        //     print(path.Count);
        //     wanderPosition.x = path[pathIndex].X;
        //     wanderPosition.y = path[pathIndex].Y;
        // }
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
