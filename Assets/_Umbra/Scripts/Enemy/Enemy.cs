using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public Image screenOverlay;
    public float redOverlaySpeed = 1f;

    [Header("Mort du joueur")]
    public float timeBeforeKill = 5f;
    private float dangerTimer = 0f;
    private bool playerKilled = false;

    [Header("Effet Screamer")]
    public GameObject deathCanvasObject;
    public AudioClip screamSound;
    public float timeBeforeGameOver = 2f;

    private int currentPoint = 0;
    private Transform player;
    private bool isChasing = false;

    private AudioSource heartbeatSource;
    private AudioSource musicSource;
    private AudioSource audioSource;

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

        // Ajoute un AudioSource pour le cri
        audioSource = gameObject.AddComponent<AudioSource>();

        // Désactiver le screamer au départ
        if (deathCanvasObject != null)
            deathCanvasObject.SetActive(false);
    }

    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
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
            HandleScreenOverlay(true);

            dangerTimer += Time.deltaTime;

            if (dangerTimer >= timeBeforeKill && !playerKilled)
            {
                KillPlayer();
                playerKilled = true;
            }
        }
        else
        {
            Patrol();
            DetectPlayer();
            HandleChaseAudio(false);
            HandleScreenOverlay(false);

            dangerTimer = 0f;
            playerKilled = false;
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

    void HandleScreenOverlay(bool shouldPlay)
    {
        if (screenOverlay != null)
        {
            Color currentColor = screenOverlay.color;
            if (shouldPlay)
                currentColor.a = Mathf.Min(1f, currentColor.a + redOverlaySpeed * Time.deltaTime);
            else
                currentColor.a = Mathf.Max(0f, currentColor.a - redOverlaySpeed * Time.deltaTime);

            screenOverlay.color = currentColor;
        }
    }

    void KillPlayer()
    {
        Debug.Log("Le joueur a été englouti !");
        StartCoroutine(PlayScreamerAndGameOver());
    }

    System.Collections.IEnumerator PlayScreamerAndGameOver()
    {
        if (screamSound != null && audioSource != null)
            audioSource.PlayOneShot(screamSound);

        if (deathCanvasObject != null)
        {
            int flashes = 6;
            float flashDuration = 0.1f;

            for (int i = 0; i < flashes; i++)
            {
                deathCanvasObject.SetActive(true);
                yield return new WaitForSeconds(flashDuration);
                deathCanvasObject.SetActive(false);
                yield return new WaitForSeconds(flashDuration);
            }

            // Affiche le screamer à la fin
            deathCanvasObject.SetActive(true);
        }

        yield return new WaitForSeconds(timeBeforeGameOver);
        SceneManager.LoadScene("GameOver");
    }



    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
