using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;

public class MovementSystem : MonoBehaviour
{
    public BFSResult movementRange = new BFSResult();
    private List<Vector3Int> currentPath = new List<Vector3Int>();

   
    

    SelectionManager selectM;

    public static MovementSystem instance;
    #region Singleton
    private void Start()
    {
        instance = this;   

    }
    #endregion
    public void HideRange(HexGrid hexGrid)
    {
        foreach (Vector3Int hexPosition in movementRange.GetRangePositions())
        {
            hexGrid.GetTileAt(hexPosition).DisableHighlight();
        }
        movementRange = new BFSResult();
    }

    public void ShowRange(Unit selectedUnit, HexGrid hexGrid)
    {
        CalcualteRange(selectedUnit, hexGrid);

        Vector3Int unitPos = hexGrid.GetClosestHex(selectedUnit.transform.position);

        foreach (Vector3Int hexPosition in movementRange.GetRangePositions())
        {
            if (unitPos == hexPosition)
                continue;
            hexGrid.GetTileAt(hexPosition).EnableHighlight();
        }
    }

    public void CalcualteRange(Unit selectedUnit, HexGrid hexGrid)
    {
        movementRange = GraphSearch.BFSGetRange(hexGrid, hexGrid.GetClosestHex(selectedUnit.transform.position), selectedUnit.MovementPoints, selectedUnit);
    }


    public void ShowPath(Vector3Int selectedHexPosition, HexGrid hexGrid)
    {
        if (movementRange.GetRangePositions().Contains(selectedHexPosition))
        {
            foreach (Vector3Int hexPosition in currentPath)
            {
                hexGrid.GetTileAt(hexPosition).ResetHighlight();
            }
            currentPath = movementRange.GetPathTo(selectedHexPosition);
            foreach (Vector3Int hexPosition in currentPath)
            {
                hexGrid.GetTileAt(hexPosition).HighlightPath();
            }
        }
    }

    public void MoveUnit(Unit selectedUnit, HexGrid hexGrid)
    {
        if(selectedUnit.hasMoved == true)
        {
            return;
        }
        Debug.Log("Moving unit " + selectedUnit.name);
        selectedUnit.MoveThroughPath(currentPath.Select(pos => hexGrid.GetTileAt(pos).transform.position).ToList());
        selectedUnit.gameObject.GetComponentInChildren<Animator>().SetBool("Walking", true);
        

    }

    public bool IsHexInRange(Vector3Int hexPosition)
    {
        return movementRange.IsHexPositionInRange(hexPosition);
    }
}
