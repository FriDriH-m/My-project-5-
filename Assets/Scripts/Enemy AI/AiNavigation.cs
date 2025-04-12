using UnityEngine;
using UnityEngine.AI;

public class AiNavigation : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform player;
    void Update()
    {
        agent.destination = player.position;
    }
}
