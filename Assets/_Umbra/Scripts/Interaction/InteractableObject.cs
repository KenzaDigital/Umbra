using UnityEngine;
using TMPro;
using System.Collections;



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

        if (panelToShow != null)
        {
            panelToShow.SetActive(false); // Désactive le Canvas des notes par défaut
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
                    //Debug.Log($"Key with ID {keyID} added to inventory!");


                    // Désactive l'objet clé après interaction
                    gameObject.SetActive(false);

                    // Désactive manuellement le prompt s'il existe
                    var playerInteraction = player.GetComponent<PlayerInteraction>();
                    if (playerInteraction != null)
                    {
                        playerInteraction.DisableInteractionPrompt();
                    }

                    // Active le panneau pour afficher un message ou une icône
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

            case InteractableType.Fragment:
                // Collecte du fragment et mise à jour dans le FragmentQuestManager
                var questManager = FindObjectOfType<FragmentQuestManager>(); // Trouve le gestionnaire des fragments dans la scène
                if (questManager != null)
                {
                    questManager.CollectFragment(keyID);  // Utilise le keyID pour identifier le fragment
                    Debug.Log($"Fragment with ID {keyID} collected!");
                    gameObject.SetActive(false);  // Désactive l'objet après collecte
                }
                else
                {
                    Debug.LogError("FragmentQuestManager not found!");
                }
                break;


            case InteractableType.Battery:
         
                var energyScript = player.GetComponentInChildren<TorchEnergy>();
                if (energyScript != null)
                {
                    energyScript.Recharge(energyAmount);
                }
                else
                {
                    Debug.LogWarning("TorchEnergy script not found on the Player object.");
                }
                Debug.Log("Interacting with a battery.");
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
    public void CloseNotePanel()
    {
        if (panelToShow != null)
        {
            panelToShow.SetActive(false);
            Debug.Log("Note panel closed.");
        }
    }
   
}
