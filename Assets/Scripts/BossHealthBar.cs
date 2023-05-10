using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BossHealthBar : MonoBehaviour
{
    private Image healthBar;
    private float maxHealth = 100f;
    public Color damagedColor = Color.white;
    
    void Start()
    {
        healthBar = GetComponent<Image>();
    }

    public void SetMaxHealth(float health)
    {
        maxHealth = health;
    }

    public void SetHealth(float health)
    {
        healthBar.fillAmount = health / maxHealth;
    }
}