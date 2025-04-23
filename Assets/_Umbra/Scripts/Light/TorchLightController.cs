using UnityEngine;

public class TorchLightController : MonoBehaviour
{
    [SerializeField] private Transform Player; // Reference to the player transform
    [SerializeField] private Vector3 offset; // d�calage par rapport au joueur 

    private void LateUpdate()
    {
        if (Player != null)
        {
            // Met � jour la position de la torche en fonction de celle du joueur
            transform.position = Player.position + offset;
        }
    }
}