using UnityEngine;
public class KeyItem : Interactable
{
    [SerializeField] private string keyName = "Llave 1";

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
    public string GetKeyName()
    {
        return keyName;
    }
}