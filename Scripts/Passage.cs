using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Passage : MonoBehaviour
{
    public Transform connection; // Transform de la posición de conexión al que se moverá el objeto al pasar por el passage.

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Ajusta la posición del objeto que colisiona con el passage para que esté en la misma posición de z que el passage.
        Vector3 position = connection.position;
        position.z = other.transform.position.z; // Mantiene la posición en el eje z del objeto que entró al trigger.
        other.transform.position = position; // Mueve el objeto al punto de conexión.
    }
}