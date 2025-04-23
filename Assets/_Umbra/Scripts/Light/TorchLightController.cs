using UnityEngine;

public class TorchLightController : MonoBehaviour
{
    [SerializeField] private Transform Player; // Reference to the player transform
    [SerializeField] private Vector3 offset; // décalage par rapport au joueur 

    private void LateUpdate()
    {
        if (Player != null)
        {
            // Met à jour la position de la torche en fonction de celle du joueur
            transform.position = Player.position + offset;
        }
    }
}