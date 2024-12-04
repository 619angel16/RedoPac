using UnityEngine;

public class GhostFrightened : GhostBehavior
{
    // Referencias a los diferentes componentes visuales del fantasma.
    public SpriteRenderer body; // SpriteRenderer para el cuerpo normal del fantasma.
    public SpriteRenderer eyes; // SpriteRenderer para los ojos.
    public SpriteRenderer blue; // SpriteRenderer para el estado asustado (azul).
    public SpriteRenderer white; // SpriteRenderer para el parpadeo (blanco) al final del estado asustado.

    private bool eaten; // Indica si el fantasma ha sido comido.

    // Habilita el estado "asustado" por una duración específica.
    public override void Enable(float duration)
    {
        base.Enable(duration);

        // Cambia las apariencias del fantasma para el estado asustado.
        body.enabled = false;
        eyes.enabled = false;
        blue.enabled = true;
        white.enabled = false;

        // Programa el inicio del parpadeo a la mitad de la duración del estado.
        Invoke(nameof(Flash), duration / 2f);
    }

    // Deshabilita el estado "asustado" y restablece las apariencias.
    public override void Disable()
    {
        base.Disable();

        body.enabled = true;
        eyes.enabled = true;
        blue.enabled = false;
        white.enabled = false;
    }

    // Lógica cuando el fantasma es comido por Pacman.
    private void Eaten()
    {
        eaten = true;

        // Envía al fantasma a su casa y habilita su movimiento hacia ella.
        ghost.SetPosition(ghost.home.inside.position);
        ghost.home.Enable(duration);

        // Cambia las apariencias para mostrar solo los ojos.
        body.enabled = false;
        eyes.enabled = true;
        blue.enabled = false;
        white.enabled = false;
    }

    // Activa el parpadeo (azul/blanco) indicando que el estado asustado está por terminar.
    private void Flash()
    {
        if (!eaten)
        {
            blue.enabled = false;
            white.enabled = true;
            white.GetComponent<AnimatedSprite>().Restart();
        }
    }

    // Configura el estado inicial al habilitar este componente.
    private void OnEnable()
    {
        blue.GetComponent<AnimatedSprite>().Restart(); // Reinicia la animación de "azul asustado".
        ghost.movement.speedMultiplier = 0.5f; // Reduce la velocidad del fantasma al 50%.
        eaten = false;
    }

    // Restablece los valores predeterminados al deshabilitar este componente.
    private void OnDisable()
    {
        ghost.movement.speedMultiplier = 1f; // Restablece la velocidad normal del fantasma.
        eaten = false;
    }

    // Lógica al entrar en contacto con un nodo del laberinto.
    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>();

        if (node != null && enabled)
        {
            Vector2 direction = Vector2.zero;
            float maxDistance = float.MinValue;

            // Busca la dirección que aleje más al fantasma de Pacman.
            foreach (Vector2 availableDirection in node.availableDirections)
            {
                Vector3 newPosition = transform.position + new Vector3(availableDirection.x, availableDirection.y);
                float distance = (ghost.target.position - newPosition).sqrMagnitude;

                if (distance > maxDistance)
                {
                    direction = availableDirection;
                    maxDistance = distance;
                }
            }

            ghost.movement.SetDirection(direction); // Actualiza la dirección del movimiento del fantasma.
        }
    }

    // Lógica al colisionar con Pacman.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            // Si está asustado, el fantasma es "comido".
            if (enabled)
            {
                Eaten();
            }
        }
    }
}
