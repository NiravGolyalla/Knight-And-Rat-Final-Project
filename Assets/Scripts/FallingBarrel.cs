using System.Collections;
using UnityEngine;

public class FallingBarrel : CNBController
{
    [SerializeField] private float fallDuration = 1f;
    [SerializeField] private float startScale = 1.5f;
    [SerializeField] private float endScale = 1f;
    [SerializeField] private GameObject hitEffectPrefab;

    private Collider2D barrelCollider;

    private void Start()
    {
        base.Start();
        barrelCollider = GetComponent<Collider2D>();
        StartCoroutine(FallAndScale());
    }

    private IEnumerator FallAndScale()
    {
        barrelCollider.enabled = false;

        float elapsedTime = 0f;
        Vector3 startLocalScale = Vector3.one * startScale;
        Vector3 endLocalScale = Vector3.one * endScale;

        while (elapsedTime < fallDuration)
        {
            transform.localScale = Vector3.Lerp(startLocalScale, endLocalScale, elapsedTime / fallDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = endLocalScale;
        StartCoroutine(InstantiateHitEffect());
        barrelCollider.enabled = true;
    }

    private IEnumerator InstantiateHitEffect()
    {
        if (hitEffectPrefab != null)
        {
            GameObject hitEffect = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(1f);
            Destroy(hitEffect);
        }
    }

    public override void GotHit()
    {
        base.GotHit();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Sword"))
        {
            GotHit();
        }
    }
}
