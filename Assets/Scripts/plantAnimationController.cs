using UnityEngine;
using UnityEngine.AI;

public class PlantAnimationController : MonoBehaviour
{
    Animator animator;
    NavMeshAgent agent;

    void Start()
    {
        // Busca el Animator en hijos y el NavMeshAgent en el mismo objeto
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (animator == null || agent == null) return;

        // Detecta movimiento real del agente
        bool caminando = agent.velocity.magnitude > 0.1f;
        animator.SetBool("isWalking", caminando);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Detener movimiento
            agent.isStopped = true;

            // Activar animación de mordida
            animator.SetTrigger("bite");
            agent.isStopped = false;
        }
    }

    // Este método se llamará al final de la animación Bite mediante un Animation Event
    public void ReanudarMovimiento()
    {
        agent.isStopped = false;
    }
    
}