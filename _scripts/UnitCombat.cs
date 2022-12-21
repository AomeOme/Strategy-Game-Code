using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCombat : MonoBehaviour
{
    [SerializeField]
    GameObject attacker;
    [SerializeField]
    GameObject defender;
    [SerializeField]
    HexGrid hg;

    public static UnitCombat instance;

    // Start is called before the first frame update
    #region Singleton
    void Start()
    {
        instance = this;
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        
    }

    public void testButton()
    {
        doCombat(attacker, defender, hg.GetTileAt(hg.GetClosestHex(attacker.transform.position)).hexType, hg.GetTileAt(hg.GetClosestHex(defender.transform.position)).hexType);
    }

    public void doCombat(GameObject attackingUnit, GameObject defendingUnit, HexType attackingHexType, HexType defendingHexType)
    {
        Debug.Log(attackingUnit.name);
        Debug.Log(defendingUnit.name);
        float[] aStats = new float[3];
        float[] dStats = new float[3];
        Debug.Log(attackingHexType.ToString());
        Debug.Log(defendingHexType.ToString());
        //get attacking unit stats
        if (attackingUnit.transform.tag == "Infantry")
        {
            aStats[0] = attackingUnit.GetComponent<InfantryUSA>().health;
            aStats[1] = attackingUnit.gameObject.GetComponent<InfantryUSA>().attack;
            aStats[2] = attackingUnit.gameObject.GetComponent<InfantryUSA>().defense;
            Debug.Log("attacking unit is american infantry");
        }
        else if(attackingUnit.transform.tag == "InfantryGERMAN")
        {
            aStats[0] = attackingUnit.gameObject.GetComponent<InfantryGERMAN>().health;
            aStats[1] = attackingUnit.gameObject.GetComponent<InfantryGERMAN>().attack;
            aStats[2] = attackingUnit.gameObject.GetComponent<InfantryGERMAN>().defense;
            Debug.Log("attacking unit is german infantry");
        }
        else if(attackingUnit.transform.tag == "Tank")
        {
            aStats[0] = attackingUnit.gameObject.GetComponent<TankUSA>().health;
            aStats[1] = attackingUnit.gameObject.GetComponent<TankUSA>().attack;
            aStats[2] = attackingUnit.gameObject.GetComponent<TankUSA>().defense;
            Debug.Log("attacking unit is american tank");
        }
        else if(attackingUnit.transform.tag == "TankGERMAN")
        {
            aStats[0] = attackingUnit.gameObject.GetComponent<TankGERMAN>().health;
            aStats[1] = attackingUnit.gameObject.GetComponent<TankGERMAN>().attack;
            aStats[2] = attackingUnit.gameObject.GetComponent<TankGERMAN>().defense;
            Debug.Log("attacking unit is german tank");
        }
        //get defending unit stats
        if (defendingUnit.transform.tag == "Infantry")
        {
            dStats[0] = defendingUnit.gameObject.GetComponent<InfantryUSA>().health;
            dStats[1] = defendingUnit.gameObject.GetComponent<InfantryUSA>().attack;
            dStats[2] = defendingUnit.gameObject.GetComponent<InfantryUSA>().defense;
            Debug.Log("defending unit is american infantry");
        }
        else if (defendingUnit.transform.tag == "InfantryGERMAN")
        {
            dStats[0] = defendingUnit.gameObject.GetComponent<InfantryGERMAN>().health;
            dStats[1] = defendingUnit.gameObject.GetComponent<InfantryGERMAN>().attack;
            dStats[2] = defendingUnit.gameObject.GetComponent<InfantryGERMAN>().defense;
            Debug.Log("defending unit is german infantry");
        }
        else if (defendingUnit.transform.tag == "Tank")
        {
            dStats[0] = defendingUnit.gameObject.GetComponent<TankUSA>().health;
            dStats[1] = defendingUnit.gameObject.GetComponent<TankUSA>().attack;
            dStats[2] = defendingUnit.gameObject.GetComponent<TankUSA>().defense;
            Debug.Log("defending unit is american tank");
        }
        else if (defendingUnit.transform.tag == "TankGERMAN")
        {
            dStats[0] = defendingUnit.gameObject.GetComponent<TankGERMAN>().health;
            dStats[1] = defendingUnit.gameObject.GetComponent<TankGERMAN>().attack;
            dStats[2] = defendingUnit.gameObject.GetComponent<TankGERMAN>().defense;
            Debug.Log("defending unit is german tank");
        }

        if (defendingHexType == HexType.Difficult)
        {
            dStats[1] += 1;
            dStats[2] += 1;
        }
        else if (defendingHexType == HexType.Rough)
        {
            dStats[2] += 1;
        }
        else if (defendingHexType == HexType.Road)
        {
            dStats[2] -= 1;
        }

        while(aStats[0] > 0 || dStats[0] > 0)
        {
            
            //determine penalties/buffs based on terrain

            Debug.Log("defense " + dStats[2].ToString());
            Debug.Log("attack " + dStats[1].ToString());
            Debug.Log("defense " + aStats[2].ToString());
            Debug.Log("attack " + aStats[1].ToString());
            //
            float aRandom = Random.Range(1, 5);
            float dRandom = Random.Range(1, 5);

            Debug.Log(aRandom + " " + dRandom);

            //attack turn

            float attackingDamage = (aStats[1] / dStats[2]) * 7 * aRandom;

            dStats[0] = dStats[0] - attackingDamage;

            Debug.Log("attacked for " + attackingDamage.ToString() + " damage");
            if (dStats[0] < 0)
            {
                Debug.Log("Defending unit lost all health");
                defendingUnit.GetComponentInChildren<Animator>().SetBool("Death", true);
                //defendingUnit.gameObject.SetActive(false);
                StartCoroutine(WaitToDestory(defendingUnit));
                break;
            }

            //defend counterattack

            float defendingDamage = (dStats[1] / aStats[2] * 7 * dRandom);

            aStats[0] = aStats[0] - defendingDamage;

            Debug.Log("counter attacked for " + defendingDamage.ToString() + " damage");

            if (aStats[0] < 0)
            {
                Debug.Log("Attacking unit lost all health");
                attackingUnit.GetComponentInChildren<Animator>().SetBool("Death", true);
                StartCoroutine(WaitToDestory(attackingUnit));
                //attackingUnit.gameObject.SetActive(false);
                break;
            }

        }


    }


    IEnumerator WaitToDestory(GameObject toDestroy)
    {
        Debug.Log("start destroy");
        yield return new WaitForSeconds(3f);
        toDestroy.SetActive(false);
        Debug.Log("End destroy");
    }
}
