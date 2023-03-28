using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;

    public float speed;

    void Start(){
        rb = GetComponent<Rigidbody2D>();
    }

    Vector2 movement;
    void Update()
    {
        movement.y = Input.GetAxis("Vertical");
        movement.x = Input.GetAxis("Horizontal") ;
    }

    void FixedUpdate(){
        rb.MovePosition(rb.position + movement*speed*Time.deltaTime);
    }
}
