using UnityEngine;

// Requiere que el objeto al que se adjunte este script tenga un componente SpriteRenderer.
[RequireComponent(typeof(SpriteRenderer))]
public class GhostEyes : MonoBehaviour
{
    // Sprites para representar los ojos en las diferentes direcciones.
    public Sprite up;    // Sprite para mirar hacia arriba.
    public Sprite down;  // Sprite para mirar hacia abajo.
    public Sprite left;  // Sprite para mirar hacia la izquierda.
    public Sprite right; // Sprite para mirar hacia la derecha.

    private SpriteRenderer spriteRenderer; // Referencia al SpriteRenderer del objeto.
    private Movement movement;            // Referencia al componente Movement del objeto padre.

    private void Awake()
    {
        // Obtiene y almacena el SpriteRenderer del GameObject actual.
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Obtiene y almacena el componente Movement del GameObject padre.
        movement = GetComponentInParent<Movement>();
    }

    private void Update()
    {
        // Cambia el sprite de los ojos según la dirección actual del movimiento.
        if (movement.direction == Vector2.up)
        {
            spriteRenderer.sprite = up; // Asigna el sprite para mirar hacia arriba.
        }
        else if (movement.direction == Vector2.down)
        {
            spriteRenderer.sprite = down; // Asigna el sprite para mirar hacia abajo.
        }
        else if (movement.direction == Vector2.left)
        {
            spriteRenderer.sprite = left; // Asigna el sprite para mirar hacia la izquierda.
        }
        else if (movement.direction == Vector2.right)
        {
            spriteRenderer.sprite = right; // Asigna el sprite para mirar hacia la derecha.
        }
    }
}