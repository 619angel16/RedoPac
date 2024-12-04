using UnityEngine;

// Requiere que el objeto al que se adjunta este script tenga un componente de tipo Rigidbody2D.
[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    public float speed = 8f; // Velocidad de movimiento base.
    public float speedMultiplier = 1f; // Multiplicador de velocidad para ajustar la velocidad de movimiento.
    public Vector2 initialDirection; // Dirección inicial de movimiento.
    public LayerMask obstacleLayer; // Capa de obstáculos para la detección de colisiones.

    public Rigidbody2D rb { get; private set; } // Referencia al componente Rigidbody2D.
    public Vector2 direction { get; private set; } // Dirección actual de movimiento.
    public Vector2 nextDirection { get; private set; } // Dirección de movimiento pendiente.
    public Vector3 startingPosition { get; private set; } // Posición inicial del objeto.

    private void Awake()
    {
        // Obtiene y guarda la referencia al Rigidbody2D y la posición inicial.
        rb = GetComponent<Rigidbody2D>();
        startingPosition = transform.position;
    }

    private void Start()
    {
        // Resetea el estado del objeto al inicio.
        ResetState();
    }

    public void ResetState()
    {
        // Reinicia los valores de las variables de estado.
        speedMultiplier = 1f;
        direction = initialDirection;
        nextDirection = Vector2.zero;
        transform.position = startingPosition;
        rb.isKinematic = false; // Asegura que el Rigidbody2D no sea cinemático.
        enabled = true; // Activa el script.
    }

    private void Update()
    {
        // Cambia la dirección de movimiento si hay una dirección pendiente.
        if (nextDirection != Vector2.zero)
        {
            SetDirection(nextDirection);
        }
    }

    private void FixedUpdate()
    {
        // Calcula la posición y la cantidad de desplazamiento en cada actualización física.
        Vector2 position = rb.position;
        Vector2 translation = speed * speedMultiplier * Time.fixedDeltaTime * direction;
        rb.MovePosition(position + translation); // Mueve el Rigidbody2D.
    }

    public void SetDirection(Vector2 direction, bool forced = false)
    {
        // Establece la dirección de movimiento solo si la celda en esa dirección está libre.
        // Si no, se guarda como la siguiente dirección para aplicarla cuando sea posible.
        if (forced || !Occupied(direction))
        {
            this.direction = direction;
            nextDirection = Vector2.zero; // Limpia la dirección pendiente.
        }
        else
        {
            nextDirection = direction; // Guarda la dirección pendiente.
        }
    }

    public bool Occupied(Vector2 direction)
    {
        // Detecta si hay un obstáculo en la dirección especificada.
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.75f, 0f, direction, 1.5f, obstacleLayer);
        return hit.collider != null; // Devuelve true si se detecta un obstáculo.
    }
}
