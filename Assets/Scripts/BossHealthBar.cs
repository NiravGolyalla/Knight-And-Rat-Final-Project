using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BossHealthBar : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private Image flashBar; // This is an additional image for flashing
    private float maxHealth = 100f;
    public Color flashColor = Color.white;
    public float flashDuration = 0.5f;

    private void Start()
    {
        healthBar.type = Image.Type.Filled;
        healthBar.fillMethod = Image.FillMethod.Horizontal;
        healthBar.fillOrigin = (int)Image.OriginHorizontal.Left;

        flashBar.type = Image.Type.Filled;
        flashBar.fillMethod = Image.FillMethod.Horizontal;
        flashBar.fillOrigin = (int)Image.OriginHorizontal.Left;
        flashBar.color = Color.white;
        flashBar.enabled = false;
    }

    public void SetMaxHealth(float health)
    {
        maxHealth = health;
    }

    public void SetHealth(float health)
    {
        float previousFill = healthBar.fillAmount;
        float currentFill = health / maxHealth;

        healthBar.fillAmount = currentFill;

        if (currentFill < previousFill)
        {
            StartCoroutine(FlashBar(previousFill));
        }
    }

    IEnumerator FlashBar(float startFill)
    {
        flashBar.fillAmount = startFill;
        flashBar.enabled = true;

        while (flashBar.fillAmount > healthBar.fillAmount)
        {
            flashBar.fillAmount -= Time.deltaTime / flashDuration;
            yield return null;
        }

        flashBar.enabled = false;
    }
}
