using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    [Header("Stamina Settings")]
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float staminaSprintDrain = 20f;
    [SerializeField] private float staminaWalkDrain = 5f;
    private float consumeStamina;
    private float currentStamina;



    void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }

        currentStamina = maxStamina;
    }

    public void HealStamina(float amount)
    {
        currentStamina += amount;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

   
    }

    public void ConsumeStamina(bool isSprinting)
    {
        if (isSprinting) { consumeStamina= staminaSprintDrain; }
        else { consumeStamina=staminaWalkDrain; }

        currentStamina -= consumeStamina * Time.deltaTime;
        currentStamina = Mathf.Max(currentStamina, 0);
    }

    public bool CanSprint()
    {

        return currentStamina > 0;
    }

    public float CurrentStamina()
    {
        return currentStamina;
    }
    public float CurrentStaminaNormalized()
    {
        return currentStamina/maxStamina;
    }
}