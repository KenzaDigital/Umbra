using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    [Header("UI References")]
    public GameObject interactionPromptPanel;
    public TextMeshProUGUI interactionPromptText;

    private bool isInteracting = false;
    private IInteractable currentInteractable;

    private void Awake()
    {

        Debug.Log("PlayerInteraction: Awake called.");

        // Assurer que le prompt est désactivé au démarrage
        if (interactionPromptPanel != null)
        {
            interactionPromptPanel.SetActive(false);
            Debug.Log("PlayerInteraction: Interaction prompt panel disabled at start.");
        }
        else
        {
            Debug.LogWarning("PlayerInteraction: Interaction prompt panel is not assigned.");
        }
    }

    public void Cancel()
    {
        // Vérifie si l'objet interactif actuel est une note
        var interactableObject = currentInteractable as InteractableObject;
        if (interactableObject != null && interactableObject.interactableType == InteractableType.Note)
        {
            interactableObject.CloseNotePanel(); // Ferme le panneau de la note
            Debug.Log("PlayerInteraction: Note panel closed.");
        }
        else
        {
            Debug.LogWarning("PlayerInteraction: No note panel to close.");
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"PlayerInteraction: OnTriggerEnter2D called with {other.gameObject.name}.");

        // Vérifier si l'objet est dans le Layer Interactables
        if (other.gameObject.layer == LayerMask.NameToLayer("Interactable"))
        {
            IInteractable interactable = other.GetComponentInParent<IInteractable>();

            if (interactable != null)
            {
                Debug.Log($"PlayerInteraction: Interactable object detected: {interactable.GetType()} on {other.gameObject.name}");
                currentInteractable = interactable;

                if (interactionPromptPanel != null && interactionPromptText != null)
                {
                    interactionPromptPanel.SetActive(true);
                    interactionPromptText.text = currentInteractable.GetInteractionPrompt();
                    Debug.Log($"PlayerInteraction: Interaction prompt displayed with text: {currentInteractable.GetInteractionPrompt()}.");
                }
                else
                {
                    Debug.LogError("PlayerInteraction: Interaction prompt panel or text is null.");
                }
            }
            else
            {
                Debug.Log($"PlayerInteraction: {other.gameObject.name} is not interactable.");
            }
        }
        else
        {
            Debug.Log($"PlayerInteraction: {other.gameObject.name} is not in the Interactables layer.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log($"PlayerInteraction: OnTriggerExit2D called with {other.gameObject.name}.");

        if (currentInteractable != null && other.GetComponentInParent<IInteractable>() == currentInteractable)
        {
            Debug.Log($"PlayerInteraction: Exiting interaction with {other.gameObject.name}.");
            currentInteractable = null;

            if (interactionPromptPanel != null)
            {
                isInteracting = false; // Réinitialise l'état d'interaction
                interactionPromptPanel.SetActive(false);
                Debug.Log("PlayerInteraction: Interaction prompt panel disabled.");
            }
        }
        else
        {
            Debug.Log($"PlayerInteraction: {other.gameObject.name} was not the current interactable.");
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        Debug.Log("PlayerInteraction: OnInteract called.");

        if (currentInteractable != null && !isInteracting)
        {
            Debug.Log($"PlayerInteraction: Interacting with {currentInteractable.GetInteractionPrompt()}.");
            isInteracting = true;
            currentInteractable.Interact();

            // Réinitialise l'état d'interaction après l'interaction
            isInteracting = false;
        }
        else if (currentInteractable == null)
        {
            Debug.LogWarning("PlayerInteraction: No interactable object available.");
        }
        else if (isInteracting)
        {
            Debug.LogWarning("PlayerInteraction: Already interacting with an object.");
        }
    }
}
