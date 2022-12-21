using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Animations;

public class SelectionManager : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;

    public LayerMask selectionMask;
    public LayerMask enemyUnitSelectionMask;

    


    public UnityEvent<GameObject> OnUnitSelected;
    public UnityEvent<GameObject> TerrainSelected;
    public UnityEvent<GameObject> OnEnemySelected;
    UnitManager um = UnitManager.Instance;

    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    public void HandleClick(Vector3 mousePosition)
    {
        GameObject result;
        if (FindTarget(mousePosition, out result))
        {
            if (UnitSelected(result))
            {
                OnUnitSelected?.Invoke(result);
            }
            else
            {
                TerrainSelected?.Invoke(result);
            }
        }

        if(FindEnemy(mousePosition, out result))
        {
           if (EnemyUnit(result))
           {
                OnEnemySelected?.Invoke(result);
           }
           else
           {
                Debug.Log("why no worko");
           }
        }
    }

    private bool UnitSelected(GameObject result)
    {

        
        return result.GetComponent<Unit>() != null;

    }

    private bool EnemyUnit(GameObject result)
    {
        return result.GetComponent<Enemy>() != null;
    }

    private bool FindTarget(Vector3 mousePosition, out GameObject result)
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out hit, 100, selectionMask))
        {
            result = hit.collider.gameObject;
            return true;
        }
        result = null;
        return false;
    }

    private bool FindEnemy(Vector3 mousePosition, out GameObject result)
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out hit, 100, enemyUnitSelectionMask))
        {
            result = hit.collider.gameObject;
            Debug.Log("found enemy names"+hit.collider.gameObject.name);
            //um.hasTarget = true;
            return true;
        }
        result = null;
        return false;
    }
}
