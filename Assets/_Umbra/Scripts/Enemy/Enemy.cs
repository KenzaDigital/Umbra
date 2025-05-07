using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EnemyAI : MonoBehaviour
{
    public float chaseSpeed = 3f;
    public float fleeSpeed = 5f;
    public float detectionRadius = 10f;

    public Transform player;
    public Light2D torchLight;

    public float lightKillTime = 2f;
    private float lightTimer = 0f;
    private bool isDead = false;

    [Header("Flee Settings")]
    public Transform initialPosition;
    public float maxFleeDistance = 10f;

    private bool scarySoundPlayed = false;
    private bool heartBeatPlaying = false;

    void Update()
    {
        if (isDead || player == null || torchLight == null)
            return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius)
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;

            if (IsLitByTorch())
            {
                float fleeDistance = Vector2.Distance(transform.position, initialPosition.position);
                if (fleeDistance < maxFleeDistance)
                {
                    transform.position -= directionToPlayer * fleeSpeed * Time.deltaTime;
                }

                lightTimer += Time.deltaTime;
                if (lightTimer >= lightKillTime)
                {
                    Die();
                }
            }
            else
            {
                transform.position += directionToPlayer * chaseSpeed * Time.deltaTime;
                lightTimer = 0f;
            }

            // Play scary sound once if close
            if (!scarySoundPlayed && distanceToPlayer < detectionRadius / 2f)
            {
                if (audioManager.instance != null)
                {
                    audioManager.instance.PlaySFX("ScarySound");
                    scarySoundPlayed = true;
                }
            }

            // Play heartbeat sound continuously if close
            if (!heartBeatPlaying && audioManager.instance != null)
            {
                audioManager.instance.PlaySFX("HeartBeatSound");
                heartBeatPlaying = true;
            }
        }
        else
        {
            // Return to start if too far from player
            float returnDist = Vector2.Distance(transform.position, initialPosition.position);
            if (returnDist > 1f)
            {
                Vector3 dirBack = (initialPosition.position - transform.position).normalized;
                transform.position += dirBack * chaseSpeed * Time.deltaTime;
            }

            // Reset sounds if player out of range
            if (scarySoundPlayed)
            {
                scarySoundPlayed = false;
            }

            if (heartBeatPlaying)
            {
                heartBeatPlaying = false;
                // Optionnel : stop via audioManager if implémenté
                // audioManager.instance.StopSFX("HeartBeatSound");
            }
        }
    }

    bool IsLitByTorch()
    {
        Vector3 toEnemy = transform.position - torchLight.transform.position;
        float distance = toEnemy.magnitude;
        float dot = Vector3.Dot(torchLight.transform.right, toEnemy.normalized);

        return dot > 0.5f && distance < torchLight.pointLightOuterRadius;
    }

    void Die()
    {
        isDead = true;
        Debug.Log("L'ennemi est mort !");
        gameObject.SetActive(false);
    }
}
