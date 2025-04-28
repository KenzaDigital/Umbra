using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private HashSet<int> keys = new HashSet<int>(); // Ensemble des identifiants des cl�s

    public void AddKey(int keyID)
    {
        keys.Add(keyID);
        Debug.Log($"Key with ID {keyID} added to inventory. Current keys: {string.Join(", ", keys)}");
    }

    public bool HasKey(int keyID)
    {
        bool hasKey = keys.Contains(keyID);
        Debug.Log($"Checking for key with ID {keyID}: {hasKey}");
        return hasKey;
    }
}
