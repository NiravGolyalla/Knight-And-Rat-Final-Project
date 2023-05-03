using UnityEngine;

public class BarrelController : MonoBehaviour
{
    private bool isPushing;
    public bool solved = false;
    private Rigidbody2D rb;

    void Start() {
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.mass = int.MaxValue;

        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (PlayerController.isKnightController)
        {
            isPushing = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        isPushing = false;
        rb.mass = int.MaxValue;
    }

    void OnCollisionStay2D(Collision2D collision)
    {

        if (PlayerController.isKnightController && isPushing)
        {
            rb.mass = 10f;
            Vector2 pushDirection = transform.position - PlayerController.instantance.transform.position;

            GetComponent<Rigidbody2D>().AddForce(pushDirection.normalized * rb.mass * Time.deltaTime);
        }
    }
}
