using UnityEngine;
using UnityEngine.UI;

public class FragmentQuestManager : MonoBehaviour
{
    public int fragmentsCollected = 0;  // Nombre de fragments collect�s
    public int totalFragments = 4;  // Nombre total de fragments dans le jeu
    public Text fragmentProgressText;  // Affiche la progression des fragments
    public GameObject endSequenceTrigger;  // L'objet qui active la s�quence de fin
    public string[] fragmentTexts;  // Texte � afficher quand chaque fragment est r�cup�r�

    public void CollectFragment(int fragmentIndex)
    {
        if (fragmentIndex > fragmentsCollected && fragmentIndex <= totalFragments)
        {
            fragmentsCollected++;
            Debug.Log("Fragment collected: " + fragmentsCollected + "/" + totalFragments);

            // Met � jour le texte pour afficher la progression
            if (fragmentProgressText != null)
            {
                fragmentProgressText.text = $"Fragments collected: {fragmentsCollected}/{totalFragments} - {fragmentTexts[fragmentsCollected - 1]}";
            }

            // Si tous les fragments sont collect�s, active la s�quence de fin
            if (fragmentsCollected >= totalFragments)
            {
                if (endSequenceTrigger != null)
                {
                    endSequenceTrigger.SetActive(true);  // Active la s�quence de fin
                    Debug.Log("All fragments collected! Triggering end sequence.");
                }
            }
        }
    }
}