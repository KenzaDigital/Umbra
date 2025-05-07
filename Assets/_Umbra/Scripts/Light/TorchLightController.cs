using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TorchLightController : MonoBehaviour
{
    [SerializeField] private Transform Player; // Référence au joueur
    [SerializeField] private Vector3 offset; // Décalage de base
    private Vector3 currentOffset;
    public Light2D torchlight; // Lumière de la torche
    
    public bool IsTorchOn => torchlight != null && torchlight.enabled;
    public Vector3 TorchDirection => currentOffset.normalized;

    private void LateUpdate()
    {
        if (Player != null)
        {
            float horizontal = Input.GetAxis("Horizontal");

            if (horizontal < 0)
                currentOffset = new Vector3(-offset.x, offset.y, offset.z);
            else if (horizontal > 0)
                currentOffset = offset;

            transform.position = Player.position + currentOffset;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && torchlight != null)
        {
            torchlight.enabled = !torchlight.enabled;
        }
    }
}