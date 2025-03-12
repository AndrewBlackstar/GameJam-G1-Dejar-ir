using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    int maxLives = 4;
    int currentLives;
    public GameObject[] lifeIcons; 


    // Start is called before the first frame update
    void Start()
    {
        
        currentLives = maxLives;
        
        updateLifes();
    }


    public void Takedamage(bool cur)
    {
        currentLives--;
        AudioManager.Instance.PlaySfx("Hit");
        updateLifes() ;
        if (currentLives == 0)
        {
            GameManager.GameInstance.GameOverLose();
            
        }
    }

    
    void updateLifes()
    {
        for (int i = 0; i < lifeIcons.Length; i++)
        {
            lifeIcons[i].SetActive(i < currentLives);
        }
    }

    public void ResetLifes()
    {
        currentLives =maxLives;
        updateLifes();
    }
}
