using UnityEngine;

public class GhostScatter : GhostBehavior
{
    // Se llama al deshabilitar el comportamiento, activando el modo de persecución del fantasma.
    private void OnDisable()
    {
        ghost.chase.Enable(); // Activa el comportamiento de persecución del fantasma al deshabilitar el modo de dispersión.
    }

    // Se llama cuando el fantasma entra en contacto con un objeto colisionador.
    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>(); // Obtiene el componente Node del objeto con el que colisiona.

        // No hace nada si el nodo es nulo, si el comportamiento no está habilitado
        // o si el fantasma está en estado de asustado.
        if (node != null && enabled && !ghost.frightened.enabled)
        {
            // Selecciona una dirección aleatoria de las direcciones disponibles del nodo.
            int index = Random.Range(0, node.availableDirections.Count);

            // Prefiere no regresar en la dirección opuesta a la actual.
            if (node.availableDirections.Count > 1 && node.availableDirections[index] == -ghost.movement.direction)
            {
                index++; // Incrementa el índice para elegir la siguiente dirección.

                // Si el índice se desborda, se ajusta para que vuelva a la primera dirección disponible.
                if (index >= node.availableDirections.Count)
                {
                    index = 0;
                }
            }

            // Establece la dirección de movimiento elegida para el fantasma.
            ghost.movement.SetDirection(node.availableDirections[index]);
        }
    }
}