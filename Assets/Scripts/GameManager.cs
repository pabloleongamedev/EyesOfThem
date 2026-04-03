using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Para el componente Button
using TMPro; // Para TextMeshProUGUI

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dieText;
    [SerializeField] private Button restartButton;
    public bool isGameActive = false;

    public static GameManager Instance { get; private set; }

    void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;  
    }



    public void GameOver()
    {
        isGameActive = false;
        dieText.gameObject.SetActive(true); 
        restartButton.gameObject.SetActive(true); 
    }

    public void RestartGame()
    {
        
        // Carga la escena del menú
        SceneManager.LoadScene("Menu_Inicio");
        isGameActive = true;
        dieText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);   
        Debug.Log("reinicidaso:   "+isGameActive);
    }
}