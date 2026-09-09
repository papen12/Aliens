using UnityEngine;

public class ControlAlien : MonoBehaviour
{
    public int puntos = 100;

    void OnTriggerEnter2D(Collider2D otro)
    {
        if (otro.CompareTag("bala"))
        {
            if (Generador.instancia != null)
            {
                Generador.instancia.SumarPuntos(puntos);
            }
            Destroy(otro.gameObject);
            Destroy(gameObject);
        }
    }
}