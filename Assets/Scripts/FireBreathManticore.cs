using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBreathManticore : MonoBehaviour
{
    public Animator animator;
    private Manticore manticore;
    private bool stage2Started = false;
    public GameObject targetObject;
    public float runSpeed = 10f;

    void Start()
    {
        manticore = GetComponent<Manticore>();
    }

    void Update()
    {
        if (manticore.health <= 60 && !stage2Started)
        {
            manticore.stage = 2;
            stage2Started = true;
            StartCoroutine(Stage2Sequence());
        }
    }

    IEnumerator Stage2Sequence()
    {
        // Check if Manticore is initially facing right and store this information
        bool initiallyFacingRight = manticore.IsFacingRight();

        // If Manticore is initially facing left, flip the sprite before the tired animation
        if (!initiallyFacingRight)
        {
            manticore.Flip();
        }

        animator.SetTrigger("Tired");

        yield return new WaitForSeconds(4f);

        // If Manticore is initially facing right, flip the sprite before the run animation
        if (initiallyFacingRight)
        {
            manticore.Flip();
        }

        animator.SetBool("Running", true);
        yield return StartCoroutine(RunTowardsTarget());

        // If Manticore was initially facing right, flip the sprite again
        if (initiallyFacingRight)
        {
            manticore.Flip();
        }
        animator.SetTrigger("FireBreath");
    }

    IEnumerator RunTowardsTarget()
    {
        // Wait for the running animation to start
        yield return new WaitForSeconds(0.1f);

        // Get the animator state info
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        float t = 0f;
        Vector3 startPosition = transform.position;
        float distance = Vector2.Distance(startPosition, targetObject.transform.position);

        while (Vector2.Distance(transform.position, targetObject.transform.position) > 0.1f)
        {
            // Check if the running animation is playing
            if (stateInfo.IsName("ManticoreRun"))
            {
                t += Time.deltaTime * runSpeed / distance;
                transform.position = Vector2.Lerp(startPosition, targetObject.transform.position, t);
            }
            else
            {
                // Update the animator state info
                stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            }

            yield return null;
        }
    }



}
