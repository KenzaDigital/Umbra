using UnityEngine;

public class FragmentQuestManager : MonoBehaviour
{
    public static FragmentQuestManager Instance { get; private set; }

    private int fragmentsCollected = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Pour éviter que l'objet ne soit détruit entre les scènes.
        }
        else
        {
            Destroy(gameObject);  // Si une instance existe déjà, on la détruit.
        }
    }

    public void CollectFragment(int fragmentID)
    {
        fragmentsCollected++;
        Debug.Log($"Fragment {fragmentID} collected! Total fragments: {fragmentsCollected}");
        // Logique supplémentaire pour la collecte des fragments (sauvegarde, progression de la quête, etc.)
    }

    public int GetFragmentsCollected()
    {
        return fragmentsCollected;
    }
}
