using UnityEngine;
using UnityEngine.UI;

// Define el orden de ejecución para que este script se ejecute antes que otros scripts con un orden predeterminado.
[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour
{
    // Instancia única del GameManager para implementar el patrón Singleton.
    public static GameManager Instance { get; private set; }

    // Referencias a los objetos del juego configuradas desde el Inspector.
    [SerializeField] private Ghost[] ghosts; // Lista de fantasmas en el juego.
    [SerializeField] private Pacman pacman; // Referencia al jugador (Pacman).
    [SerializeField] private Transform pellets; // Contenedor de todos los pellets.
    [SerializeField] private Text gameOverText; // Texto de "Game Over".
    [SerializeField] private Text scoreText; // Texto para mostrar el puntaje.
    [SerializeField] private Text livesText; // Texto para mostrar las vidas restantes.

    // Variables de estado del juego.
    public int score { get; private set; } = 0; // Puntuación actual del jugador.
    public int lives { get; private set; } = 3; // Número de vidas restantes.

    private int ghostMultiplier = 1; // Multiplicador de puntos para los fantasmas (aumenta al comer varios seguidos).

    private void Awake()
    {
        // Asegura que solo exista una instancia del GameManager.
        if (Instance != null)
        {
            DestroyImmediate(gameObject); // Elimina esta instancia si ya hay otra.
        }
        else
        {
            Instance = this; // Establece esta como la instancia única.
        }
    }

    private void OnDestroy()
    {
        // Limpia la referencia a la instancia única cuando este objeto es destruido.
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        // Inicia un nuevo juego al comenzar.
        NewGame();
    }

    private void Update()
    {
        // Si no quedan vidas y el jugador presiona cualquier tecla, comienza un nuevo juego.
        if (lives <= 0 && Input.anyKeyDown)
        {
            NewGame();
        }
    }

    private void NewGame()
    {
        // Restablece el puntaje y las vidas al valor inicial.
        SetScore(0);
        SetLives(3);

        // Inicia una nueva ronda del juego.
        NewRound();
    }

    private void NewRound()
    {
        // Oculta el texto de "Game Over".
        gameOverText.enabled = false;

        // Reactiva todos los pellets del juego.
        foreach (Transform pellet in pellets)
        {
            pellet.gameObject.SetActive(true);
        }

        // Restablece el estado de Pacman y los fantasmas.
        ResetState();
    }

    private void ResetState()
    {
        // Restablece el estado de todos los fantasmas.
        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].ResetState();
        }

        // Restablece el estado de Pacman.
        pacman.ResetState();
    }

    private void GameOver()
    {
        // Muestra el texto de "Game Over".
        gameOverText.enabled = true;

        // Desactiva a todos los fantasmas y a Pacman.
        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].gameObject.SetActive(false);
        }
        pacman.gameObject.SetActive(false);
    }

    private void SetLives(int lives)
    {
        // Actualiza las vidas restantes y muestra el texto actualizado.
        this.lives = lives;
        livesText.text = "x" + lives.ToString();
    }

    private void SetScore(int score)
    {
        // Actualiza el puntaje y muestra el texto con formato.
        this.score = score;
        scoreText.text = score.ToString().PadLeft(2, '0'); // Formatea con ceros a la izquierda.
    }

    public void PacmanEaten()
    {
        // Activa la secuencia de muerte de Pacman.
        pacman.DeathSequence();

        // Reduce las vidas en uno.
        SetLives(lives - 1);

        // Si hay vidas restantes, reinicia el estado después de 3 segundos.
        if (lives > 0)
        {
            Invoke(nameof(ResetState), 3f);
        }
        else
        {
            // Si no hay vidas, termina el juego.
            GameOver();
        }
    }

    public void GhostEaten(Ghost ghost)
    {
        // Calcula los puntos ganados por comer un fantasma y aumenta el puntaje.
        int points = ghost.points * ghostMultiplier;
        SetScore(score + points);

        // Incrementa el multiplicador para comer fantasmas consecutivos.
        ghostMultiplier++;
    }

    public void PelletEaten(Pellet pellet)
    {
        // Desactiva el pellet que fue comido.
        pellet.gameObject.SetActive(false);

        // Aumenta el puntaje por comer el pellet.
        SetScore(score + pellet.points);

        // Si ya no quedan pellets, comienza una nueva ronda después de 3 segundos.
        if (!HasRemainingPellets())
        {
            pacman.gameObject.SetActive(false);
            Invoke(nameof(NewRound), 3f);
        }
    }

    public void PowerPelletEaten(PowerPellet pellet)
    {
        // Activa el estado "asustado" para todos los fantasmas, durante la duración del pellet.
        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].frightened.Enable(pellet.duration);
        }

        // Procesa el pellet como uno normal y reinicia el multiplicador de fantasmas después de la duración.
        PelletEaten(pellet);
        CancelInvoke(nameof(ResetGhostMultiplier)); // Cancela cualquier llamada pendiente al reinicio del multiplicador.
        Invoke(nameof(ResetGhostMultiplier), pellet.duration); // Programa el reinicio del multiplicador.
    }

    private bool HasRemainingPellets()
    {
        // Comprueba si quedan pellets activos en el juego.
        foreach (Transform pellet in pellets)
        {
            if (pellet.gameObject.activeSelf)
            {
                return true;
            }
        }

        return false; // No quedan pellets.
    }

    private void ResetGhostMultiplier()
    {
        // Reinicia el multiplicador de fantasmas a su valor inicial.
        ghostMultiplier = 1;
    }
}
