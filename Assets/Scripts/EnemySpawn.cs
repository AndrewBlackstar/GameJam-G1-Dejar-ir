using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public float spawnRate;
    [SerializeField] float enemyLife;
    private EnemyPool enemyPool;
    private bool hasPlayedSound = false; // Renombramos la bandera

    private void Start()
    {
        InvokeRepeating("SpawnEnemy", 2.5f, spawnRate);
        enemyPool = GetComponent<EnemyPool>();
    }

    void SpawnEnemy()
    {
        GameObject enemy = enemyPool.GetEnemy();

        if (enemy != null)
        {
            enemy.transform.position = transform.position;
            enemyPool.ReturnEnemy(enemy, enemyLife);

            // 🔥 Ahora el sonido se reproduce solo cuando el primer enemigo es correctamente generado
            if (!hasPlayedSound)
            {
                AudioManager.Instance.PlaySfx("EnemyInstance");
                hasPlayedSound = true; // Evita que el sonido se repita en siguientes enemigos
            }
        }
        else
        {
            Debug.Log("Pool vacía, inténtelo más tarde");
        }
    }
}
