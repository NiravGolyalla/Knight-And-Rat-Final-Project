using UnityEngine;

public class BarrelController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float pushPower = 10f;
    public PlayerController playerController;

    private BoxCollider2D bc;
    private bool isPushing;

    void Start()
    {
        bc = GetComponent<BoxCollider2D>();
    }

    void FixedUpdate()
    {
        // float horizontal = Input.GetAxis("Horizontal");
        // float vertical = Input.GetAxis("Vertical");

        // Vector2 movement = new Vector2(horizontal, vertical) * moveSpeed;

        // if (isPushing)
        // {
        //     movement *= 0.5f;
        // }

        // transform.Translate(movement * Time.deltaTime);
    }

    void OnColliderEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Knight"))
        {
            print("yes");
            isPushing = true;

            Vector2 pushDirection = transform.position - playerController.transform.position;

            transform.Translate(pushDirection.normalized * pushPower * Time.deltaTime);
        }
    }

    void OnColliderExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Knight"))
        {
            isPushing = false;
        }
    }
}
