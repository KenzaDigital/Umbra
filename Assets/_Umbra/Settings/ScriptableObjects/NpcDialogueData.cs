using UnityEngine;

[CreateAssetMenu(fileName = "NpcDialogueData", menuName = "Scriptable Objects/NpcDialogueData")]
public class NpcDialogueData : ScriptableObject
{
    public string npcName;
    public string[] dialogueLines;
    public string[] dialogueChoices;
    public float typingSpeed = 0.05f;
    public AudioClip VoiceSound;
    public bool[] isChoice;

    [TextArea] public string finalQuestion;
    public int correctChoiceIndex; // Ajout pour choix final correct
}
