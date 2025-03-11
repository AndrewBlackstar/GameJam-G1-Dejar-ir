using UnityEngine;

public class MoviminetoJugador : MonoBehaviour
{

    
    
    
    [SerializeField] private float velocidadMovimiento;
    [SerializeField] private Vector2 direccion;
    private Rigidbody2D rb2D;
    private Animator animator;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        direccion = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized; 

                if (animator != null)
        {
            animator.SetFloat("MovimientoX", direccion.x);
            animator.SetFloat("MovimientoY", direccion.y);
            animator.SetBool("EstaMoviendose", direccion.magnitude > 0);
        }
        if (direccion.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1); // Voltea el sprite
        }
        else if (direccion.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1); // Restaura la escala normal
        }
    }
    
    void FixedUpdate()
    {
        rb2D.MovePosition(rb2D.position + direccion * velocidadMovimiento * Time.deltaTime);
    }

}
