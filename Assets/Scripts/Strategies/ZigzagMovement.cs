using UnityEngine;

public class ZigzagMovement : IMovementStrategy
{
    private float zigzagFrequency = 3f;
    private float zigzagAmplitude = 2f;

    public void Move(Transform enemyTransform, Transform targetTransform, float speed)
    {
        if (targetTransform == null) return;

        Vector3 forwardDirection = (targetTransform.position - enemyTransform.position).normalized;
        
        Vector3 zigzagDirection = enemyTransform.right * Mathf.Sin(Time.time * zigzagFrequency) * zigzagAmplitude;

        enemyTransform.position += (forwardDirection * speed + zigzagDirection) * Time.deltaTime;
        enemyTransform.LookAt(targetTransform);
    }
}