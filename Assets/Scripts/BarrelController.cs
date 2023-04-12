using UnityEngine;

public class BarrelController : MonoBehaviour
{
    public PlayerController playerController;
    [SerializeField] private GameObject knight;

    private bool isPushing;
    private Rigidbody2D rb;

    void Start() {
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.mass = int.MaxValue;

        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (knight.activeSelf )
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

        if (knight.activeSelf && isPushing)
        {
            rb.mass = 10f;
            Vector2 pushDirection = transform.position - playerController.transform.position;

            GetComponent<Rigidbody2D>().AddForce(pushDirection.normalized * rb.mass * Time.deltaTime);
        }
    }
}
