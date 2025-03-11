using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public GameObject enemyPrefabs;
    [SerializeField]int poolSize = 5;
    List<GameObject> pool = new List<GameObject>();

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Makepool(poolSize);
    }

    void Makepool(int size)
    {
        for (int i = 0; i < size; i++)
        {
            GameObject enemy = Instantiate(enemyPrefabs);
            enemy.SetActive(false);
            pool.Add(enemy);
            enemy.transform.parent = transform;
        }
    }

    
    public GameObject GetEnemy()
    {
        foreach (GameObject enemy in pool)
        {
            if (!enemy.activeSelf)
            {
                enemy.SetActive(true);
                return enemy;
            }
        }

        Debug.Log("no hay enemigos disponibles");
        return null;
    }

    public void ReturnEnemy(GameObject enemy, float delay)
    {
        StartCoroutine(DisableAfterTime(enemy, delay));
    }

    private IEnumerator DisableAfterTime(GameObject enemy, float delay)
    {
        yield return new WaitForSeconds(delay);
        enemy.SetActive(false);
    }
}
