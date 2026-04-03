using UnityEngine;
using System.Collections.Generic;

public class Door : Interactable
{
    [Header("Configuración de Puerta")]
    [SerializeField] private GameObject keyPrefab; // Debe coincidir con el keyName de KeyItem
    [SerializeField] private bool destroyKeyOnUse = true;
    [SerializeField] private AudioClip doorLocked;
    [SerializeField] private AudioClip unlock;
    private string requiredKeyName;

    void Awake()
    {
        CreateInteractBar();

        if (keyPrefab != null)
        {

            KeyItem scriptLlave = keyPrefab.GetComponent<KeyItem>();
            if (scriptLlave != null)
            {
                requiredKeyName = scriptLlave.GetKeyName();
            }
            else
            {
                requiredKeyName = keyPrefab.name;
            }
        }

    }
    public override bool DoInteraction()
    {
        if (CheckForKey())
        {
            OpenDoor();
            return true;
        }
        else
        {
            if (textComp != null)
            {
                textComp.SetText("Bloqueada. Necesitas: " + requiredKeyName);
                AudioManager.Instance.PlaySFX(doorLocked, 0.5f);

                return true;
            }
        }
        return false;
    }

    private bool CheckForKey()
    {

        List<Interactable> inventory = InventoryManager.Instance.GetItems();

        foreach (Interactable item in inventory)
        {
            if (item is KeyItem)
            {
                KeyItem llaveEncontrada = item as KeyItem;
                if (llaveEncontrada.GetKeyName() == requiredKeyName)
                {
                    if (destroyKeyOnUse)
                    {
                        InventoryManager.Instance.RemoveItem(item);
                    }
                    return true;
                }
            }
        }
        return false;
    }

    private void OpenDoor()
    {
        textComp.SetText("Puerta desbloqueada!");
        AudioManager.Instance.PlaySFX(unlock, 1f);
        Destroy(gameObject, 2f);
    }
}