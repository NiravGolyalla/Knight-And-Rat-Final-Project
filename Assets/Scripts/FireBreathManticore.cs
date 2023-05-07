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
            stage2Started = true;
            StartCoroutine(Stage2Sequence());
        }
    }

    IEnumerator Stage2Sequence()
    {
        animator.SetTrigger("Tired");

        yield return new WaitForSeconds(4f);

        animator.SetBool("Running", true);

        while (Vector2.Distance(transform.position, targetObject.transform.position) > 0.1f)
        {
            Vector2 direction = (targetObject.transform.position - transform.position).normalized;
            transform.position += new Vector3(direction.x, direction.y, 0) * runSpeed * Time.deltaTime;
            yield return null;
        }

        manticore.Flip();

        animator.SetTrigger("FireBreath");
    }
}
