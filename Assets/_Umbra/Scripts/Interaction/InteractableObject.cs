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
                       // playerInteraction.DisableInteractionPrompt();
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

          /*  case InteractableType.Fragment:
                HandleFragmentInteraction(); 
                                             // Affiche un message via le panel
               
                break;*/

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

    /*private void HandleFragmentInteraction()
    {
        // Utilisation du singleton FragmentQuestManager
        var questManager = FragmentQuestManager.Instance; // Accès direct via le singleton

        if (questManager != null)
        {
            // Collecte du fragment
            questManager.CollectFragment(FragmentID);
            Debug.Log($"Fragment with ID {FragmentID} collected!");

            // Affichage du message de collecte du fragment
            ShowFragmentCollectedMessage();

            // Désactivation de l'objet fragment après collecte
            gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("FragmentQuestManager not found!");
        }
    }*/

   /* private void ShowFragmentCollectedMessage()
    {
        if (panelToShow != null)
        {
            panelToShow.SetActive(true); // Affiche le panneau des fragments
            var textComponent = panelToShow.GetComponent<TextMeshProUGUI>();
            if (textComponent != null)
            {
                // Mise à jour du texte du panneau
                textComponent.text = $"{fragmentCollectMessage} {FragmentQuestManager.Instance.GetFragmentsCollected()}/{fragmentTotal}";
                Debug.Log($"Fragment with ID {keyID} collected! {fragmentCollectMessage} {FragmentQuestManager.Instance.GetFragmentsCollected()}/{fragmentTotal}");
            }
            else
            {
                Debug.LogWarning("panelToShow does not have a TextMeshProUGUI component.");
            }
        }
    }*/

    private void OpenDoor()
    {
        isDoorOpened = true;
        var collider = GetComponentsInChildren<Collider2D>();
        foreach (var col in collider)
        {
            if (col != null && !col.isTrigger)
            {
                col.enabled = false; // Désactive les colliders de la porte
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
