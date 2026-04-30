using UnityEngine;

public interface IMovementStrategy
{
    void Move(Transform enemyTransform, Transform targetTransform, float speed);
}