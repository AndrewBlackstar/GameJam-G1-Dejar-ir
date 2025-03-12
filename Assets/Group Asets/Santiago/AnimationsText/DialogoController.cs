using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogoController : MonoBehaviour
{
    [SerializeField] private TextMeshPro textoDialogo; // Referencia al componente Text
    [SerializeField] private Animator animator; // Referencia al Animator del Panel
    [SerializeField] private string[] lineasDialogo; // Array de líneas de diálogo
    [SerializeField] private float velocidadTexto = 0.05f; // Velocidad de aparición del texto

    private int indiceLinea = 0; // Índice de la línea actual
    private bool dialogoActivo = false; // Estado del diálogo

    void Start()
    {
        // Inicia el diálogo automáticamente al comenzar la escena
        IniciarDialogo();
    }

    void Update()
    {
        // Avanza al siguiente diálogo al presionar un botón (por ejemplo, el clic izquierdo del mouse)
        if (dialogoActivo && Input.GetMouseButtonDown(0))
        {
            SiguienteLinea();
        }
    }

    public void IniciarDialogo()
    {
        dialogoActivo = true;
        animator.SetBool("Mostrar", true); // Activa la animación de entrada
        indiceLinea = 0;
        StartCoroutine(MostrarTexto());
    }

    private System.Collections.IEnumerator MostrarTexto()
    {
        textoDialogo.text = ""; // Limpia el texto
        foreach (char letra in lineasDialogo[indiceLinea].ToCharArray())
        {
            textoDialogo.text += letra; // Añade una letra a la vez
            yield return new WaitForSeconds(velocidadTexto); // Espera antes de añadir la siguiente letra
        }
    }

    private void SiguienteLinea()
    {
        if (indiceLinea < lineasDialogo.Length - 1)
        {
            indiceLinea++;
            StartCoroutine(MostrarTexto()); // Muestra la siguiente línea
        }
        else
        {
            dialogoActivo = false;
            animator.SetBool("Mostrar", false); // Activa la animación de salida
        }
    }
}