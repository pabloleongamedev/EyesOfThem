using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    public void JugarJuego()
    {
        SceneManager.LoadScene("Nivel 1"); // Carga la escena del juego
    }

    public void SalirJuego()
    {
        Debug.Log("Saliendo..."); // En editor solo se ve en consola
        Application.Quit();
    }
}