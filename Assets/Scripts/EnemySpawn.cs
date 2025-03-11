using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public float spawnRate;
    [SerializeField] float enemyLife;
    EnemyPool enemyPool;
    
    


    private void Start()
    {
        InvokeRepeating("SpawnEnemy", 2.5f,spawnRate);
        enemyPool = GetComponent<EnemyPool>();
    }

    void SpawnEnemy()
    {
        //float randomX = Random.Range(-spawnRangeX, spawnRangeX);
        //Vector2 spawnPos = new Vector2(randomX, spawnPoint.position.y);
        GameObject enemy = enemyPool.GetEnemy();

        if (enemy != null)
        {
            enemy.transform.position = transform.position;
            enemyPool.ReturnEnemy(enemy, enemyLife);
        }
        else
        {
            Debug.Log("pool vacia intentelo mas tarde");
        }
    }
}
