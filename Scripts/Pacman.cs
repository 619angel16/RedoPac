using UnityEngine;

[RequireComponent(typeof(Movement))]
public class Pacman : MonoBehaviour
{
    [SerializeField]
    private AnimatedSprite deathSequence; // Referencia a la secuencia de animación que se reproduce al morir.
    private SpriteRenderer spriteRenderer; // Componente para renderizar el sprite de Pac-Man.
    private CircleCollider2D circleCollider; // Collider para la detección de colisiones.
    private Movement movement; // Componente de movimiento para controlar el movimiento de Pac-Man.

    private void Awake()
    {
        // Inicializa las referencias a los componentes necesarios.
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();
        movement = GetComponent<Movement>();
    }

    private void Update()
    {
        // Captura la entrada del jugador para cambiar la dirección de movimiento.
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
            movement.SetDirection(Vector2.up); // Mueve a Pac-Man hacia arriba.
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
            movement.SetDirection(Vector2.down); // Mueve a Pac-Man hacia abajo.
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
            movement.SetDirection(Vector2.left); // Mueve a Pac-Man hacia la izquierda.
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
            movement.SetDirection(Vector2.right); // Mueve a Pac-Man hacia la derecha.
        }

        // Calcula el ángulo de rotación basado en la dirección de movimiento y rota a Pac-Man para que mire en esa dirección.
        float angle = Mathf.Atan2(movement.direction.y, movement.direction.x);
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
    }

    public void ResetState()
    {
        // Reinicia el estado de Pac-Man y vuelve a activar sus componentes.
        enabled = true;
        spriteRenderer.enabled = true;
        circleCollider.enabled = true;
        deathSequence.enabled = false; // Oculta la secuencia de muerte.
        movement.ResetState(); // Llama al método de reinicio de estado de `Movement`.
        gameObject.SetActive(true); // Activa el objeto de Pac-Man.
    }

    public void DeathSequence()
    {
        // Maneja la secuencia de muerte de Pac-Man.
        enabled = false; // Desactiva el script de Pac-Man.
        spriteRenderer.enabled = false; // Oculta el sprite de Pac-Man.
        circleCollider.enabled = false; // Desactiva el collider de Pac-Man.
        movement.enabled = false; // Desactiva el movimiento de Pac-Man.
        deathSequence.enabled = true; // Activa la secuencia de animación de muerte.
        deathSequence.Restart(); // Reinicia la animación de muerte.
    }
}
