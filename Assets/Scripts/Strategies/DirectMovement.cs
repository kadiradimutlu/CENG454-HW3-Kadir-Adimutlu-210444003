using UnityEngine;
using UnityEngine.AI;

public class DirectMovement : IMovementStrategy
{
    public void Move(NavMeshAgent agent, Transform targetTransform, float speed)
    {
        if (targetTransform == null) return;

        agent.speed = speed;
        agent.SetDestination(targetTransform.position);
    }
}