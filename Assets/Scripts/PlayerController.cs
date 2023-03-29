using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 8f;
    public RuntimeAnimatorController KnightController;
    public RuntimeAnimatorController RatController;

    private Animator animator;
    private Rigidbody2D rb2d;
    private float lastHorizontalInput = 1f;
    private bool isKnightController = false;
    private float ratScale = 6f;
    private float knightScale = 11f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        transform.localScale = new Vector3(ratScale, ratScale, 1f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            isKnightController = !isKnightController;

            if (isKnightController)
            {
                animator.runtimeAnimatorController = KnightController;
                moveSpeed = 5f;
                transform.localScale = new Vector3(knightScale, knightScale, 1f);
            }
            else
            {
                animator.runtimeAnimatorController = RatController;
                moveSpeed = 8f;
                transform.localScale = new Vector3(ratScale, ratScale, 1f);
            }
        }
    }

    private void FixedUpdate()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        animator.SetFloat("HorizontalValue", horizontalInput);
        animator.SetFloat("VerticalValue", verticalInput);

        Vector2 movement = new Vector2(horizontalInput, verticalInput);
        movement.Normalize();
        rb2d.velocity = movement * moveSpeed;

        bool isMoving = Mathf.Abs(rb2d.velocity.x) > 0.1f || Mathf.Abs(rb2d.velocity.y) > 0.1f;
        animator.SetBool("Moving", isMoving);

        if (horizontalInput != 0f)
        {
            lastHorizontalInput = horizontalInput;
        }
        if (isMoving == false)
        {
            animator.SetFloat("HorizontalValue", lastHorizontalInput);
        }
    }
}
