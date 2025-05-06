using UnityEngine;

public class HintNote : MonoBehaviour, IInteractable
{
    [TextArea]
    public string hintText = "Voici un indice !";

    [SerializeField] private HintDisplayUI hintDisplayUI;

    public string GetInteractionPrompt()
    {
        return "Appuyez sur E pour lire la note.";
    }

    public void Interact()
    {
        Debug.Log(" Interact appelé sur HintNote.");

        if (hintDisplayUI != null)
        {
            hintDisplayUI.ShowHint(hintText);
        }
        else
        {
            Debug.LogError("HintDisplayUI n'est pas assigné !");
        }
    }
}