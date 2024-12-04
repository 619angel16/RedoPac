using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Pellet : MonoBehaviour
{
    public int points = 10; // Puntos que se otorgan al comer este pellet.

    // Método protegido que se llama cuando el pellet es "comido".
    protected virtual void Eat()
    {
        // Notifica al GameManager que el pellet ha sido comido.
        GameManager.Instance.PelletEaten(this);
    }

    // Método llamado cuando otro Collider2D entra en contacto con el collider del pellet.
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica si el objeto que colisiona es Pacman.
        if (other.gameObject.layer == LayerMask.NameToLayer("Pacman")) {
            Eat(); // Llama al método Eat si Pacman entra en el trigger.
        }
    }
}