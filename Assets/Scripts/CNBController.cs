using System.Collections;
using UnityEngine;

public class CNBController : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject knight;
    public GameObject catnip;
    private bool wasHit;
    private bool isCollding;

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (knight.activeSelf && isCollding && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)))
        {
            animator.SetFloat("isHit", 1);
            wasHit = true;
        }

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("break") && wasHit)
        {
            StartCoroutine(FadeOut());
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        isCollding = true;
    }
    private void OnCollisionExit2D(Collision2D collision) {
        isCollding = false;
    }

    private IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length*2);
        // Get the starting alpha value
        float alpha = spriteRenderer.color.a;
        
        catnip.SetActive(true);
        // Loop until the sprite is completely transparent
        while (alpha > 0f)
        {
            // Reduce the alpha value over time
            alpha -= Time.deltaTime;

            // Update the sprite color with the new alpha value
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);

            yield return null;
        }
        
        // Disable the GameObject once the sprite is completely transparent
        gameObject.SetActive(false);
    }
}
