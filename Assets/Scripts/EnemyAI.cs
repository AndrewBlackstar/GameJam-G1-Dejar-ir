using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent agent;
    public float patrolRange = 5f;  // Rango para generar posiciones aleatorias
    public float detectionRange = 5f; // Rango de detección del jugador
    private Vector3 targetPatrolPoint; // Punto al que se dirige el enemigo

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Configurar NavMeshAgent para 2D
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        SetNewPatrolPoint(); // Elegir un primer destino aleatorio
    }

    void Update()
    {
        if (player != null && Vector3.Distance(transform.position, player.position) < detectionRange)
        {
            // Si el jugador está cerca, lo persigue
            if (agent.isOnNavMesh)
                agent.SetDestination(player.position);
        }
        else
        {
            PatrolRandomly();
        }
    }

    void PatrolRandomly()
    {
        // Si ha llegado al destino, elige otro punto aleatorio
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            SetNewPatrolPoint();
        }
    }

    void SetNewPatrolPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRange;
        randomDirection += transform.position; // Mueve el punto aleatorio cerca del enemigo

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, patrolRange, NavMesh.AllAreas))
        {
            targetPatrolPoint = hit.position;
            agent.SetDestination(targetPatrolPoint);
        }
    }
}
