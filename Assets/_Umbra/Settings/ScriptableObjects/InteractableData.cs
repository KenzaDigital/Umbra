using UnityEngine;

[CreateAssetMenu(fileName = "NewInteractableData", menuName = "Interactables/InteractableData")]
public class InteractableData : ScriptableObject
{
    public InteractableType type;
    public string interactionPrompt;
    public string noteText;
    public int energyAmount;
    public int keyID;
    public bool isReusable;
}

