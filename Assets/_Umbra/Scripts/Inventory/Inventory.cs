using UnityEngine;

public class Inventory : MonoBehaviour
{
    public bool hasKey = false; // Indique si le joueur poss�de une cl�

    public void AddKey()
    {
        hasKey = true;
        Debug.Log("Key added to inventory.");
    }

    public bool HasKey()
    {
        return hasKey;
    }
}
