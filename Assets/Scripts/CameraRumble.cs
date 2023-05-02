using System.Collections;
using UnityEngine;

public class CameraRumble : MonoBehaviour
{
    [SerializeField] private float shakeIntensity = 2f;
    [SerializeField] private float shakeDuration = 0.2f;

    public void ShakeScreen()
    {
        Debug.Log("were here boys");
        StartCoroutine(ScreenRumble(shakeIntensity, shakeDuration));
    }

    private IEnumerator ScreenRumble(float shakeIntensity, float shakeDuration)
    {

        float elapsedTime = 0f;
        Vector3 originalPos = transform.position;
        Debug.Log("were here boys");
        while (elapsedTime < shakeDuration)
        {
            Debug.Log("yeeee");
            float x = Random.Range(-1f, 1f) * shakeIntensity;
            float y = Random.Range(-1f, 1f) * shakeIntensity;

            transform.position = originalPos + new Vector3(x, y, 0f);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Reset camera position
        transform.position = originalPos;
    }
}
