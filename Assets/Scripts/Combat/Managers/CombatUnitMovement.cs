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
    }

    private void Start()
    {
        currentGrid = GridTracker.Instance.CombatGrid;
        CombatEventBus<UnitStartMovingEvent>.OnEvent += StartMovement;
    }
    

    void StartMovement(UnitStartMovingEvent e)
    {
        List<Node> path = Pathfinding.Instance.FindPath(e.unit.currentNodePosition.GridPosition, e.destination, currentGrid);
        StartCoroutine(MoveAlongPath(path));
    }
    
    public IEnumerator MoveAlongPath(List<Node> path)
    {
        Transform currentUnitTransform = currentUnit.transform;
        //Debug.Log(currentUnit.gameObject.name);
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
                //Debug.Log(Vector3.Distance(currentUnitTransform.position, targetPosition));
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
        currentUnit.isMoving = false;
        //Debug.Log("The node the unit ended is at: " + currentUnit.currentNodePosition.GridPosition);
        CombatEventBus<UnitEndMovingEvent>.Publish(new UnitEndMovingEvent(currentUnit));
    }
    
}
