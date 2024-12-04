using System.Collections;
using UnityEngine;

public class GhostHome : GhostBehavior
{
    // Referencias a los puntos de inicio y salida del hogar del fantasma.
    public Transform inside; // Posición interna del hogar.
    public Transform outside; // Posición externa del hogar.

    // Se llama al habilitar el comportamiento, deteniendo cualquier transición anterior.
    private void OnEnable()
    {
        StopAllCoroutines(); // Detiene cualquier corrutina en ejecución cuando el objeto se habilita.
    }

    // Se llama al deshabilitar el comportamiento, iniciando la transición de salida si el objeto sigue activo.
    private void OnDisable()
    {
        // Verifica que el objeto esté activo para evitar errores al destruirlo.
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(ExitTransition()); // Inicia la corrutina de transición de salida.
        }
    }

    // Maneja las colisiones del fantasma con obstáculos (ej. paredes) dentro del hogar.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Invierte la dirección del movimiento al chocar con un obstáculo.
        if (enabled && collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            ghost.movement.SetDirection(-ghost.movement.direction);
        }
    }

    // Corrutina que gestiona la transición de salida del hogar del fantasma.
    private IEnumerator ExitTransition()
    {
        // Desactiva el movimiento mientras se anima manualmente la posición.
        ghost.movement.SetDirection(Vector2.up, true); // Mueve el fantasma hacia arriba.
        ghost.movement.rb.isKinematic = true; // Establece el Rigidbody como cinemático para evitar físicas automáticas.
        ghost.movement.enabled = false; // Desactiva el componente de movimiento para controlar la posición manualmente.

        Vector3 position = transform.position; // Guarda la posición actual del objeto.

        float duration = 0.5f; // Duración de la animación.
        float elapsed = 0f; // Tiempo transcurrido.

        // Animación de movimiento hacia el punto de inicio (inside).
        while (elapsed < duration)
        {
            ghost.SetPosition(Vector3.Lerp(position, inside.position, elapsed / duration)); // Interpola la posición.
            elapsed += Time.deltaTime;
            yield return null;
        }

        elapsed = 0f;

        // Animación de salida del hogar hacia la posición externa (outside).
        while (elapsed < duration)
        {
            ghost.SetPosition(Vector3.Lerp(inside.position, outside.position, elapsed / duration)); // Interpola la posición.
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Elige una dirección aleatoria (izquierda o derecha) y reactiva el movimiento.
        ghost.movement.SetDirection(new Vector2(Random.value < 0.5f ? -1f : 1f, 0f), true);
        ghost.movement.rb.isKinematic = false; // Reactiva la física del Rigidbody.
        ghost.movement.enabled = true; // Activa el componente de movimiento.
    }
}
