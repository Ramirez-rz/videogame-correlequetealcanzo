using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    private class SongData
    {
        public AudioClip clip;
        public string[] options;
        public int correctOption;

        public SongData(AudioClip clip, string option1, string option2, string option3, string option4, int correctOption)
        {
            this.clip = clip;
            options = new[] { option1, option2, option3, option4 };
            this.correctOption = correctOption;
        }
    }

    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private TMP_Text promptText;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private ZombieCollision zombieCollision;
    [SerializeField] private float boostedSpeed = 5.5f;
    [SerializeField] private float reducedSpeed = 4.5f;
    [SerializeField] private float speedEffectDuration = 1f;
    [SerializeField] private AudioClip song1Clip;
    [SerializeField] private AudioClip song2Clip;
    [SerializeField] private AudioClip song3Clip;
    [SerializeField] private AudioClip song4Clip;
    [SerializeField] private AudioClip song5Clip;
    [SerializeField] private AudioClip song6Clip;

    private int wrongAnswers;
    private float normalSpeed;
    private Coroutine speedResetRoutine;
    private SongData[] songs;
    private SongData currentSong;

    void Awake()
    {
        if (playerMovement != null)
        {
            normalSpeed = playerMovement.moveSpeed;
        }

        songs = new[]
        {
            new SongData(song1Clip, "Safaera", "Dakiti", "Yonaguni", "Moscow Mule", 1),
            new SongData(song2Clip, "Timber", "Give Me Everything", "Rain Over Me", "Hotel Room Service", 2),
            new SongData(song3Clip, "Baby", "Sorry", "Beauty and a Beat", "Peaches", 3),
            new SongData(song4Clip, "Ella Baila Sola", "Lady Gaga", "Rubicon", "Luna", 4),
            new SongData(song5Clip, "Mi Bello Angel", "Perlas Negras", "Amor Tumbado", "Pacas de Billetes", 2),
            new SongData(song6Clip, "Tu si sabes quererme", "Nunca es suficiente", "Lo que construimos", "Hasta la Raiz", 4)
        };
    }

    void Start()
    {
        // Primera ronda.
        PlayRandomSong();
    }

    void Update()
    {
        if (Keyboard.current == null)
        {
            return;
        }

        if (Keyboard.current.xKey.wasPressedThisFrame)
        {
            SelectOption(1);
        }

        if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            SelectOption(2);
        }

        if (Keyboard.current.vKey.wasPressedThisFrame)
        {
            SelectOption(3);
        }

        if (Keyboard.current.bKey.wasPressedThisFrame)
        {
            SelectOption(4);
        }
    }

    void PlayRandomSong()
    {
        if (songs == null || songs.Length == 0)
        {
            return;
        }

        // Elige una cancion random.
        SongData nextSong = songs[Random.Range(0, songs.Length)];

        if (songs.Length > 1)
        {
            while (nextSong == currentSong)
            {
                nextSong = songs[Random.Range(0, songs.Length)];
            }
        }

        currentSong = nextSong;

        // Reproduce el audio.
        if (musicSource != null && currentSong.clip != null)
        {
            musicSource.clip = currentSong.clip;
            musicSource.Play();
        }

        UpdatePromptText();
    }

    void UpdatePromptText()
    {
        if (promptText == null || currentSong == null)
        {
            return;
        }

        // Muestra opciones.
        promptText.text =
            "Selecciona el nombre correcto\n" +
            $"Op1: {currentSong.options[0]} (tecla X)\n" +
            $"Op2: {currentSong.options[1]} (tecla C)\n" +
            $"Op3: {currentSong.options[2]} (tecla V)\n" +
            $"Op4: {currentSong.options[3]} (tecla B)";
    }

    void SelectOption(int selectedOption)
    {
        if (playerMovement == null || currentSong == null)
        {
            return;
        }

        // Respuesta correcta.
        if (selectedOption == currentSong.correctOption)
        {
            wrongAnswers = 0;
            ApplyTemporarySpeed(boostedSpeed);
        }
        else
        {
            // Error acumulado.
            wrongAnswers++;
            ApplyTemporarySpeed(reducedSpeed);

            if (wrongAnswers >= 2)
            {
                TriggerGameOver();
                return;
            }
        }

        // Siguiente ronda.
        PlayRandomSong();
    }

    void ApplyTemporarySpeed(float newSpeed)
    {
        // Cambio temporal.
        playerMovement.SetSpeed(newSpeed);

        if (speedResetRoutine != null)
        {
            StopCoroutine(speedResetRoutine);
        }

        speedResetRoutine = StartCoroutine(ResetSpeedAfterDelay());
    }

    System.Collections.IEnumerator ResetSpeedAfterDelay()
    {
        yield return new WaitForSeconds(speedEffectDuration);

        // Velocidad base.
        playerMovement.SetSpeed(normalSpeed);
        speedResetRoutine = null;
    }

    void TriggerGameOver()
    {
        if (zombieCollision != null)
        {
            zombieCollision.TriggerGameOver();
        }
    }
}
