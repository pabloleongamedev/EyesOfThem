using UnityEngine;
using UnityEngine.UI;

public class SelectedItemSlot : MonoBehaviour
{
    private Button button;
    private InventoryManager inventoryManager;

    public int slot;
    
    void Start()
    {
        button = GetComponent<Button>();
        inventoryManager = FindFirstObjectByType<InventoryManager>();

        button.onClick.AddListener(SetDifficulty);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetDifficulty()
    {
        Debug.Log("Difficulty set to: " + button.name);
        inventoryManager.UseItem(slot);
    }

}
