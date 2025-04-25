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

        // S'assurer que le prompt est désactivé au démarrage
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"PlayerInteraction: OnTriggerEnter2D called with {other.gameObject.name}.");

        if (other == null)
        {
            Debug.LogError("PlayerInteraction: Collider2D is null in OnTriggerEnter2D.");
            return;
        }

        IInteractable interactable = other.GetComponent<IInteractable>();

        if (interactable != null)
        {
            Debug.Log($"PlayerInteraction: Interactable object detected: {other.gameObject.name}.");
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

    private void OnTriggerExit2D(Collider2D other)
    {
        

        if (other == null)
        {
            Debug.LogError("PlayerInteraction: Collider2D is null in OnTriggerExit2D.");
            return;
        }

        IInteractable interactable = other.GetComponent<IInteractable>();

        if (currentInteractable == interactable)
        {
            Debug.Log($"PlayerInteraction: Exiting interaction with {other.gameObject.name}.");
            currentInteractable = null;

            if (interactionPromptPanel != null)
            {
                isInteracting = false;
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
