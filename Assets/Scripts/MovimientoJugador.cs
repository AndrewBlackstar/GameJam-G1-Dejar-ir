using UnityEngine;

public class MoviminetoJugador : MonoBehaviour
{
    [SerializeField] private float velocidadMovimiento;
    float velocidadBase;
    [SerializeField] private Vector2 direccion;
    private Rigidbody2D rb2D;
    private Animator animator;
    HealthManager healthManager;

    [SerializeField]GameManagerHelper gameManagerHelper;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        healthManager = GetComponent<HealthManager>();
        gameManagerHelper = GameObject.FindAnyObjectByType<GameManagerHelper>();

        if (gameManagerHelper == null)
        {
            Debug.LogError("❌ gameManagerHelper no encontrado en la escena.");
        }


        velocidadBase = velocidadMovimiento;
    }

    void Update()
    {
        movePlayer();
        ApplyRun();
    }

    void movePlayer()
    {
        // Captura la entrada del jugador
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Prioriza el movimiento horizontal sobre el vertical
        if (horizontal != 0)
        {
            direccion = new Vector2(horizontal, 0).normalized; // Solo movimiento horizontal
        }
        else if (vertical != 0)
        {
            direccion = new Vector2(0, vertical).normalized; // Solo movimiento vertical
        }
        else
        {
            direccion = Vector2.zero; // No hay movimiento
        }

        // Actualiza los parámetros del Animator
        if (animator != null)
        {
            animator.SetFloat("MovimientoX", direccion.x);
            animator.SetFloat("MovimientoY", direccion.y);
            animator.SetBool("EstaMoviendose", direccion.magnitude > 0);
        }

        // Voltea el sprite si se mueve hacia la izquierda
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
        // Mueve al jugador
        rb2D.MovePosition(rb2D.position + direccion * velocidadMovimiento * Time.deltaTime);
    }

    void ApplyRun()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
           velocidadMovimiento = velocidadBase *3f;
        }
        else
        {
            velocidadMovimiento = velocidadBase * 1;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("enemigo detectado por collision");
            healthManager.Takedamage(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D Other)
    {
        if (Other.CompareTag("ReachPoint"))
        {

            Debug.Log("llega al final");
            GameManager.GameInstance.GameOverWin();
        }
    }

}