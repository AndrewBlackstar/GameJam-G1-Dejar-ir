using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public float spawnRate;
    [SerializeField] float enemyLife;
    EnemyPool enemyPool;
    
    


    private void Start()
    {
        InvokeRepeating("SpawnEnemy", 2.5f,spawnRate);
        AudioManager.Instance.PlaySfx("EnemyInstance");
        enemyPool = GetComponent<EnemyPool>();
    }

    void SpawnEnemy()
    {
        
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
