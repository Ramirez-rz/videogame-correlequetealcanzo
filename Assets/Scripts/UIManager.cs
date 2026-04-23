using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections.Generic;

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
    [SerializeField] private AudioClip song7Clip;
    [SerializeField] private AudioClip song8Clip;
    [SerializeField] private AudioClip song9Clip;
    [SerializeField] private AudioClip song10Clip;
    [SerializeField] private AudioClip song11Clip;
    [SerializeField] private AudioClip song12Clip;
    [SerializeField] private AudioClip song13Clip;
    [SerializeField] private AudioClip song14Clip;
    [SerializeField] private AudioClip song15Clip;

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

        List<SongData> configuredSongs = new List<SongData>();

        AddSongIfClipAssigned(configuredSongs, song1Clip, "Safaera", "Dakiti", "Yonaguni", "Moscow Mule", 1);
        AddSongIfClipAssigned(configuredSongs, song2Clip, "Timber", "Give Me Everything", "Rain Over Me", "Hotel Room Service", 2);
        AddSongIfClipAssigned(configuredSongs, song3Clip, "Baby", "Sorry", "Beauty and a Beat", "Peaches", 3);
        AddSongIfClipAssigned(configuredSongs, song4Clip, "Ella Baila Sola", "Lady Gaga", "Rubicon", "Luna", 4);
        AddSongIfClipAssigned(configuredSongs, song5Clip, "Mi Bello Angel", "Perlas Negras", "Amor Tumbado", "Pacas de Billetes", 2);
        AddSongIfClipAssigned(configuredSongs, song6Clip, "Tu si sabes quererme", "Nunca es suficiente", "Lo que construimos", "Hasta la Raiz", 4);
        AddSongIfClipAssigned(configuredSongs, song7Clip, "Un Coco", "Tití Me Pregunto", "Me Porto Bonito", "Neverita", 1);
        AddSongIfClipAssigned(configuredSongs, song8Clip, "Ch y la Pizza", "Billete Grande", "Oye", "TQM", 3);
        AddSongIfClipAssigned(configuredSongs, song9Clip, "25/8", "Monaco", "Me Fui de Vacaciones", "Where She Goes", 1);
        AddSongIfClipAssigned(configuredSongs, song10Clip, "A Mi", "Lo que hay por ahi", "Como dormiste?", "Pa quererte", 2);
        AddSongIfClipAssigned(configuredSongs, song11Clip, "Las Babys", "Vas a Quedarte", "Superestrella", "Mon Amour", 3);
        AddSongIfClipAssigned(configuredSongs, song12Clip, "Columbia", "BZRP Music Sessions #52", "Punto G", "Playa del Ingles", 2);
        AddSongIfClipAssigned(configuredSongs, song13Clip, "Love Story", "Blank Space", "Style", "Shake It Off", 2);
        AddSongIfClipAssigned(configuredSongs, song14Clip, "Yandel 150", "Nunca y Pico", "Reloj", "Noche de Entierro", 1);
        AddSongIfClipAssigned(configuredSongs, song15Clip, "En la orilla", "Memorias", "La Inocente", "Reina", 1);

        songs = configuredSongs.ToArray();
    }

    void AddSongIfClipAssigned(List<SongData> configuredSongs, AudioClip clip, string option1, string option2, string option3, string option4, int correctOption)
    {
        if (clip == null)
        {
            return;
        }

        configuredSongs.Add(new SongData(clip, option1, option2, option3, option4, correctOption));
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
