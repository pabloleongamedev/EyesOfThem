using UnityEngine;
using TMPro;

public class Interactable : MonoBehaviour
{
    [Header("Interactable Settings")]
    [SerializeField] private string itemName = "Objeto";
    [SerializeField] private GameObject interactBarPrefab;
    [SerializeField] private Sprite icon;
    public Sprite Icon => icon;

    protected GameObject itemBar;
    protected TextMeshProUGUI textComp;
    private Canvas targetCanvas;

    void Awake()
    {
        CreateInteractBar();
    }
    protected void CreateInteractBar()
    {
        if (itemBar != null) return;

        targetCanvas = FindFirstObjectByType<Canvas>();

        if (interactBarPrefab != null && targetCanvas != null)
        {
            itemBar = Instantiate(interactBarPrefab, targetCanvas.transform);
            textComp = itemBar.GetComponentInChildren<TextMeshProUGUI>();
            if (textComp != null)
            {
                textComp.SetText("Pulsa E para recoger " + itemName);
            }
            itemBar.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (itemBar != null) itemBar.SetActive(true);

            // Pasamos la referencia al jugador directamente
            PlayerController pc = other.GetComponent<PlayerController>();
            if (pc != null) pc.SetCurrentInteractable(this);

            if (CompareTag("Door"))
            {
                textComp.SetText("Bloqueada. Necesitas una llave para desbloquear ");
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            if (itemBar != null) itemBar.SetActive(false);

            PlayerController pc = other.GetComponent<PlayerController>();
            if (pc != null) pc.SetCurrentInteractable(null);
        }
    }


    // Al ser virtual se implementa en la clase hija
    public virtual bool DoInteraction()
    {
        return false;
    }


    private void OnDestroy()
    {
        if (itemBar != null) Destroy(itemBar);
    }
}