using UnityEngine;

public class PlayerEnergy : MonoBehaviour
{
    public int maxEnergy = 100;
    public int currentEnergy = 100;

    public void Recharge(int amount)
    {
        currentEnergy = Mathf.Min(currentEnergy + amount, maxEnergy);
        Debug.Log("Energy recharged. Current energy: " + currentEnergy);
    }
}
