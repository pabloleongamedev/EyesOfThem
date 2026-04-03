using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance { get; private set; }

    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private AudioClip damageSong;
    private float currentHealth;
    private bool isDead;

    // Eventos estáticos para acceder sin instanciar
    public static event Action<float> OnHealthChanged;

    void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }

        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {

        if (isDead || amount <= 0) return;

        currentHealth -= amount;
        AudioManager.Instance.PlaySFX(damageSong, 0.7f);
        UpdateHealth();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (isDead || amount <= 0) return;

        currentHealth += amount;
        UpdateHealth();
    }

    private void UpdateHealth()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (OnHealthChanged != null)
        {
            OnHealthChanged.Invoke(currentHealth / maxHealth);
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;
    }
    public bool IsDead()
    {
        return isDead;
    }
    public float CurrentHealth()
    {
        return currentHealth;
    }

    public float GetHealthNormalized()
    {
        return currentHealth / maxHealth;
    }
}