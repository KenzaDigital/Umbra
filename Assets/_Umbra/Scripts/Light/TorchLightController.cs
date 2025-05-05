using UnityEngine;

public class TorchLightController : MonoBehaviour
{
    [SerializeField] private Transform Player; // Reference to the player transform
    [SerializeField] private Vector3 offset; // décalage par rapport au joueur 
    private Vector3 currentOffset; // décalage actuel
    public Light torchlight; // Reference to the torch light component
    private void LateUpdate()
    {
        if (Player != null)
        {
           float horizontal = Input.GetAxis("Horizontal");
            // inverser l'offset si le joueur se déplace vers la gauche
            if (horizontal < 0)
            {
                currentOffset = new Vector3(-offset.x, offset.y, offset.z);
            }
            else if (horizontal > 0)
            {
             currentOffset = offset;
                transform.position = Player.position + currentOffset;
            }
        }
    }

}