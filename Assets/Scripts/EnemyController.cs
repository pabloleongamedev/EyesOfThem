using UnityEngine;

public class EnemigoPatrullaAleatoria : MonoBehaviour
{
    private Rigidbody enemyRigidbody;

    public GameObject player;

    public float radioPatrulla = 5f;
    public float velocidad = 2f;
    public float tiempoCambio = 3f;
    public float velocidadRotacion = 5f;
    public float radioDeteccion = 6f;

    public float umbralMirada = 0.6f; // qué tan preciso debe mirar al enemigo

    private Vector3 centro;
    private Vector3 destino;
    private float contadorTiempo;

    void Start()
    {
        enemyRigidbody = GetComponent<Rigidbody>();
        centro = transform.position;
        destino = ObtenerDestinoAleatorio();
    }

    void Update()
    {
        if (player == null) return;

        Vector3 posicionJugador = player.transform.position;
        Vector3 posicionEnemigo = transform.position;

        posicionJugador.y = posicionEnemigo.y;

        float distanciaJugador = Vector3.Distance(posicionEnemigo, posicionJugador);

        if (distanciaJugador <= radioDeteccion)
        {
            if (JugadorMirandoEnemigo())
            {
                // se queda quieto
                destino = transform.position;
            }
            else
            {
                // persigue al jugador
                destino = player.transform.position;
            }
        }
        else
        {
            // patrulla
            contadorTiempo += Time.deltaTime;

            if (contadorTiempo >= tiempoCambio || Vector3.Distance(transform.position, destino) < 0.3f)
            {
                destino = ObtenerDestinoAleatorio();
                contadorTiempo = 0f;
            }
        }
    }

    void FixedUpdate()
    {
        MoverHaciaDestino();
    }

    bool JugadorMirandoEnemigo()
    {
        Vector3 direccionJugador = player.transform.forward;
        Vector3 direccionAlEnemigo = (transform.position - player.transform.position).normalized;

        float dot = Vector3.Dot(direccionJugador, direccionAlEnemigo);

        return dot > umbralMirada;
    }

    void MoverHaciaDestino()
    {
        Vector3 direccion = destino - transform.position;
        direccion.y = 0;

        if (direccion.magnitude > 0.1f)
        {
            Quaternion rotacionObjetivo = Quaternion.LookRotation(direccion.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, velocidadRotacion * Time.fixedDeltaTime);

            Vector3 nuevaPosicion = Vector3.MoveTowards(transform.position, destino, velocidad * Time.fixedDeltaTime);
            enemyRigidbody.MovePosition(nuevaPosicion);
        }
    }

    Vector3 ObtenerDestinoAleatorio()
    {
        Vector2 aleatorio = Random.insideUnitCircle * radioPatrulla;
        return centro + new Vector3(aleatorio.x, 0, aleatorio.y);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radioPatrulla);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioDeteccion);
    }
}