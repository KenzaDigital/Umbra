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
            DontDestroyOnLoad(gameObject);  // Pour �viter que l'objet ne soit d�truit entre les sc�nes.
        }
        else
        {
            Destroy(gameObject);  // Si une instance existe d�j�, on la d�truit.
        }
    }

    public void CollectFragment(int fragmentID)
    {
        fragmentsCollected++;
        Debug.Log($"Fragment {fragmentID} collected! Total fragments: {fragmentsCollected}");
        // Logique suppl�mentaire pour la collecte des fragments (sauvegarde, progression de la qu�te, etc.)
    }

    public int GetFragmentsCollected()
    {
        return fragmentsCollected;
    }
}
