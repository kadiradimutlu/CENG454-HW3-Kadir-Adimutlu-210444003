using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public float spawnInterval = 3f;
    private float timer = 0f;

    void Update()
    {
        if (Time.timeScale == 0f) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    private void SpawnEnemy()
    {
        if (spawnPoints.Length == 0 || enemyPrefab == null) return;

        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomIndex];

        GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        
        Obstacle obstacleComponent = newEnemy.GetComponent<Obstacle>();

        if (obstacleComponent != null)
        {
            if (Random.value > 0.5f)
            {
                obstacleComponent.SetStrategy(new ZigzagMovement());
            }
            else
            {
                obstacleComponent.SetStrategy(new DirectMovement());
            }
        }
    }
}