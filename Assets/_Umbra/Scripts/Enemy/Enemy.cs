using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public static Enemy currentChasingEnemy = null; // ENNEMI CONTRÔLEUR GLOBAL

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
    [SerializeField] private Image fondRouge;
    public float redOverlaySpeed = 1f;

    [Header("Fade Audio")]
    public float audioFadeSpeed = 1f;

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
        heartbeatSource.volume = 0f;

        musicSource.clip = scaryMusic;
        musicSource.loop = true;
        musicSource.playOnAwake = false;
        musicSource.volume = 0f;

        audioSource = gameObject.AddComponent<AudioSource>();

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

            if (currentChasingEnemy == this)
            {
                HandleChaseAudio(true);
                HandleScreenOverlay(true);
            }

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

            if (currentChasingEnemy == this)
            {
                HandleChaseAudio(false);
                HandleScreenOverlay(false);
            }

            dangerTimer = 0f;
            playerKilled = false;
        }

        if (currentChasingEnemy == this)
            AdjustFondRougeTransparency();
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

            if (currentChasingEnemy == null ||
                Vector2.Distance(transform.position, player.position) < Vector2.Distance(currentChasingEnemy.transform.position, player.position))
            {
                currentChasingEnemy = this;
            }
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
            if (currentChasingEnemy == this)
                currentChasingEnemy = null;

            player = null;
        }
    }

    void HandleChaseAudio(bool shouldPlay)
    {
        float targetVolume = shouldPlay ? 1f : 0f;

        if (heartbeatSource != null)
        {
            heartbeatSource.volume = Mathf.MoveTowards(heartbeatSource.volume, targetVolume, audioFadeSpeed * Time.deltaTime);
            if (shouldPlay && !heartbeatSource.isPlaying) heartbeatSource.Play();
            if (!shouldPlay && heartbeatSource.volume <= 0f && heartbeatSource.isPlaying) heartbeatSource.Stop();
            heartbeatSource.pitch = shouldPlay ? fastHeartbeatPitch : 1f;
        }

        if (musicSource != null)
        {
            musicSource.volume = Mathf.MoveTowards(musicSource.volume, targetVolume, audioFadeSpeed * Time.deltaTime);
            if (shouldPlay && !musicSource.isPlaying) musicSource.Play();
            if (!shouldPlay && musicSource.volume <= 0f && musicSource.isPlaying) musicSource.Stop();
        }
    }

    void HandleScreenOverlay(bool shouldPlay)
    {
        if (fondRouge != null)
        {
            Color currentColor = fondRouge.color;
            if (shouldPlay)
                currentColor.a = Mathf.Min(1f, currentColor.a + redOverlaySpeed * Time.deltaTime);
            else
                currentColor.a = Mathf.Max(0f, currentColor.a - redOverlaySpeed * Time.deltaTime);

            fondRouge.color = currentColor;
        }
    }

    private void AdjustFondRougeTransparency()
    {
        if (fondRouge == null || player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        float alpha = Mathf.Clamp01(0.50f - (distance / detectionRange));

        Color color = fondRouge.color;
        color.a = alpha;
        fondRouge.color = color;
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
