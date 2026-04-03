using Unity.VisualScripting;
using UnityEngine;

// Ahora hereda de Interactable
public class StaminaItem : Interactable
{
    [Header("Settings")]
    [SerializeField] private float staminaRestoreAmount = 30f;


    public override bool DoInteraction()
    {

        if (InventoryManager.Instance.AddItem(this))
        {
            itemBar.SetActive(false);
            gameObject.SetActive(false);
            return true;
        }
        return false;
    }
    public float GetStaminaAmount()
    {
        return staminaRestoreAmount;
    }
}