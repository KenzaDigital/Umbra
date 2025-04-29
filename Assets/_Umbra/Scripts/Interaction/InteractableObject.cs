using UnityEngine;
using TMPro;



public class InteractableObject : MonoBehaviour, IInteractable
{
    public InteractableType interactableType;
    public GameObject panelToShow;
    public TextMeshProUGUI textToShow;
    public int energyAmount = 25;
    public int keyID;

    private GameObject player;
    private bool isDoorOpened = false;






    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player object with tag 'Player' not found.");
        }
    }

    public void Interact()
    {
        Debug.Log($"Interact method called for object: {gameObject.name} with type: {interactableType}");

        if (player == null)
        {
            Debug.LogError("Player object is not assigned.");
            return;
        }

        switch (interactableType)
        {
            case InteractableType.Door:
                var playerInventory = player.GetComponent<Inventory>();
                if (playerInventory != null)
                {
                    Debug.Log($"Player has key: {playerInventory.HasKey(keyID)}");
                    if (playerInventory.HasKey(keyID) && !isDoorOpened)
                    {
                        OpenDoor();
                    }
                    else
                    {
                        Debug.Log("The door is locked. You need a key to open it.");
                        ShowLockedMessage();
                    }
                }
                else
                {
                    Debug.LogError("Inventory script not found on the Player object.");
                }
                break;

            case InteractableType.Note:
                if (panelToShow != null)
                {
                    panelToShow.SetActive(true);
                    var textComponent = panelToShow.GetComponent<TextMeshProUGUI>();
                    if (textComponent != null)
                    {
                        textComponent.text = textToShow.text;
                        Debug.Log("Note displayed: " + textToShow.text);
                    }
                    else
                    {
                        //Debug.LogWarning("panelToShow does not have a TextMeshProUGUI component.");
                    }
                }
                else
                {
                   // Debug.LogWarning("panelToShow is not assigned for this note.");
                }
                break;

            case InteractableType.Key:
                var inventory = player.GetComponent<Inventory>();
                if (inventory != null)
                {
                    inventory.AddKey(keyID);
                    Debug.Log("Key added to inventory!");
                    gameObject.SetActive(false);
                }
                else
                {
                    //Debug.LogError("Inventory component not found on the Player!");
                }
                break;

            case InteractableType.Battery:
                var energyScript = player.GetComponent<PlayerEnergy>();
                if (energyScript != null)
                {
                    energyScript.Recharge(energyAmount);
                }
                else
                {
                    Debug.LogWarning("PlayerEnergy script not found on the Player object.");
                }
                Debug.Log("Interacting with a battery.");
                break;

            default:
                Debug.LogWarning($"Unknown interaction type for object: {gameObject.name}");
                break;
        }
    }

    private void OpenDoor()
    {
        isDoorOpened = true;
        var collider = GetComponentsInChildren<Collider2D>();
        foreach (var col in collider)
        {
            if (col != null && !col.isTrigger)
            {
                col.enabled = false;
                Debug.Log("Door collider disabled.");
            }
        }

        var animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Open");
            Debug.Log("Door opening animation triggered.");
        }
    }

    private void ShowLockedMessage()
    {
        Debug.Log("You need a key to open this door.");
    }

    public string GetInteractionPrompt()
    {
        return interactableType switch
        {
            InteractableType.Door => "Press E to open the door.",
            InteractableType.Note => "Press E to read the note.",
            InteractableType.Key => "Press E to pick up the key.",
            InteractableType.Battery => "Press E to recharge energy.",
            _ => "Press E to interact."
        };
    }
}
