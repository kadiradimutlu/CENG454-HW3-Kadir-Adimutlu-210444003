using UnityEngine;
using UnityEngine.AI;

public interface IMovementStrategy
{
    void Move(NavMeshAgent agent, Transform targetTransform, float speed);
}