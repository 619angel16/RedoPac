using UnityEngine;

// Define el orden de ejecución de este script. Se ejecutará antes que otros scripts con el valor predeterminado (0).
[DefaultExecutionOrder(-10)]

// Requiere que el GameObject tenga el componente Movement. Esto asegura que no se pueda usar sin ese componente.
[RequireComponent(typeof(Movement))]
public class Ghost : MonoBehaviour
{
    // Propiedades públicas de solo lectura para varios comportamientos del fantasma.
    public Movement movement { get; private set; } // Componente responsable del movimiento.
    public GhostHome home { get; private set; } // Componente que define el comportamiento en "casa".
    public GhostScatter scatter { get; private set; } // Componente para el modo de dispersión (scatter).
    public GhostChase chase { get; private set; } // Componente para el modo de persecución (chase).
    public GhostFrightened frightened { get; private set; } // Componente para el estado de "miedo" (frightened).

    // El comportamiento inicial del fantasma al comenzar el juego.
    public GhostBehavior initialBehavior;

    // Referencia al objetivo del fantasma (probablemente el jugador o una posición).
    public Transform target;

    // Puntos otorgados al jugador por comer este fantasma.
    public int points = 200;

    // Método llamado al inicializar el script.
    private void Awake()
    {
        // Obtiene y almacena referencias a los componentes asociados al GameObject.
        movement = GetComponent<Movement>();
        home = GetComponent<GhostHome>();
        scatter = GetComponent<GhostScatter>();
        chase = GetComponent<GhostChase>();
        frightened = GetComponent<GhostFrightened>();
    }

    // Método llamado al comienzo del juego.
    private void Start()
    {
        // Restablece el estado del fantasma a su configuración inicial.
        ResetState();
    }

    // Restablece el estado del fantasma.
    public void ResetState()
    {
        // Activa el GameObject por si estaba desactivado.
        gameObject.SetActive(true);

        // Restablece el estado del movimiento.
        movement.ResetState();

        // Desactiva los comportamientos no iniciales.
        frightened.Disable();
        chase.Disable();
        scatter.Enable();

        // Si el comportamiento inicial no es el de "casa", lo desactiva.
        if (home != initialBehavior)
        {
            home.Disable();
        }

        // Si hay un comportamiento inicial definido, lo habilita.
        if (initialBehavior != null)
        {
            initialBehavior.Enable();
        }
    }

    // Establece una nueva posición para el fantasma, manteniendo el valor de la posición Z (profundidad).
    public void SetPosition(Vector3 position)
    {
        position.z = transform.position.z; // Asegura que la profundidad (z) no cambie.
        transform.position = position; // Asigna la nueva posición.
    }

    // Detecta colisiones con otros objetos en el juego.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Comprueba si el fantasma colisionó con "Pacman".
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            // Si el fantasma está en modo "frightened", el jugador lo come.
            if (frightened.enabled)
            {
                GameManager.Instance.GhostEaten(this); // Llama al método para procesar al fantasma comido.
            }
            else
            {
                GameManager.Instance.PacmanEaten(); // Si no está asustado, Pacman es comido.
            }
        }
    }
}
