using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header("UI Prompt")]
    public GameObject interactionPromptPanel;
    public TextMeshProUGUI interactionPromptText;

    private IInteractable currentInteractable;
    private bool isInteracting;

    private void Awake()
    {
        if (interactionPromptPanel != null)
            interactionPromptPanel.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger avec : " + other.name);
        IInteractable interactable = other.GetComponentInParent<IInteractable>();

        if (interactable != null)
        {
            currentInteractable = interactable;
            if (interactionPromptPanel != null && interactionPromptText != null)
            {
                interactionPromptPanel.SetActive(true);
                interactionPromptText.text = currentInteractable.GetInteractionPrompt();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (currentInteractable != null && other.GetComponentInParent<IInteractable>() == currentInteractable)
        {
            currentInteractable = null;
            if (interactionPromptPanel != null)
                interactionPromptPanel.SetActive(false);
        }
    }

    private void Update()
    {
        // Interaction avec "E"
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (currentInteractable != null && !isInteracting)
            {
                isInteracting = true;
                currentInteractable.Interact();
                isInteracting = false;
            }
        }

        // Fermeture avec "Escape"
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            var interactableObject = currentInteractable as InteractableObject;
            if (interactableObject != null && interactableObject.interactableType == InteractableType.Note)
            {
                interactableObject.CloseNotePanel();
            }
        }
    }
}
