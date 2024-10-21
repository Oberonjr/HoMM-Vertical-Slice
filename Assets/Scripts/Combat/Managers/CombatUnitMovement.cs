using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class CombatUnitMovement : MonoBehaviour
{
    public static CombatUnitMovement Instance;
    
    [HideInInspector]public Unit currentUnit;
    
    [SerializeField] private float animationSpeed = 3;
    private Dictionary<Vector2, Node> currentGrid;
    
    private List<Node> currentPath;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
        currentGrid = GridManager.Instance.grid;
    }

    private void Start()
    {
        CombatEventBus<UnitStartMovingEvent>.OnEvent += StartMovement;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartMovement(UnitStartMovingEvent e)
    {
        List<Node> path = Pathfinding.Instance.FindPath(e.unit.currentNodePosition.GridPosition, e.destination, GridManager.Instance.grid);
        StartCoroutine(MoveAlongPath(path));
    }
    
    public IEnumerator MoveAlongPath(List<Node> path)
    {
        Transform currentUnitTransform = currentUnit.transform;
        currentUnit.isMoving = true;
        int tilesMoved = 0;
        
        foreach (Node node in path)
        {
            if (!currentUnit.CanMove(1))
            {
                break;
            }

            Vector3 targetPosition = node.GridPosition;
            while (Vector3.Distance(currentUnitTransform.position, targetPosition) > Mathf.Epsilon)
            {
                currentUnitTransform.position = Vector3.MoveTowards(currentUnitTransform.position, targetPosition, animationSpeed * Time.deltaTime);
                yield return null;
            }
            currentUnitTransform.position = targetPosition;
            currentUnit.currentNodePosition.stationedUnit = null;
            currentUnit.currentNodePosition = node;
            currentUnit.currentNodePosition.stationedUnit = currentUnit;
            currentUnit.UseMovement(1);
            CombatEventBus<UnitMovedEvent>.Publish(new UnitMovedEvent(currentUnit, node));
            //yield return null;
            tilesMoved++;
        }
        path = new List<Node>();
        currentUnit.isMoving = false;
        //Debug.Log("The node the unit ended is at: " + currentUnit.currentNodePosition.GridPosition);
        CombatEventBus<UnitEndMovingEvent>.Publish(new UnitEndMovingEvent(currentUnit));
        //CombatTurnManager.Instance.EndTurn();
    }
    
    public void SnapToGridCenter(Unit unit)
    {
        Vector3Int playerGridPosition = GridManager.Instance.tilemap.WorldToCell(unit.transform.position);
        Vector3 snappedPosition = GridManager.Instance.tilemap.GetCellCenterWorld(playerGridPosition);
        unit.currentNodePosition = ClosestNode(snappedPosition);
        unit.transform.position = snappedPosition;
    }
    
    Node ClosestNode(Vector2 clickedPosition)
    {
        float shortestDistance = Mathf.Infinity;
        Node closestNode = null;
        foreach (Node node in currentGrid.Values)
        {
            float specificDistance = Vector2.Distance(node.GridPosition, clickedPosition);
            if (specificDistance < shortestDistance)
            {
                shortestDistance = specificDistance;
                closestNode = node;
            }
        }
        //Debug.Log("Mouse click position is: " + clickedPosition);
        //Debug.Log("Closest node attributed to that is: " + closestNode.GridPosition);
        if (Vector2.Distance(clickedPosition, closestNode.GridPosition) <= GridManager.Instance.tileSize.x / 2)
        {
            return closestNode;
        }
        else
        {
            Debug.Log("Clicked position is too far from closest eligible node.");
            return null;
        }
        
    }
}
