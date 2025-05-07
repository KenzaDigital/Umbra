using UnityEngine;
using UnityEngine.UI;  // Pour l'UI (l'effet visuel)

public class SimpleEnemyAI : MonoBehaviour
{
    [Header("Déplacement")]
    public Transform[] patrolPoints;
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;

    [Header("Détection")]
    public float detectionRange = 5f;
    public LayerMask playerLayer;

    [Header("Sons")]
    public AudioClip heartbeatSound;
    public AudioClip scaryMusic;
    public float fastHeartbeatPitch = 1.5f;

    [Header("Effets Visuels")]
    public Image screenOverlay;  // Image pour l'effet visuel
    public float redOverlaySpeed = 1f;  // Vitesse d'apparition de l'overlay rouge

    private int currentPoint = 0;
    private Transform player;
    private bool isChasing = false;

    private AudioSource heartbeatSource;
    private AudioSource musicSource;

    void Start()
    {
        AudioSource[] sources = GetComponents<AudioSource>();
        if (sources.Length < 2)
        {
            Debug.LogError("Ajoute deux AudioSources sur l'objet Enemy.");
            return;
        }

        heartbeatSource = sources[0];
        musicSource = sources[1];

        heartbeatSource.clip = heartbeatSound;
        heartbeatSource.loop = true;
        heartbeatSource.playOnAwake = false;

        musicSource.clip = scaryMusic;
        musicSource.loop = true;
        musicSource.playOnAwake = false;
    }

    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;  // Trouver le joueur avec le tag "Player"
            if (player == null)
            {
                Debug.LogError("Le joueur n'a pas été trouvé avec le tag 'Player'.");
                return;
            }
        }

        if (isChasing)
        {
            ChasePlayer();
            HandleChaseAudio(true);
            HandleScreenOverlay(true);  // L'écran devient rouge progressivement
        }
        else
        {
            Patrol();
            DetectPlayer();
            HandleChaseAudio(false);
            HandleScreenOverlay(false);  // L'écran redevient transparent
        }
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        Transform target = patrolPoints[currentPoint];
        transform.position = Vector2.MoveTowards(transform.position, target.position, patrolSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target.position) < 0.2f)
        {
            currentPoint = (currentPoint + 1) % patrolPoints.Length;
        }
    }

    void DetectPlayer()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, detectionRange, playerLayer);
        if (hit != null)
        {
            player = hit.transform;
            isChasing = true;
        }
    }

    void ChasePlayer()
    {
        if (player == null) return;

        transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance > detectionRange * 1.5f)
        {
            isChasing = false;
            player = null;
        }
    }

    void HandleChaseAudio(bool shouldPlay)
    {
        if (shouldPlay)
        {
            if (!heartbeatSource.isPlaying)
                heartbeatSource.Play();

            if (!musicSource.isPlaying)
                musicSource.Play();

            heartbeatSource.pitch = fastHeartbeatPitch;
        }
        else
        {
            if (heartbeatSource.isPlaying)
                heartbeatSource.Stop();

            if (musicSource.isPlaying)
                musicSource.Stop();

            heartbeatSource.pitch = 1f;
        }
    }

    // Gérer l'effet visuel de l'écran rouge
    void HandleScreenOverlay(bool shouldPlay)
    {
        if (shouldPlay)
        {
            if (screenOverlay != null)
            {
                // Augmenter progressivement l'opacité de l'écran
                Color currentColor = screenOverlay.color;
                currentColor.a = Mathf.Min(1f, currentColor.a + redOverlaySpeed * Time.deltaTime);
                screenOverlay.color = currentColor;
            }
        }
        else
        {
            if (screenOverlay != null)
            {
                // Réduire progressivement l'opacité de l'écran
                Color currentColor = screenOverlay.color;
                currentColor.a = Mathf.Max(0f, currentColor.a - redOverlaySpeed * Time.deltaTime);
                screenOverlay.color = currentColor;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
