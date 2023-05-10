using System.Collections;
using UnityEngine;

public class CNBController : MonoBehaviour
{
    public Animator animator;
    private SpriteRenderer spriteRenderer;
    public GameObject catnip;
    public GameObject c;
    [SerializeField]private bool hasCatnip;
    public string catnipTag = "catnip";

    public void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    virtual public void GotHit(){
        animator.CrossFade("break",0,0);
        FindAnyObjectByType<AudioManager>().Play("BarrelBreak");
        GetComponent<Collider2D>().enabled = false;
        StartCoroutine(FadeOut());
    }

    public IEnumerator FadeOut()
    {
        if(hasCatnip){
            c.SetActive(false);
            GameObject newCatnip = Instantiate(catnip, transform.position, Quaternion.identity);
            newCatnip.tag = catnipTag;
        }
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length*2);
        // Get the starting alpha value
        float alpha = spriteRenderer.color.a;
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
        Destroy(gameObject);
    }
}
