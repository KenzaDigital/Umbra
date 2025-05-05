using UnityEngine;
using TMPro;
using System.Collections;

public class InteractableObject : MonoBehaviour, IInteractable
{
    public InteractableType interactableType;
    public GameObject panelToShow; // Panel utilisé pour afficher des messages
    public TextMeshProUGUI textToShow; // Texte spécifique à afficher
    public int energyAmount = 25;
    public int keyID;
    public string hintMessage; // Message d'indice à afficher
    public TextMeshProUGUI hintText; // Référence au TextMeshPro pour afficher l'indice

    private GameObject player;
    private bool isDoorOpened = false;
    private bool isPlayerInRange = false; // Indique si le joueur est dans la zone d'interaction

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player object with tag 'Player' not found.");
        }

        if (panelToShow != null)
        {
            panelToShow.SetActive(false); // Désactive le Canvas des notes par défaut
        }

        if (hintText != null)
        {
            hintText.text = ""; // Assurez-vous que le texte est vide au départ
        }
    }

    private void Update()
    {
        // Vérifie si le joueur est dans la zone et appuie sur "E"
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true; // Le joueur est dans la zone
            if (interactableType == InteractableType.Note)
            {
                ShowHintMessage(); // Affiche le message d'indice
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false; // Le joueur quitte la zone
            HideHintMessage(); // Cache le message d'indice
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
                    if (playerInventory.HasKey(keyID) && !isDoorOpened)
                    {
                        OpenDoor();
                    }
                    else
                    {
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
                    panelToShow.SetActive(true); // Active le Canvas des notes
                    var textComponent = panelToShow.GetComponent<TextMeshProUGUI>();
                    if (textComponent != null)
                    {
                        textComponent.text = textToShow.text; // Affiche le texte de la note
                        Debug.Log("Note displayed: " + textToShow.text);
                    }
                    else
                    {
                        Debug.LogWarning("panelToShow does not have a TextMeshProUGUI component.");
                    }
                }
                else
                {
                    Debug.LogWarning("panelToShow is not assigned for this note.");
                }
                break;

            case InteractableType.Key:
                var inventory = player.GetComponent<Inventory>();
                if (inventory != null)
                {
                    // Ajoute la clé à l'inventaire
                    inventory.AddKey(keyID);
                    gameObject.SetActive(false); // Désactive l'objet clé après interaction

                    // Désactive le prompt d'interaction
                    var playerInteraction = player.GetComponent<PlayerInteraction>();
                    if (playerInteraction != null)
                    {
                        playerInteraction.DisableInteractionPrompt();
                    }

                    // Affiche un message via le panel
                    if (panelToShow != null)
                    {
                        panelToShow.SetActive(true);
                        var textComponent = panelToShow.GetComponent<TextMeshProUGUI>();
                        if (textComponent != null)
                        {
                            textComponent.text = $"Key with ID {keyID} added to inventory!";
                            Debug.Log("Key panel displayed.");
                        }
                        else
                        {
                            Debug.LogWarning("panelToShow does not have a TextMeshProUGUI component.");
                        }
                    }
                }
                else
                {
                    Debug.LogError("Inventory component not found on the Player!");
                }
                break;

            

            case InteractableType.Battery:
                var energyScript = player.GetComponentInChildren<TorchEnergy>();
                if (energyScript != null)
                {
                    energyScript.Recharge(energyAmount); // Recharge l'énergie du joueur
                }
                else
                {
                    Debug.LogWarning("TorchEnergy script not found on the Player object.");
                }
                Debug.Log("Interacting with a battery.");
                break;
        }
    }

    private void ShowHintMessage()
    {
        if (hintText != null)
        {
            hintText.text = hintMessage; // Affiche le message dans l'UI
            Debug.Log($"Hint displayed: {hintMessage}");
        }
    }

    private void HideHintMessage()
    {
        if (hintText != null)
        {
            hintText.text = ""; // Efface le message
            Debug.Log("Hint hidden.");
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
            }
        }

        var animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("DoorOpen");
        }
    }

    private void ShowLockedMessage()
    {
        Debug.Log("You need a key to open this door.");
    }

    public string GetInteractionPrompt()
    {
        throw new System.NotImplementedException();
    }
    public void CloseNotePanel()
    {
        if (panelToShow != null)
        {
            panelToShow.SetActive(false); // Désactive le panneau des notes
            Debug.Log("Note panel closed.");
        }
        else
        {
            Debug.LogWarning("panelToShow is not assigned.");
        }
    }
}
