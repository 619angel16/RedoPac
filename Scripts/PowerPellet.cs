using UnityEngine;

public class PowerPellet : Pellet
{
    public float duration = 8f; // Duración del efecto de poder al comer el power pellet.

    // Sobrescribe el método Eat de la clase base Pellet.
    protected override void Eat()
    {
        // Llama al método PowerPelletEaten en el GameManager.
        GameManager.Instance.PowerPelletEaten(this);
    }
}