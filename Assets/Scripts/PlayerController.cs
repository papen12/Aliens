using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public GameObject balaPrefab;
    public Transform puntoDisparo;
    public float velocidad = 5f;
    public float limiteX = 8f;
    public float cadencia = 0.25f;
    public float velocidadBala = 10f;
    public float vidaBala = 3f;

    private float tiempoUltimoDisparo;

    void Update()
    {
        Keyboard teclado = Keyboard.current;
        if (teclado == null) return;

        float horizontal = 0f;
        if (teclado.leftArrowKey.isPressed || teclado.aKey.isPressed) horizontal = -1f;
        if (teclado.rightArrowKey.isPressed || teclado.dKey.isPressed) horizontal = 1f;

        transform.Translate(Vector3.right * horizontal * velocidad * Time.deltaTime);

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -limiteX, limiteX);
        transform.position = pos;

        if (teclado.spaceKey.wasPressedThisFrame && Time.time >= tiempoUltimoDisparo + cadencia)
        {
            Disparar();
        }
    }

    void Disparar()
    {
        Transform origen = puntoDisparo != null ? puntoDisparo : transform;
        GameObject bala = Instantiate(balaPrefab, origen.position, Quaternion.identity);

        Rigidbody2D rb = bala.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.up * velocidadBala;
        }

        Destroy(bala, vidaBala);
        tiempoUltimoDisparo = Time.time;
    }
}