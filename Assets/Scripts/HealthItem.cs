using UnityEngine;
public class HealthItem : Interactable
{
    [SerializeField] private float healAmount = 25f;

    public float GetHealAmount()
    {
        return healAmount;
    }

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
}