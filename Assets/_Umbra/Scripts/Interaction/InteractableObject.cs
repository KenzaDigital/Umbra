using UnityEngine;
using TMPro;


public enum InteractableType
    {

    Door, Note ,Key ,Battery

    }
public class InteractableObject : MonoBehaviour
{
    public InteractableType interactableType;
    public GameObject panelToShow;
    public TextMeshProUGUI textToShow;
    public int energyAmount = 25;

    public void Interact()
    {
        switch (interactableType)
        {
            case InteractableType.Door:
                Debug.Log("Interacting with a door.");
                break;

            case InteractableType.Note:
                if (panelToShow != null)
                {
                    panelToShow.SetActive(true);
                    if (panelToShow.GetComponent<TextMeshProUGUI>() != null)
                    {
                        panelToShow.GetComponent<TextMeshProUGUI>().text = textToShow.text;
                    }
                    else
                    {
                        Debug.LogWarning("panelToShow does not have a TextMeshProUGUI component.");
                    }
                }
                break;

            case InteractableType.Key:
                Debug.Log("Interacting with a key.");
                break;

            case InteractableType.Battery:
               
                Debug.Log("Interacting with a battery.");
                break;

            default:
                Debug.LogWarning("Unknown interaction type.");
                break;
        }
    }
}
