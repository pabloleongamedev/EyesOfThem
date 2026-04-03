using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Sliders")]
    [SerializeField] private Slider healthFill;
    [SerializeField] private Slider staminaFill;
    [SerializeField] public GameObject inventoryScreen;
    [SerializeField] public GameObject menuScreen;

    private bool isActiveMenu = false;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void UpdateHealth()
    {
        if (healthFill != null)
        {
            healthFill.value = PlayerHealth.Instance.GetHealthNormalized();
            if (PlayerHealth.Instance.GetHealthNormalized() == 0)
            {
                GameManager.Instance.GameOver();
            }
        }
    }

    private void UpdateStamina()
    {
        if (staminaFill != null)
        {
            staminaFill.value = PlayerStats.Instance.CurrentStaminaNormalized();
        }
    }

    private void UpdateKeys(int cantidad)
    {
        if (cantidad != 0)
        {
            // Muestra la llave solo si el contador es mayor a 0
            gameObject.SetActive(true);
        }
    }
    void Update()
    {
        UpdateStamina();
        UpdateHealth();
    }

    public bool SetVisibilityMenu()
    {
        if (isActiveMenu)
        {
            menuScreen.SetActive(false);
            isActiveMenu = false;
        }
        else
        {
            menuScreen.SetActive(true);
            isActiveMenu = true;
        }
        return isActiveMenu;
    }
    public void CloseMenuExplicitly()
    {
        menuScreen.SetActive(false);
        isActiveMenu = false;
    }

}