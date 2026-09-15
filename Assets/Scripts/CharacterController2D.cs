using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class CharacterController2D : MonoBehaviour
{
    public float velocidad = 5f;
    public float fuerzaSalto = 7f;
    public Transform groundCheck;
    public float radioGroundCheck = 0.1f;
    private const string tagSuelo = "piso";

    private Rigidbody2D rb;
    private Animator animator;
    private bool enSuelo;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (groundCheck == null)
        {
            Debug.LogError("CharacterController2D: falta asignar 'Ground Check' en el Inspector.", this);
        }
    }

    void Update()
    {
        Keyboard teclado = Keyboard.current;
        if (teclado == null || groundCheck == null) return;

        enSuelo = false;
        Collider2D[] colisiones = Physics2D.OverlapCircleAll(groundCheck.position, radioGroundCheck);
        foreach (Collider2D col in colisiones)
        {
            if (col.CompareTag(tagSuelo))
            {
                enSuelo = true;
                break;
            }
        }

        float horizontal = 0f;
        if (teclado.leftArrowKey.isPressed || teclado.aKey.isPressed) horizontal = -1f;
        if (teclado.rightArrowKey.isPressed || teclado.dKey.isPressed) horizontal = 1f;

        rb.linearVelocity = new Vector2(horizontal * velocidad, rb.linearVelocity.y);

        if (horizontal != 0f)
        {
            transform.localScale = new Vector3(Mathf.Sign(horizontal), 1f, 1f);
        }

        animator.SetBool("isWalking", horizontal != 0f);

        if (teclado.spaceKey.wasPressedThisFrame && enSuelo)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
            animator.SetTrigger("Jump");
        }
    }
}
