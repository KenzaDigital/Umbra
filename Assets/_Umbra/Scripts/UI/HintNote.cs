using UnityEngine;

public class HintNote : MonoBehaviour, IInteractable
{
    public HintData hintData;
    [SerializeField] private HintDisplayUI hintDisplayUI;

    public string GetInteractionPrompt()
    {
        return "Appuyez sur E pour lire la note.";
    }

    public void Interact()
    {
        Debug.Log("Interact appel� sur HintNote.");

        if (hintData == null)
        {
            Debug.LogError("hintData est null. Assurez-vous qu'il est assign� dans l'inspecteur.");
            return;
        }

        if (hintDisplayUI != null)
        {
            Debug.Log("HintDisplayUI trouv�. Affichage de l'indice...");
            hintDisplayUI.ShowHint(hintData.hintText);
        }
        else
        {
            Debug.LogError("HintDisplayUI n'est pas assign� dans l'inspecteur.");
        }
    }
}
