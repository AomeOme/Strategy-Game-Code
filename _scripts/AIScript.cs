using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AIScript : MonoBehaviour
{
    [SerializeField]
    GameObject aiUnit;
    [SerializeField]
    Vector3 target;
    [SerializeField]
    Vector3 playerTarget;
    [SerializeField]
    Vector3 factoryTarget;
    [SerializeField]
    GameObject PlayerBase;
    [SerializeField]
    Vector3 wayPoint;
    [SerializeField]
    HexGrid hg;

    bool doingCombat = false;

    [SerializeField]
    LayerMask characterMask;
    [SerializeField]
    LayerMask factoryMask;
    [SerializeField] 
    LayerMask waypointMask;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void calculateClosestTarget()
    {
        Debug.Log("Function called");
        if (TurnManager.instance.aiTurn)
        {
            //check if can attack






            //start movement
            Debug.Log("Entered if statement");
            //target = Vector3Int.FloorToInt(PlayerBase.transform.position);

            Collider[] unitsInRange = Physics.OverlapSphere(aiUnit.transform.position, 2f, characterMask);
            Collider[] factoriesInRange = Physics.OverlapSphere(aiUnit.transform.position, 2f, factoryMask);
            Collider[] wayPointsinRange = Physics.OverlapSphere(aiUnit.transform.position, 2f, waypointMask);
            /*Debug.Log(factoriesInRange[0]);
            Debug.Log(unitsInRange[0]);
            Debug.Log(wayPointsinRange[0]);*/
            bool foundCloserPlayer = false;
            bool foundCloserFactory = false;
            bool foundCloserWaypoint = false;
            float closestPlayerDistance = 0f;
            float closestFactoryDistance = 0f;
            float closestWaypointDistance = 0f;
            float playerBaseDistance = Vector3Int.Distance(Vector3Int.FloorToInt(target), Vector3Int.FloorToInt(aiUnit.transform.position));

            target = PlayerBase.transform.position;

            foreach (Collider WayPoints in wayPointsinRange)
            {
                Debug.Log(WayPoints.name);
            }
            if (!foundCloserWaypoint)
            {
                Debug.Log("did not find closer waypoint");
            }

            foreach (Collider unit in unitsInRange)
            {
                Debug.Log(unit.name);
                
                if (Vector3Int.Distance(Vector3Int.FloorToInt(target), Vector3Int.FloorToInt(aiUnit.transform.position)) > Vector3Int.Distance(Vector3Int.FloorToInt(unit.transform.position), Vector3Int.FloorToInt(aiUnit.transform.position)))
                {
                    if (Vector3Int.Distance(Vector3Int.FloorToInt(aiUnit.transform.position), Vector3Int.FloorToInt(unit.transform.position)) < closestPlayerDistance)
                    {
                        //target = Vector3Int.FloorToInt(unit.transform.position);
                        Debug.Log("Found even closer player");
                        closestPlayerDistance = Vector3Int.Distance(Vector3Int.FloorToInt(aiUnit.transform.position), Vector3Int.FloorToInt(unit.transform.position));
                    }
                    closestPlayerDistance = Vector3Int.Distance(Vector3Int.FloorToInt(aiUnit.transform.position), Vector3Int.FloorToInt(unit.transform.position));
                    Debug.Log("player unit in range");
                    foundCloserPlayer = true;
                    playerTarget = unit.transform.position;
                }
            }
            if (!foundCloserPlayer)
            {
                Debug.Log("did not find closer player");
            }

            
            foreach(Collider factory in factoriesInRange)
            {
                Debug.Log(factory.name);
                if (Vector3Int.Distance(Vector3Int.FloorToInt(target), Vector3Int.FloorToInt(aiUnit.transform.position)) > Vector3Int.Distance(Vector3Int.FloorToInt(factory.transform.position), Vector3Int.FloorToInt(aiUnit.transform.position)))
                {
                    closestFactoryDistance = Vector3Int.Distance(Vector3Int.FloorToInt(aiUnit.transform.position), Vector3Int.FloorToInt(factory.transform.position));
                    foundCloserFactory = true;
                    Debug.Log("factroy in range");
                    factoryTarget = factory.transform.position;
                }
                else
                {
                    Debug.Log("fart");
                }

                if (!foundCloserFactory)
                {
                    Debug.Log("did not find closer factory");
                }



            }

            if(closestFactoryDistance == 0 && closestPlayerDistance != 0)
            {
                Debug.Log("Player is closer");
                target = playerTarget;
            }
            else if(closestPlayerDistance == 0 && closestFactoryDistance != 0)
            {
                Debug.Log("factory is closer");
                target = factoryTarget;
            }
            else if(closestFactoryDistance != 0 && closestPlayerDistance != 0)
            {
                if(closestPlayerDistance > closestFactoryDistance)
                {
                    Debug.Log("factory is closer");
                    target = factoryTarget;
                }
                else
                {
                    Debug.Log("Player is closer");
                    target = playerTarget;
                }
            }
            else
            {
                Debug.Log("no close factory or player");
            }

            

            
            //Hex h = hg.GetTileAt(Vector3Int.FloorToInt(target));
            /*if(h == null)
            {
                Debug.Log("break");
                return;
            }*/
            MovementSystem.instance.ShowRange(aiUnit.GetComponent<Unit>(), hg);
            if (!MovementSystem.instance.movementRange.IsHexPositionInRange(Vector3Int.FloorToInt(target)))
            {
                //var last = MovementSystem.instance.movementRange.visitedNodesDict.Values.Last();
                //target = last.Value;
                
                Debug.Log("thing isn't range");
                //return;
            }
            Hex h;
            h = hg.GetTileAt(hg.GetClosestHex(target));
            Debug.Log(h.name);
            MovementSystem.instance.ShowPath(h.HexCoords,hg);
            UnitManager.Instance.selectedUnit = aiUnit.GetComponentInChildren<Unit>();
            UnitManager.Instance.moveAiHexBs(h);
        }








    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(aiUnit.transform.position, 20);
    }



    private void OnTriggerStay(Collider other)
    {
        //Debug.Log(other.name);
        if(other.gameObject.layer == 9 && TurnManager.instance.aiTurn && !doingCombat)
        {
            doingCombat = true;
            //this.gameObject.GetComponent<CapsuleCollider>().isTrigger = true;
            Debug.Log("collided with player on ai turn, starting combat");
            //start combat
            
            UnitCombat.instance.doCombat(aiUnit, other.gameObject, hg.GetTileAt(hg.GetClosestHex(aiUnit.transform.position)).hexType, hg.GetTileAt(hg.GetClosestHex(other.gameObject.transform.position)).hexType);
        }
        else
        {
            //Debug.Log(other.gameObject.layer.ToString());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
