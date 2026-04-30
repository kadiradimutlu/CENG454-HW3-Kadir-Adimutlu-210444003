using UnityEngine;

public class DirectMovement : IMovementStrategy
{
    public void Move(Transform enemyTransform, Transform targetTransform, float speed)
    {
        if (targetTransform == null) return;

        Vector3 direction = (targetTransform.position - enemyTransform.position).normalized;
        enemyTransform.position += direction * speed * Time.deltaTime;
        
        enemyTransform.LookAt(targetTransform);
    }
}