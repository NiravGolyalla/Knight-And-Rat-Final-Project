using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instantance;
    [Header("Setup Variables")]
    [SerializeField] private GameObject rat;
    [SerializeField] private GameObject knight;
    [SerializeField] private GameObject ratIcon;
    [SerializeField] private GameObject knightIcon;
    [SerializeField] private Sword sw;
    [SerializeField] public Bar_Controller healthBar;
    [SerializeField] private Bar_Controller staminaBar;
    [SerializeField] private Bar_Controller formBar;
    [SerializeField] private GameObject poof;
    [SerializeField] private Color damageColor = Color.red;
    [SerializeField] private float damageIndicatorDuration = 0.5f;
    [SerializeField] private bool isShowingDamageIndicator = false;
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] private float shakeIntensity = 2f;
    [SerializeField] private float shakeDuration = 0.2f;

    
    //Swaping variables
    private Animator currAnimator;
    private Animator ratAnimator;
    private Animator knightAnimator;
    private Rigidbody2D rb2d;
    
    //State Variables
    public static bool isKnightController = false;
    private bool isMoving = false;
    public bool isAttacking = false;
    private bool isDashing = false;
    private bool isRecovering = false;
    private bool isHeld = false;

    private bool canDash = true;
    private bool canMove = true;
    public bool canAttack = true;
    private bool canHold = false;
    private bool canSwap = true;
    public bool takingDamage = false;

    private float inputBuffer = 0.4f;
    private float cooldownBuffer;

    //movement variables
    private float horizontalInput = 1f;
    private float verticalInput = 1f;
    
    [Header("Customizable Variables")]
    [SerializeField] public float health = 10f;
    [SerializeField] private float stamina = 10f;
    [SerializeField] public float dmgTakeR = 1f;
    [SerializeField] public float dmgTakeK = 0.5f;
    [SerializeField] private float stmLostR = 1f;
    [SerializeField] private float stmLostK = 5f;
    [SerializeField] private float ratMoveSpeed = 10f;
    [SerializeField] private float knightMoveSpeed = 5f;
    [SerializeField] private float runBonusMoveSpeed = 5f;
    [SerializeField] private float dashAmount = 2f;
    [SerializeField] private float dashDur = 0.5f;
    [SerializeField] private float dashCooldown = 2f;
    
    //Managing Animations
    private int currentState;
    private float lockedTimer;
    private float RIdle;
    private float RMove;
    private float KIdle;
    private float KMove;
    private float KDash;
    private float KRecover;
    private float KAttack;
    private float KHoldAttack;
    private float KAttackWalk;
    private float KHoldAttackWalk;
    private static readonly int R_Idle_LR = Animator.StringToHash("R_Idle_LR");
    private static readonly int R_Move_LR = Animator.StringToHash("R_Move_LR");
    private static readonly int K_Idle_LR = Animator.StringToHash("K_Idle_LR");
    private static readonly int K_Move_LR = Animator.StringToHash("K_Move_LR");
    private static readonly int K_Dash_LR = Animator.StringToHash("K_Dash_LR");
    private static readonly int K_Recover_LR = Animator.StringToHash("K_Recover_LR");
    private static readonly int K_Attack_LR = Animator.StringToHash("K_Attack_LR");
    private static readonly int K_HoldAttack_LR = Animator.StringToHash("K_HoldAttack_LR");
    private static readonly int K_AttackWalk_LR = Animator.StringToHash("K_AttackWalk_LR");
    private static readonly int K_HoldAttackWalk_LR = Animator.StringToHash("K_HoldAttackWalk_LR");
    private bool isAttackSoundPlaying = false;
    //0.017f
    float frame_refer = 0.017f;
        
    //Unity Functions
    private void Awake(){
        instantance = this;
    }
    private void Start()
    {
        ratAnimator = rat.GetComponent<Animator>();
        knightAnimator = knight.GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        UpdateAnimClipTimes();
        FindAnyObjectByType<AudioManager>().Play("KnightWalk");
        Setup();
    }

    void Setup(){
        

        rat.SetActive(!isKnightController);
        knight.SetActive(isKnightController);

        ratIcon.SetActive(!isKnightController);
        knightIcon.SetActive(isKnightController);
        
        currAnimator = isKnightController ? knightAnimator : ratAnimator;
        currentState = isKnightController ? K_Idle_LR : R_Idle_LR;
        
        healthBar.setMaxValue(health);
        healthBar.setValue(health);
        staminaBar.setMaxValue(stamina);
        staminaBar.setValue(stamina);
    }

    private void Update()
    {
        // if(Time.timeScale != 0 || !LevelManager.instance.reloading){
        if(Time.timeScale != 0){
            cInput();
            Movement();
            int state = updateSprite();
            if(state != currentState){
                // print(state);
                currAnimator.CrossFade(state,0,0);
                currentState = state;
            }
            regenBar(healthBar,0.1f);
            regenBar(staminaBar,2f);
            regenBar(formBar,7f);
            
        }
        cooldownBuffer += Time.deltaTime;
        if(healthBar.getValue()<= 0f){LevelManager.instance.Reload();}
    }


    //Actions
    private void cInput(){
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        isAttacking = ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))  && isKnightController);
        isMoving = (horizontalInput != 0 || verticalInput != 0); 
        isDashing = (Input.GetKey(KeyCode.LeftShift) && canDash);
        isHeld = (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space));
        
        // print(isHeld && canHolds && isMoving);

        //Swap
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Swap();
        }
        if (isMoving)
        {
            FindAnyObjectByType<AudioManager>().SetSoundVolume("KnightWalk", 1f);
        }
        else
        {
            FindAnyObjectByType<AudioManager>().SetSoundVolume("KnightWalk", 0f);
        }
    }

    private void Movement(){
        Vector2 movement = new Vector2(horizontalInput, verticalInput);
        movement.Normalize();
        float moveSpeed = isKnightController ? knightMoveSpeed : ratMoveSpeed;
        moveSpeed += (!isKnightController && isDashing && staminaBar.getValue() > .2f) ? runBonusMoveSpeed : 0f;
        
        if(isDashing){
            StartCoroutine(Dash());
        }
        if(isAttacking){
            StartCoroutine(Attack());
        } 
        
        rb2d.velocity = (canMove) ?  movement * moveSpeed : rb2d.velocity;
        if(rb2d.velocity.x != 0f && canMove && !isAttacking){
            transform.localScale = new Vector3(Mathf.Sign(rb2d.velocity.x), 1, 1);
        }
    }

    public IEnumerator TakeDamage(float dmg)
    {
        if (!takingDamage)
        {
            takingDamage = true;
            float take = isKnightController ? dmgTakeK * dmg : dmgTakeR * dmg;
            healthBar.setValue(healthBar.getValue() - take);

            StartCoroutine(ShowDamageIndicator());
            
            yield return new WaitForSeconds(0.5f);
            takingDamage = false;
        }
        yield return null;
    }

    public IEnumerator Attack(){
        if(isAttacking && canAttack){
            canAttack = false;
            cooldownBuffer = 0;
            if(isMoving){
                yield return new WaitForSeconds(KAttackWalk);
            } else{
                yield return new WaitForSeconds(KAttack);
            }
            canHold = true;
            while((isHeld || !sw.exitFrame) && 0 < staminaBar.getValue()){
                if(cooldownBuffer > inputBuffer){
                    regenBar(staminaBar,-12f);
                }
                yield return null;
            }
            canHold = false;
            isRecovering = true;
            canMove = false;
            rb2d.velocity = new Vector2(0f,0f);
            yield return new WaitForSeconds(KRecover);
            isRecovering = false;
            canAttack = true;
            canMove = true;
            
        }
        yield return null;
    }

    public void Heal(float heal){healthBar.setValue(healthBar.getValue()+heal);}
    
    public void Swap(){
        if(canSwap && formBar.getValue() == formBar.getMaxValue()){
            Instantiate(poof, transform.position, Quaternion.identity);
            isKnightController = !isKnightController;
            rat.SetActive(!isKnightController);
            knight.SetActive(isKnightController);
            ratIcon.SetActive(!isKnightController);
            knightIcon.SetActive(isKnightController);
            currAnimator = isKnightController ? knightAnimator : ratAnimator;
            formBar.setValue(0f);
        }
    }

    private IEnumerator Dash(){
        float take = isKnightController ? stmLostK : stmLostR;
        if(canDash && take < staminaBar.getValue() && !isAttacking){
            canDash = false;
            // print(isKnightController);
            if(isKnightController){
                canMove = false;
                if(!(horizontalInput == 0 && verticalInput == 0)){
                    rb2d.velocity = new Vector2(horizontalInput, verticalInput)* 3*knightMoveSpeed/4 *dashAmount;
                } else{
                    rb2d.velocity = new Vector3(-Mathf.Sign(transform.localScale.x)*dashAmount*knightMoveSpeed/2f,0f,0f);
                    transform.localScale = new Vector3(-Mathf.Sign(rb2d.velocity.x), 1, 1);
                }
                staminaBar.setValue(staminaBar.getValue()-take);
                yield return new WaitForSeconds(dashDur);
                canMove = true;
                isDashing = false;
                yield return new WaitForSeconds(dashCooldown);
            } else{
                isDashing = false;
                canDash = true;
                staminaBar.setValue(staminaBar.getValue()-take*400*Time.deltaTime);
                yield return new WaitForSeconds(0.1f);
            }
            canDash = true;
        }
        isDashing = false;
        yield return null;
    }

    //Collision
    void OnCollisionStay2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Enemy")){
            StartCoroutine(TakeDamage(2f));
        }
        if(col.gameObject.CompareTag("EnemyAttack")){
            StartCoroutine(TakeDamage(2f));
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        canSwap = (col.tag != "NoSwap");
    }

    void OnTriggerExit2D(Collider2D col)
    {
        canSwap = true;
    }


    //Updating Graphics
    public void UpdateAnimClipTimes(){
        AnimationClip[] rclips = ratAnimator.runtimeAnimatorController.animationClips;
        AnimationClip[] kclips = knightAnimator.runtimeAnimatorController.animationClips;
        foreach(AnimationClip clip in rclips){
            switch(clip.name){
                case "R_Idle_LR":
                    RIdle = clip.length;
                    break;
                case "R_Move_LR":
                    RMove = clip.length;
                    break;
            }
        }
        foreach(AnimationClip clip in kclips){
            switch(clip.name){
                case "K_Idle_LR":
                    KIdle = clip.length-frame_refer;
                    break;
                case "K_Move_LR":
                    KMove = clip.length-frame_refer;
                    break;
                case "K_Dash_LR":
                    KDash = clip.length-frame_refer;
                    break;
                case "K_Recover_LR":
                    KRecover = clip.length-frame_refer;
                    break;
                case "K_Attack_LR":
                    KAttack = clip.length-frame_refer;
                    break;
                case "K_HoldAttack_LR":
                    KHoldAttack = clip.length-frame_refer;
                    break;
                case "K_AttackWalk_LR":
                    KAttackWalk = clip.length-frame_refer;
                    break;
                case "K_HoldAttackWalk_LR":
                    KHoldAttackWalk = clip.length-frame_refer;
                    break;
            }
        }
    }
    private int updateSprite()
    {
        if(Time.time < lockedTimer){
            return currentState;
        }

        if(isKnightController){
            // print(isHeld && canHold);
            if(isRecovering) return lockState(K_Recover_LR,KRecover);
            if(isHeld && canHold && isMoving) {
                    print("me1");
                
                if (isAttackSoundPlaying)
                {
                    FindAnyObjectByType<AudioManager>().setSoundLooping("Attack", true);
                    FindAnyObjectByType<AudioManager>().SetPitchVolume("Attack", 1.5f);
                    FindAnyObjectByType<AudioManager>().SetSoundVolume("Attack", 1.0f);
                    FindAnyObjectByType<AudioManager>().Play("Attack");
                    isAttackSoundPlaying = true;
                }
                return lockState(K_HoldAttackWalk_LR,KAttack);
                
                }
            if(isHeld && canHold) {
                    print("me2");

                if (isAttackSoundPlaying)
                {
                    FindAnyObjectByType<AudioManager>().setSoundLooping("Attack", true);
                    FindAnyObjectByType<AudioManager>().SetPitchVolume("Attack", 1.5f);
                    FindAnyObjectByType<AudioManager>().SetSoundVolume("Attack", 1.0f);
                    FindAnyObjectByType<AudioManager>().Play("Attack");
                    isAttackSoundPlaying = true;
                }
                return lockState(K_HoldAttack_LR,KAttack);}
            
            if (!canAttack && isMoving) {
                if (!isAttackSoundPlaying)
                {
                    FindAnyObjectByType<AudioManager>().setSoundLooping("Attack", false);
                    FindAnyObjectByType<AudioManager>().SetPitchVolume("Attack", 0.5f);
                    FindAnyObjectByType<AudioManager>().SetSoundVolume("Attack", 1.0f);
                    FindAnyObjectByType<AudioManager>().Play("Attack");
                    isAttackSoundPlaying = true;
                }
                return K_AttackWalk_LR;
            }
            if (!canAttack){
                    print("me4");

                if (!isAttackSoundPlaying)
                {
                    FindAnyObjectByType<AudioManager>().setSoundLooping("Attack", false);
                    FindAnyObjectByType<AudioManager>().SetPitchVolume("Attack", 0.5f);
                    FindAnyObjectByType<AudioManager>().SetSoundVolume("Attack", 1.0f);
                    FindAnyObjectByType<AudioManager>().Play("Attack");
                    isAttackSoundPlaying = true;
                }
                return K_Attack_LR;
            } 
            isAttackSoundPlaying = false;
            FindAnyObjectByType<AudioManager>().StopByName("Attack");
            if (isDashing) return lockState(K_Dash_LR,dashDur);
            if (isMoving) return K_Move_LR;
            else return K_Idle_LR;
        } else{
            if (isMoving) return R_Move_LR;
            else return R_Idle_LR;
        }
        int lockState(int s,float t){
            lockedTimer = Time.time + t;
            return s;
        }
    }

    void regenBar(Bar_Controller Bar,float rate = 1){
        Bar.setValue(Bar.getValue()+Time.deltaTime * rate);
    }   

    public void UpdateHealth(float value)
    {
        healthBar.setValue(healthBar.getValue() - value);
        health = health - value;
        StartCoroutine(ShowDamageIndicator()); 
        StartCoroutine(TakeDamage(value));
    }

    private IEnumerator ShowDamageIndicator()
    {
        if (!isShowingDamageIndicator){
            
            isShowingDamageIndicator = true;
            
            // Get the sprite renderer for the appropriate child object
            SpriteRenderer spriteRenderer = isKnightController ? knight.GetComponent<SpriteRenderer>() : rat.GetComponent<SpriteRenderer>();

            // Save the original color
            Color originalColor = spriteRenderer.color;
            if (originalColor == damageColor) 
            {
                isShowingDamageIndicator = false;
                yield break;
            }
        

            // Set the new color
            spriteRenderer.color = damageColor;
        

            // Wait for the damage indicator duration
            float elapsedTime = 0f;
            while (elapsedTime < damageIndicatorDuration)
            {
                // Lerp between the damage color and the original color
                float t = elapsedTime / damageIndicatorDuration;
                spriteRenderer.color = Color.Lerp(damageColor, originalColor, t);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Reset the color
            spriteRenderer.color = originalColor;
            
            isShowingDamageIndicator = false;
        }
        yield return null;

    }

}
