using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TurnManager : MonoBehaviour
{
    public bool playerTurn = true;
    public bool aiTurn = false;
    [SerializeField]
    GameObject PlayerEndTurnButton;
    [SerializeField]
    GameObject AIEndTurnButton;

    [SerializeField]
    public GameObject PlayerUnitParent;

    [SerializeField]
    GameObject AiUnitParent;

    #region Singleton  
    public static TurnManager instance;
    void Awake()
    {
        instance = this;
    }
    #endregion
    // Update is called once per frame
    void Update()
    {
        //players turn
        while (playerTurn)
        {
            //Debug.Log("player turn start");
            break;
        }
        //computers turn
        while (aiTurn)
        {
            //Debug.Log("computer turn start");
            break;
        }
    }

    public void playerButtonPress()
    {
        if (UnitManager.Instance.attacking)
        {
            return;
        }
        Debug.Log("player hit the button");
        PlayerEndTurnButton.SetActive(false);
        AIEndTurnButton.SetActive(true);
        playerTurn = false;
        aiTurn = true;
        UnitManager.Instance.attackButton.SetActive(false);
    }

    public void aiEndTurn()
    {
        //ai ended their turn
        PlayerEndTurnButton.SetActive(true);
        AIEndTurnButton.SetActive(false);
        resetMovementPlayer();
        aiTurn = false;
        playerTurn = true;
        UnitManager.Instance.attackButton.SetActive(true);
        for(int i = 0; i < AiUnitParent.transform.childCount; i++)
        {
            AiUnitParent.transform.GetChild(i).gameObject.GetComponent<Unit>().hasAttacked = false;
            AiUnitParent.transform.GetChild(i).gameObject.GetComponent<Unit>().hasAttacked = false;
        }
    }

    void resetMovementPlayer()
    {
        //GameObject[] children = new GameObject[PlayerUnitParent.transform.childCount];

        for(int i = 0; i < PlayerUnitParent.transform.childCount; i++)
        {
            PlayerUnitParent.transform.GetChild(i).gameObject.GetComponent<Unit>().hasMoved = false;
            PlayerUnitParent.transform.GetChild(i).gameObject.GetComponent<Unit>().hasAttacked = false;

        }
    }
}