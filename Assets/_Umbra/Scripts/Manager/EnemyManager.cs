using System;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    public Image fondRouge; // Assigne ici l’image UI "FondRouge"
    public float detectionRange = 5f;

    private Enemy closestEnemy = null;
    private Transform player;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }


    void Update()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (player == null) return;

        float closestDistance = Mathf.Infinity;
        Enemy[] enemies = FindObjectsWithTag<Enemy>();
        Enemy nearest = null;

        foreach (Enemy enemy in enemies)
        {
            float dist = Vector2.Distance(player.position, enemy.transform.position);
            if (dist < detectionRange && dist < closestDistance)
            {
                closestDistance = dist;
                nearest = enemy;
            }
        }

        closestEnemy = nearest;
    }

    private T[] FindObjectsWithTag<T>() where T : Component
    {
        // Trouve tous les objets avec le tag "Enemy"
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("Enemy");

        // Filtre les objets pour ne garder que ceux qui ont le composant T
        T[] components = Array.ConvertAll(objectsWithTag, obj => obj.GetComponent<T>());

        // Retourne uniquement les objets valides (non null)
        return Array.FindAll(components, component => component != null);
    }

    public bool IsActiveEnemy(Enemy enemy)
    {
        return enemy == closestEnemy;
    }

    public void UpdateFondRouge(float alpha)
    {
        if (fondRouge != null)
        {
            Color color = fondRouge.color;
            color.a = Mathf.Clamp01(alpha);
            fondRouge.color = color;
        }
    }
}
