using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private Vector2 move;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // M�thode appel�e par le syst�me d'Input
    public void Move(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public void Run(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            moveSpeed *= 2; // Double la vitesse de d�placement
        }
        else if (context.canceled)
        {
            moveSpeed /= 2; // R�tablit la vitesse de d�placement
        }
    }

    private void Update()
    {
        // V�rifier si le joueur est en mouvement
        if (move != Vector2.zero)
        {
            // Calculer l'angle de rotation en fonction de la direction du mouvement
            float angle = Mathf.Atan2(move.y, move.x) * Mathf.Rad2Deg;

            // Appliquer la rotation au Transform du joueur
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private void FixedUpdate()
    {
        // Appliquer le mouvement
        Vector2 movement = move * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
    }
}
