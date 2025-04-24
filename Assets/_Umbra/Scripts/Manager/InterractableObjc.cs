using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;


public enum InterractableType
{
    Door, Note, Key, Battery
}
public class InterractableObjc : MonoBehaviour
{
    public InterractableType interractableType;
    public GameObject panelToShow;
    public TextMeshProUGUI textToShow;
    public int EnergyAmount = 25;

    public void Interract()
    {
        switch (interractableType)
        {
            case InterractableType.Door:
                // Handle door interaction
                Debug.Log("Interacting with a door.");
                break;

            case InterractableType.Note:
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

            case InterractableType.Key:
                // Handle key interaction
                Debug.Log("Interacting with a key.");
                break;

            case InterractableType.Battery:
                // Handle battery interaction
                Debug.Log("Interacting with a battery.");
                break;

            default:
                Debug.LogWarning("Unknown interaction type.");
                break;
        }
    }
}

