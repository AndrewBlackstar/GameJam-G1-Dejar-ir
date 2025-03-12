using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScenari : MonoBehaviour
{
    [SerializeField] private string escenario2; // Nombre de la escena a cargar

    [SerializeField] private Image fadeImage; // Imagen para el fade
    [SerializeField] private float fadeSpeed = 1f; // Velocidad del fade

    private bool iniciarFade = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica si el objeto que entra en el trigger es el jugador
        if (collision.CompareTag("Player"))
        {
            SceneManager.LoadScene("escenario 2");
            // Carga la nueva escena
        }
    }
}

