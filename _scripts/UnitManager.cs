using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitManager : MonoBehaviour
{
    [SerializeField]
    private HexGrid hexGrid;

    [SerializeField]
    private MovementSystem movementSystem;

    public LayerMask enemyMask;
    [SerializeField]
    public GameObject enemy1 = null;


    public bool attacking = false;
    public bool hasTarget = false;

    Transform t = null;

    [SerializeField]
    public GameObject attackButton;
    //jacob is stupid
    //not being used, switching to TurnManger variable
    public bool PlayersTurn { get; private set; } = true;

    [SerializeField]
    public Unit selectedUnit;
    private Hex previouslySelectedHex;

    public static UnitManager Instance;
    #region Singleton
    private void Start()
    {
        Instance = this;
    }
    #endregion


    public void HandleUnitSelected(GameObject unit)
    {
        if (TurnManager.instance.playerTurn == false)
        {
            return;
        }
        Unit unitReference = unit.GetComponent<Unit>();
        

        t = unit.transform;
        if (attacking && !unitReference.hasAttacked)
        {
            foreach(Transform t2 in t)
            {
                if(t2.tag == "AttackRadius")
                {
                    t2.gameObject.SetActive(true);
                }
            }
            Collider[] enemiesInRange = Physics.OverlapSphere(unit.transform.position, 2.5f, enemyMask);
            foreach(Collider enemy in enemiesInRange)
            {
                Debug.Log("we hit" + enemy.name);
                hasTarget = true;
                /*if(hasTarget)
                {
                    UnitCombat.instance.doCombat(unit, enemy1);
                    hasTarget = false;
                    attacking = false;
                    unitReference.hasAttacked = true;
                }*/               
            }
        }
        else
        {
            attacking = false;
            if (unitReference.hasMoved || unitReference.isMoving || unitReference.hasAttacked)
            {
                return;
            }
            if (CheckIfTheSameUnitSelected(unitReference))
                return;

            PrepareUnitForMovement(unitReference);
        }       
    }

    /*private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(t.position, 2.5f);
    }*/

    private bool CheckIfTheSameUnitSelected(Unit unitReference)
    {
        if (this.selectedUnit == unitReference)
        {
            ClearOldSelection();
            return true;
        }
        return false;
    }

    public void HandleTerrainSelected(GameObject hexGO)
    {
        if (selectedUnit == null /*|| PlayersTurn == false*/)
        {
            return;
        }

        Hex selectedHex = hexGO.GetComponent<Hex>();

        if (HandleHexOutOfRange(selectedHex.HexCoords) || HandleSelectedHexIsUnitHex(selectedHex.HexCoords))
            return;

        HandleTargetHexSelected(selectedHex);

    }

    private void PrepareUnitForMovement(Unit unitReference)
    {
        if (this.selectedUnit != null)
        {
            ClearOldSelection();
        }

        this.selectedUnit = unitReference;
        this.selectedUnit.Select();
        movementSystem.ShowRange(this.selectedUnit, this.hexGrid);
    }

    private void ClearOldSelection()
    {
        previouslySelectedHex = null;
        this.selectedUnit.Deselect();
        movementSystem.HideRange(this.hexGrid);
        this.selectedUnit = null;

    }

    public void HandleTargetHexSelected(Hex selectedHex)
    {
        if (previouslySelectedHex == null || previouslySelectedHex != selectedHex)
        {
            previouslySelectedHex = selectedHex;
            movementSystem.ShowPath(selectedHex.HexCoords, this.hexGrid);
        }
        else
        {
            movementSystem.MoveUnit(selectedUnit, this.hexGrid);
            PlayersTurn = false;
            selectedUnit.MovementFinished += ResetTurn;
            ClearOldSelection();

        }
    }

    public void moveAiHexBs(Hex selectedHex)
    {
        movementSystem.MoveUnit(selectedUnit, this.hexGrid);
        //PlayersTurn = false;
        //selectedUnit.MovementFinished += ResetTurn;
        ClearOldSelection();

    }

    private bool HandleSelectedHexIsUnitHex(Vector3Int hexPosition)
    {
        if (hexPosition == hexGrid.GetClosestHex(selectedUnit.transform.position))
        {
            selectedUnit.Deselect();
            ClearOldSelection();
            return true;
        }
        return false;
    }

    private bool HandleHexOutOfRange(Vector3Int hexPosition)
    {
        if (movementSystem.IsHexInRange(hexPosition) == false)
        {
            Debug.Log("Hex Out of range!");
            return true;
        }
        return false;
    }

    private void ResetTurn(Unit selectedUnit)
    {
        selectedUnit.MovementFinished -= ResetTurn;
        PlayersTurn = true;
    }


    public void onAttackPressed()
    {
        if (!TurnManager.instance.playerTurn) { return; }
        attacking = !attacking;
        Image i = attackButton.GetComponent<Image>();
        if (attacking)
        {
            Debug.Log("changed to red");
            i.color = Color.red;
        }
        else
        {
            Transform h = TurnManager.instance.PlayerUnitParent.transform;
            foreach(Transform t in h)
            {
                foreach(Transform t2 in t)
                {
                    if(t2.tag == "AttackRadius")
                    {
                        t2.gameObject.SetActive(false);
                    }
                }
            }
            Debug.Log("changed to white");
            i.color = Color.white;
        }

        
    }

    public void HandleEnemyClickedDuringAttack(GameObject enemy)
    {
        //enemy1 = enemy;
        Debug.Log("you clicked on an ememy"+enemy.name);
        // hasTarget = true;
        Collider[] enemiesInRange = Physics.OverlapSphere(t.position, 2.5f, enemyMask);
        foreach (Collider enemy1 in enemiesInRange)
        {
            /*if(enemy1 != enemy)
            {
                return;
            }*/
            if(enemy1 == enemy)
            {
                break;
            }
        }
        if (attacking && hasTarget)
        {
            Hex hex = hexGrid.GetTileAt(hexGrid.GetClosestHex(enemy.transform.position));
            HexType aht = hex.hexType;
            Hex hex1 = hexGrid.GetTileAt(hexGrid.GetClosestHex(t.position));
            HexType dht = hex1.hexType;
            doTheCombat(t.gameObject, enemy, aht, dht);
        }
    }

    void doTheCombat(GameObject attacker, GameObject defender, HexType attackingHexType, HexType defendingHexType)
    {
        UnitCombat.instance.doCombat(attacker, defender, attackingHexType, defendingHexType);
        attacking = false;
        attacker.GetComponent<Unit>().hasAttacked = true;
        attackButton.GetComponent<Image>().color = Color.white;
        

        Transform t1 = attacker.transform;
        foreach (Transform t2 in t1)
        {
            if (t2.tag == "AttackRadius")
            {
                t2.gameObject.SetActive(false);
            }
        }
    }

    /*public IEnumerator WiatUntilTrue(bool fart)
    {
        while(!hasTarget)
        {
            yield return null;
        }
    }*/
}
