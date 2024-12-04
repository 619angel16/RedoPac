using UnityEngine;

// Requiere que el objeto al que se adjunta este script tenga un componente SpriteRenderer.
[RequireComponent(typeof(SpriteRenderer))]
public class AnimatedSprite : MonoBehaviour
{
    // Lista de sprites que se usarán para la animación.
    public Sprite[] sprites = new Sprite[0];

    // Tiempo que dura cada fotograma de la animación.
    public float animationTime = 0.25f;

    // Determina si la animación se repite en bucle.
    public bool loop = true;

    // Referencia al componente SpriteRenderer que muestra los sprites.
    private SpriteRenderer spriteRenderer;

    // Índice del fotograma actual de la animación.
    private int animationFrame;

    private void Awake()
    {
        // Obtiene el componente SpriteRenderer adjunto a este objeto.
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        // Asegura que el SpriteRenderer esté habilitado cuando este script está activo.
        spriteRenderer.enabled = true;
    }

    private void OnDisable()
    {
        // Desactiva el SpriteRenderer cuando este script está deshabilitado.
        spriteRenderer.enabled = false;
    }

    private void Start()
    {
        // Comienza la animación invocando repetidamente el método Advance.
        // Se ejecuta por primera vez después de "animationTime" segundos y luego repetidamente cada "animationTime".
        InvokeRepeating(nameof(Advance), animationTime, animationTime);
    }

    private void Advance()
    {
        // Si el SpriteRenderer está deshabilitado, no se avanza la animación.
        if (!spriteRenderer.enabled)
        {
            return;
        }

        // Avanza al siguiente fotograma de la animación.
        animationFrame++;

        // Si el índice supera el número de sprites y la animación está en bucle, vuelve al primer fotograma.
        if (animationFrame >= sprites.Length && loop)
        {
            animationFrame = 0;
        }

        // Si el índice está dentro del rango válido, actualiza el sprite del SpriteRenderer.
        if (animationFrame >= 0 && animationFrame < sprites.Length)
        {
            spriteRenderer.sprite = sprites[animationFrame];
        }
    }

    public void Restart()
    {
        // Reinicia la animación estableciendo el índice en -1.
        // Esto asegura que el siguiente fotograma mostrado sea el primero al llamar a Advance.
        animationFrame = -1;

        // Muestra inmediatamente el primer fotograma.
        Advance();
    }
}
