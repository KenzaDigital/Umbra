using UnityEngine;

public class Inventory : MonoBehaviour
{
  public bool hasKey = false;

    
    public void AddKey()
    {
        hasKey = true;
        Debug.Log("Key added to inventory.");
    }
}
