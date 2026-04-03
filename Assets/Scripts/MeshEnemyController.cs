using UnityEngine;

public class MeshEnemyController : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent agent;
    private AudioSource audioSource;

    public GameObject player;

    public float radioPatrulla = 5f;
    public float velocidad = 2f;
    public float tiempoCambio = 3f;
    public float radioDeteccion = 6f;

    public float umbralMirada = 0.6f;

    private Vector3 centro;
    private Vector3 destino;
    private float contadorTiempo;

    public float damage = 15f;

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        audioSource = GetComponent<AudioSource>(); // busca el AudioSource en el objeto

        agent.speed = velocidad;

        centro = transform.position;
        destino = ObtenerDestinoAleatorio();
        agent.SetDestination(destino);
    }

    void Update()
    {
        if (player == null) return;

        float distanciaJugador = Vector3.Distance(transform.position, player.transform.position);

        if (distanciaJugador <= radioDeteccion)
        {
            // 🔊 Reproduce sonido al entrar en rango
            if (audioSource != null && !audioSource.isPlaying)
            {
                audioSource.Play();
            }


            if (JugadorMirandoEnemigo())
            {
                agent.SetDestination(transform.position);
            }
            else
            {
                destino = player.transform.position;
                agent.SetDestination(destino);
            }
        }
        else
        {
            /*
            // 🔇 Opcional: detener sonido al salir del rango
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
            }*/

            contadorTiempo += Time.deltaTime;

            if (contadorTiempo >= tiempoCambio || agent.remainingDistance < 0.3f)
            {
                destino = ObtenerDestinoAleatorio();
                agent.SetDestination(destino);
                contadorTiempo = 0f;
            }
        }
    }

    bool JugadorMirandoEnemigo()
    {
        Vector3 direccionJugador = player.transform.forward;
        Vector3 direccionAlEnemigo = (transform.position - player.transform.position).normalized;

        float dot = Vector3.Dot(direccionJugador, direccionAlEnemigo);

        return dot > umbralMirada;
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

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Daño hecho");

        PlayerHealth.Instance.TakeDamage(damage);
    }
}