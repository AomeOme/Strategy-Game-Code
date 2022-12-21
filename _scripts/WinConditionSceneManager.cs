using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinConditionSceneManager : MonoBehaviour
{
    
    public GameObject playerUnitParent;
    public GameObject enemyUnitsParent;

    bool hasPlayerUnits = true;
    bool hasEnemyUnits = true;

    // Start is called before the first frame update
    void Start()
    {
        hasEnemyUnits = true;
        hasPlayerUnits = true;
        CheckForPlayerAndEnemyAmount();
       
    }

    // Update is called once per frame
    void Update()
    {
        CheckForPlayerAndEnemyAmount();
        if(hasPlayerUnits == false)
        {
            Fail();
        }

        if(hasEnemyUnits == false)
        {
            Win();
        }
        
    }

    void CheckForPlayerAndEnemyAmount()
    {
        float playerAliveUnits = 0f;
        float enemyAliveunits = 0f;
        for (int i = 0; i < playerUnitParent.transform.childCount; i++)
        {
            if (playerUnitParent.transform.GetChild(i).gameObject.activeInHierarchy)
            {
                hasPlayerUnits = true;
                playerAliveUnits++;
                
            }

        }

        for (int i = 0; i < enemyUnitsParent.transform.childCount; i++)
        {
            if (enemyUnitsParent.transform.GetChild(i).gameObject.activeInHierarchy)
            {
                hasEnemyUnits = true;
                enemyAliveunits++;
                
            }

        }


        if(enemyAliveunits == 0f)
        {
            hasEnemyUnits = false;
        }

        if (playerAliveUnits == 0f)
        {
            hasPlayerUnits = false;
        }
    }

    void Fail()
    {
        Debug.Log("You lose");
    }

    void Win()
    {
        Debug.Log("You Win");
        Time.timeScale = 0;
    }
}
