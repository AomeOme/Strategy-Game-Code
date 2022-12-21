using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Hex : MonoBehaviour
{
    [SerializeField]
    private GlowHighlight highlight;
    private HexCoordinates hexCoordinates;

    [SerializeField]
    public HexType hexType;

    [SerializeField]
    public bool hasTroop;

    [SerializeField]
    public GameObject troopOnTile;

    public Vector3Int HexCoords => hexCoordinates.GetHexCoords();

    public int GetCost()
        => hexType switch
        {
            HexType.Difficult => 15,
            HexType.Rough => 10,
            HexType.Default => 5,
            HexType.Road => 4,
            _ => throw new Exception($"Hex of type {hexType} not supported")
        };

    public bool IsObstacle()
    {
        return this.hexType == HexType.Obstacle;
        
        
    }

    private void Awake()
    {
        hexCoordinates = GetComponent<HexCoordinates>();
        highlight = GetComponent<GlowHighlight>();
    }
    public void EnableHighlight()
    {
        highlight.ToggleGlow(true);
    }

    public void DisableHighlight()
    {
        highlight.ToggleGlow(false);
    }

    internal void ResetHighlight()
    {
        highlight.ResetGlowHighlight();
    }

    internal void HighlightPath()
    {
        highlight.HighlightValidPath();
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Infantry" || other.gameObject.tag == "InfantryGERMAN")
        {
            hasTroop = true;
        }
    }

    public void OnTriggerExit(Collider other2)
    {
        if(other2.gameObject.tag != "Infantry" || other2.gameObject.tag != "InfantryGERMAN")
        {
            hasTroop = false;
        }

        
    }
}

public enum HexType
{
    None,
    Default, //x1
    Rough,  
    Difficult,
    Road,
    Water,
    Obstacle
    
}


