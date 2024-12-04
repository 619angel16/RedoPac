using UnityEngine;

public class GhostChase : GhostBehavior
{
    private void OnDisable()
    {
        // Cuando el comportamiento de persecución se desactiva, activa el comportamiento de dispersión del fantasma.
        ghost.scatter.Enable();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Intenta obtener el componente "Node" del objeto con el que colisionó.
        Node node = other.GetComponent<Node>();

        // No realiza ninguna acción si:
        // - El objeto no tiene un componente Node.
        // - El comportamiento de persecución está deshabilitado.
        // - El fantasma está en estado "asustado".
        if (node != null && enabled && !ghost.frightened.enabled)
        {
            // Variable para almacenar la dirección hacia Pacman más cercana.
            Vector2 direction = Vector2.zero;

            // Variable para rastrear la menor distancia encontrada hasta el momento.
            float minDistance = float.MaxValue;

            // Itera sobre las direcciones disponibles en el nodo.
            foreach (Vector2 availableDirection in node.availableDirections)
            {
                // Calcula la posición que se alcanzaría al moverse en la dirección actual.
                Vector3 newPosition = transform.position + new Vector3(availableDirection.x, availableDirection.y);

                // Calcula la distancia cuadrada entre la nueva posición y la posición de Pacman.
                float distance = (ghost.target.position - newPosition).sqrMagnitude;

                // Si esta distancia es menor que la distancia mínima encontrada hasta ahora,
                // actualiza la dirección más cercana y la distancia mínima.
                if (distance < minDistance)
                {
                    direction = availableDirection;
                    minDistance = distance;
                }
            }

            // Establece la dirección del movimiento del fantasma hacia la dirección más cercana a Pacman.
            ghost.movement.SetDirection(direction);
        }
    }
}
