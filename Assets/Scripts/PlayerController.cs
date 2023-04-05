using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Setup Variables")]
    [SerializeField] private GameObject rat;
    [SerializeField] private GameObject knight;
    [SerializeField] private Bar_Controller healthBar;
    [SerializeField] private Bar_Controller staminaBar;
    [SerializeField] private Bar_Controller formBar;

    
    //Swaping variables
    private Animator currAnimator;
    private Animator ratAnimator;
    private Animator knightAnimator;
    private Rigidbody2D rb2d;
    
    //State Variables
    private bool isKnightController = false;
    private bool isMoving = false;
    private bool isAttacking = false;
    private bool isDashing = false;
    private bool isInteracting = false;

    //movement variables
    private float horizontalInput = 1f;
    private float verticalInput = 1f;
    
    [Header("Customizable Variables")]
    [SerializeField] private float health = 10f;
    [SerializeField] private float stamina = 10f;
    [SerializeField] private float dmgTakeR = 1f;
    [SerializeField] private float dmgTakeK = 0.5f;
    [SerializeField] private float stmLostR = 1f;
    [SerializeField] private float stmLostK = 5f;
    [SerializeField] private float ratMoveSpeed = 10f;
    [SerializeField] private float knightMoveSpeed = 5f;
    [SerializeField] private float runBonusMoveSpeed = 5f;
    

    //Managing Animations
    private int currentState;
    private float lockedTimer;
    //private float 

    private static readonly int R_Idle_D = Animator.StringToHash("R_Idle_D");
    private static readonly int R_Idle_LR = Animator.StringToHash("R_Idle_LR");
    private static readonly int R_Idle_U = Animator.StringToHash("R_Idle_U");
    private static readonly int R_Move_D = Animator.StringToHash("R_Move_D");
    private static readonly int R_Move_LR = Animator.StringToHash("R_Move_LR");
    private static readonly int R_Move_U = Animator.StringToHash("R_Move_U");
    private static readonly int K_Idle_D = Animator.StringToHash("K_Idle_D");
    private static readonly int K_Idle_LR = Animator.StringToHash("K_Idle_LR");
    private static readonly int K_Idle_U = Animator.StringToHash("K_Idle_U");
    private static readonly int K_Move_D = Animator.StringToHash("K_Move_D");
    private static readonly int K_Move_LR = Animator.StringToHash("K_Move_LR");
    private static readonly int K_Move_U = Animator.StringToHash("K_Move_U");
    private static readonly int K_Attack_D = Animator.StringToHash("K_Attack_D");
    private static readonly int K_Attack_LR = Animator.StringToHash("K_Attack_LR");
    private static readonly int K_Attack_U = Animator.StringToHash("K_Attack_U");
    //May Need to put in a roll(idk yet)
    private static readonly int K_Dash_D = Animator.StringToHash("K_Dash_D");
    private static readonly int K_Dash_LR = Animator.StringToHash("K_Dash_LR");
    private static readonly int K_Dash_U = Animator.StringToHash("K_Dash_U");
    



    private void Start()
    {
        ratAnimator = rat.GetComponent<Animator>();
        knightAnimator = knight.GetComponent<Animator>();
        isKnightController = false;
        rat.SetActive(!isKnightController);
        knight.SetActive(isKnightController);
        rb2d = GetComponent<Rigidbody2D>();
        currAnimator = ratAnimator;
        currentState = R_Idle_D;
        healthBar.setMaxValue(health);
        healthBar.setValue(health);
        staminaBar.setMaxValue(stamina);
        staminaBar.setValue(stamina);
    }

    private void Update()
    {
        cInput();
        int state = updateSprite();
        if(state != currentState){
            currAnimator.CrossFade(state,0,0);
            currentState = state;
            print(state);
        }
    }

    private void FixedUpdate(){
        Movement();
    }

    private void cInput(){
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Q))
        {
            isKnightController = !isKnightController;
            rat.SetActive(!isKnightController);
            knight.SetActive(isKnightController);
            currAnimator = isKnightController ? knightAnimator : ratAnimator;
        }

        isAttacking = (Input.GetMouseButtonDown(0) && isKnightController);
        isMoving = (horizontalInput != 0 || verticalInput != 0); 
        isInteracting = Input.GetKeyDown(KeyCode.E); 
        isDashing = Input.GetKey(KeyCode.LeftShift); 
    }

    private void Movement(){
        Vector2 movement = new Vector2(horizontalInput, verticalInput);
        movement.Normalize();
        float moveSpeed = isKnightController ? knightMoveSpeed : ratMoveSpeed;
        moveSpeed += (!isKnightController && isDashing) ? runBonusMoveSpeed : 0f;
        if(isDashing){Dash();}

        rb2d.velocity = movement * moveSpeed;
    }

    private int updateSprite(){
        if(Time.time < lockedTimer) return currentState;

        if(isKnightController){
            if (isDashing) return lockState(K_Dash_LR,1);
            if (isAttacking) return lockState(K_Attack_LR,1);
            if (isMoving) return K_Move_LR;
            else return K_Idle_LR;
        } else{
            if (isMoving) return R_Move_LR;
            else return R_Idle_D;
        }
        int lockState(int s,float t){
            lockedTimer = Time.time + t;
            return s;
        }

        return currentState;
    }

    private void TakeDamage(float dmg){
        float take = isKnightController ? dmgTakeK*dmg : dmgTakeR*dmg;
        healthBar.setValue(healthBar.getValue()-take);
    }
    private void Heal(float heal){healthBar.setValue(healthBar.getValue()+heal);}
    private void Dash(){
        float take = isKnightController ? stmLostK : stmLostR;
        staminaBar.setValue(staminaBar.getValue()-take*Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Enemy")){
            TakeDamage(2f);
        }
    }


    
}
