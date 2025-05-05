using UnityEngine;

[CreateAssetMenu(fileName = "HintData", menuName = "hint")]
public class HintData : ScriptableObject
{
    
    [TextArea(3,10)]
    public string hintText; // Texte 
}
