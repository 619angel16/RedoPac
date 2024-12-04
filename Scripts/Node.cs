using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public LayerMask obstacleLayer; // Capa de obstáculos usada para detectar colisiones.
    public readonly List<Vector2> availableDirections = new(); // Lista de direcciones disponibles en este nodo.

    private void Start()
    {
        availableDirections.Clear(); // Limpia la lista de direcciones disponibles al inicio.

        // Verifica la disponibilidad de cada dirección (arriba, abajo, izquierda y derecha)
        CheckAvailableDirection(Vector2.up);
        CheckAvailableDirection(Vector2.down);
        CheckAvailableDirection(Vector2.left);
        CheckAvailableDirection(Vector2.right);
    }

    private void CheckAvailableDirection(Vector2 direction)
    {
        // Realiza una prueba de colisión en la dirección especificada usando un BoxCast.
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.5f, 0f, direction, 1f, obstacleLayer);

        // Si no se detecta ningún colisionador, significa que no hay obstáculos en esa dirección.
        if (hit.collider == null)
        {
            availableDirections.Add(direction); // Agrega la dirección a la lista de direcciones disponibles.
        }
    }
}