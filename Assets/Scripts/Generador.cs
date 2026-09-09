using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Generador : MonoBehaviour
{
    public static Generador instancia;

    public Rigidbody2D prefabAlienA;
    public Rigidbody2D prefabAlienB;
    public Rigidbody2D prefabAlienC;
    public TMP_Text textoPuntaje;

    private Rigidbody2D[,] aliens;
    private const int Filas = 4;
    private const int Columnas = 6;

    private enum direccion { IZQ, DER };
    private direccion rumbo = direccion.DER;

    private float altura = 0.5f;
    private float limiteIzq;
    private float limiteDer;
    private float velocidad = 1f;
    private int puntaje = 0;

    void Awake()
    {
        instancia = this;
    }

    void Start()
    {
        GenerarAliens(Filas, Columnas, 1.5f, 1.0f);
        float distanciaHorizontal = Camera.main.orthographicSize * Screen.width / Screen.height;
        limiteIzq = -1.0f * distanciaHorizontal + 1;
        limiteDer = 1.0f * distanciaHorizontal - 1;
        ActualizarTexto();
    }

    void Update()
    {
        int numAliens = 0;
        bool limi = false;

        for (int i = 0; i < Filas; i++)
        {
            for (int j = 0; j < Columnas; j++)
            {
                if (aliens[i, j] != null)
                {
                    numAliens++;
                    if (rumbo == direccion.DER)
                    {
                        aliens[i, j].transform.Translate(Vector2.right * velocidad * Time.deltaTime, Space.World);
                        if (aliens[i, j].transform.position.x > limiteDer)
                        {
                            limi = true;
                        }
                    }
                    else
                    {
                        aliens[i, j].transform.Translate(Vector2.left * velocidad * Time.deltaTime, Space.World);
                        if (aliens[i, j].transform.position.x < limiteIzq)
                        {
                            limi = true;
                        }
                    }
                }
            }
        }

        if (numAliens == 0)
        {
            SceneManager.LoadScene("Nivel1");
        }

        if (limi == true)
        {
            for (int i = 0; i < Filas; i++)
            {
                for (int j = 0; j < Columnas; j++)
                {
                    if (aliens[i, j] != null)
                    {
                        aliens[i, j].transform.position += Vector3.down * altura;
                    }
                }
            }

            if (rumbo == direccion.DER)
            {
                rumbo = direccion.IZQ;
            }
            else
            {
                rumbo = direccion.DER;
            }
        }
    }

    void GenerarAliens(int filas, int columnas, float espacioH, float espacioV, float escala = 1.0f)
    {
        Vector2 origen = new Vector2(transform.position.x - (columnas / 2.0f) * espacioH + (espacioH / 2), transform.position.y);
        aliens = new Rigidbody2D[filas, columnas];

        for (int i = 0; i < filas; i++)
        {
            for (int j = 0; j < columnas; j++)
            {
                Vector2 posicion = new Vector2(origen.x + (espacioH * j), origen.y + (espacioV * i));
                Rigidbody2D prefab = SeleccionarPrefab(i, j);
                Rigidbody2D alien = (Rigidbody2D)Instantiate(prefab, posicion, transform.rotation);
                aliens[i, j] = alien;
            }
        }
    }

    Rigidbody2D SeleccionarPrefab(int fila, int columna)
    {
        if (columna <= 1) return prefabAlienA;
        if (columna <= 3) return prefabAlienB;
        return prefabAlienC;
    }

    public void SumarPuntos(int puntos)
    {
        puntaje += puntos;
        ActualizarTexto();
    }

    void ActualizarTexto()
    {
        if (textoPuntaje != null)
        {
            textoPuntaje.text = "Puntaje:" + puntaje;
        }
    }
}