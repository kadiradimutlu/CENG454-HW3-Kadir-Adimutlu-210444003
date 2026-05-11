using UnityEngine;
using UnityEngine.AI;

public class ZigzagMovement : IMovementStrategy
{
    private float zigzagFrequency = 3f;
    private float zigzagAmplitude = 2f;

    public void Move(NavMeshAgent agent, Transform targetTransform, float speed)
    {
        if (targetTransform == null) return;

        agent.SetDestination(targetTransform.position);
        agent.speed = speed;

        Vector3 pathDirection = agent.desiredVelocity.normalized;
        
        if (pathDirection != Vector3.zero)
        {
            Vector3 rightDirection = Vector3.Cross(Vector3.up, pathDirection).normalized;
            Vector3 zigzagOffset = rightDirection * Mathf.Sin(Time.time * zigzagFrequency) * zigzagAmplitude;

            agent.velocity = (pathDirection * speed) + zigzagOffset;
        }
    }
}