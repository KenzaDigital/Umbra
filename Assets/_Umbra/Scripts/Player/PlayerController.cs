using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float originalSpeed; // Vitesse normale
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float runDuration = 3f;
    [SerializeField] private float runCooldown = 20f;

    private Vector2 move;
    private Rigidbody2D rb;
    private bool canRun = true;
    private bool isRunning = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Move(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
        if (audioManager.instance != null)
        {
            audioManager.instance.PlaySFX("Walking");
        }
    }

    public void Run(InputAction.CallbackContext context)
    {
        // V�rifie si l'entr�e est "performed" et si le joueur peut courir
        if (context.performed && canRun)
        {
            StartCoroutine(RunCoroutine());
        }
    }

    private IEnumerator RunCoroutine()
    {
        Debug.Log("RunCoroutine started");
        canRun = false; // D�sactiver la possibilit� de courir
        isRunning = true; // Indiquer que le joueur est en train de courir

        originalSpeed = moveSpeed; // Sauvegarder la vitesse normale
        moveSpeed = runSpeed; // Passer � la vitesse de course

        yield return new WaitForSeconds(runDuration); // Attendre la dur�e de la course

        isRunning = false; // Indiquer que le joueur a termin� de courir
        moveSpeed = originalSpeed; // R�tablir la vitesse normale
        Debug.Log("RunCoroutine: Speed reset to original speed");

        // Lancer le cooldown apr�s la course
        StartCoroutine(CooldownCoroutine());
    }

    private IEnumerator CooldownCoroutine()
    {
        Debug.Log("Cooldown started");
        yield return new WaitForSeconds(runCooldown); // Attendre le d�lai de r�cup�ration
        canRun = true; // R�activer la possibilit� de courir
        Debug.Log("Cooldown ended");
    }

    private void Update()
    {
        if (move != Vector2.zero)
        {
            float angle = Mathf.Atan2(move.y, move.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private void FixedUpdate()
    {
        Vector2 movement = move * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
    }
}
