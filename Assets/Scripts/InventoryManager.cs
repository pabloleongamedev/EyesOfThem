using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [Header("Configuración")]
    [SerializeField] private int maxSlots = 2;

    [Header("UI References")]
    [SerializeField] private GameObject inventoryScreen;
    [SerializeField] private Button inventorySlot1;
    [SerializeField] private Button inventorySlot2;
    [SerializeField] private Button inventoryButton1;
    [SerializeField] private Button inventoryButton2;
    [SerializeField] private AudioClip interctButton;
    [SerializeField] private AudioClip drink;
    [SerializeField] private AudioClip heal;


    private int agregados = 0;
    private bool isActiveInventory = false;
    private Sprite defaulIcon;
    private List<Interactable> items = new List<Interactable>();

    void Awake()
    {

        defaulIcon = inventorySlot1.image.sprite;

        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public List<Interactable> GetItems()
    {
        return items;
    }

    public bool AddItem(Interactable newItem)
    {
        if (items.Count < maxSlots)
        {
            if (items.Contains(newItem)) return false;

            items.Add(newItem);
            agregados++;

            if (agregados == 1)
            {
                UpdateSlotUI(inventorySlot1, inventoryButton1, newItem.Icon);
            }
            else
            {
                UpdateSlotUI(inventorySlot2, inventoryButton2, newItem.Icon);
            }
            return true;
        }
        return false;
    }

    private void UpdateSlotUI(Button slot, Button actionBtn, Sprite icon)
    {
        slot.image.sprite = icon;
        slot.enabled = true;
        actionBtn.image.sprite = icon;
        actionBtn.enabled = true;
    }

    public void RemoveItem(Interactable item)
    {
        if (items.Contains(item))
        {
            // Identificamos el índice para saber qué imagen resetear
            int index = items.IndexOf(item);

            items.Remove(item);
            agregados--;

            // Reseteamos el icono al defaulIcon según el slot
            ClearSlotUI(index);
        }
    }

    private void ClearSlotUI(int index)
    {
        if (index == 0)
        {
            inventorySlot1.image.sprite = defaulIcon;
            inventoryButton1.image.sprite = defaulIcon;
        }
        else if (index == 1)
        {
            inventorySlot2.image.sprite = defaulIcon;
            inventoryButton2.image.sprite = defaulIcon;
        }
    }

    public void UseItem(int slot)
    {
        List<Interactable> inventory = GetItems();

        if (slot < 0 || slot >= inventory.Count) return;

        Interactable selectedItem = inventory[slot];
        AudioManager.Instance.PlaySFX(interctButton, 0.3f);

        if (selectedItem.tag == "Stamina")
        {
            StaminaItem staminaItem = selectedItem as StaminaItem;
            PlayerStats.Instance.HealStamina(staminaItem.GetStaminaAmount());
            RemoveItem(selectedItem);
            AudioManager.Instance.PlaySFX(drink, 1f);
        }
        else if (selectedItem.tag == "Health")
        {
            HealthItem healthItem = selectedItem as HealthItem;
            PlayerHealth.Instance.Heal(healthItem.GetHealAmount());
            AudioManager.Instance.PlaySFX(heal, 0.6f);
            RemoveItem(selectedItem);
        }
        else if (selectedItem.tag == "Key")
        {
            Debug.Log("No puedes usarla acá");
        }

        ResetInventoryButtons();
    }

    private void ResetInventoryButtons()
    {
        inventoryButton1.enabled = true;
        inventoryButton2.enabled = true;
    }

    public bool SetVisibilityInventory()
    {
        if (isActiveInventory)
        {
            inventoryScreen.SetActive(false);
            isActiveInventory = false;
        }
        else
        {
            inventoryScreen.SetActive(true);
            isActiveInventory = true;
        }
        return isActiveInventory;
    }
}