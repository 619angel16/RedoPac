using UnityEngine;

// Requiere que el objeto al que se adjunta este script tenga un componente de tipo Ghost.
[RequireComponent(typeof(Ghost))]
public abstract class GhostBehavior : MonoBehaviour
{
    // Referencia al componente Ghost asociado a este comportamiento.
    public Ghost ghost { get; private set; }

    // Duración predeterminada para la activación de este comportamiento.
    public float duration;

    private void Awake()
    {
        // Obtiene y guarda la referencia al componente Ghost en el mismo objeto.
        ghost = GetComponent<Ghost>();
    }

    // Activa este comportamiento usando la duración predeterminada.
    public void Enable()
    {
        Enable(duration);
    }

    // Método virtual que activa este comportamiento por un tiempo específico.
    public virtual void Enable(float duration)
    {
        // Habilita este componente para que comience a ejecutarse.
        enabled = true;

        // Cancela cualquier invocación previa programada.
        CancelInvoke();

        // Programa la desactivación de este comportamiento después de la duración especificada.
        Invoke(nameof(Disable), duration);
    }

    // Método virtual que desactiva este comportamiento.
    public virtual void Disable()
    {
        // Deshabilita este componente para detener su ejecución.
        enabled = false;

        // Cancela cualquier invocación pendiente programada por este script.
        CancelInvoke();
    }
}