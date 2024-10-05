using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector2 GridPosition; 
    public bool IsWalkable; 
    public Node ParentNode; //Obsolete, should work on removing
    public BoxCollider2D Collider;
    public GameObject spriteHighlight;
    public Dictionary<Node, float> neighbours = new Dictionary<Node, float>(); //Neighbour nodes stored with total cost for pathfinding
    public float GCost; // Cost from the start node
    public float HCost; // Heuristic cost to the end node
    public int MovementCost; // Movement cost for this tile

    public float FCost { get { return GCost + HCost; } } // Total cost

    public Node(Vector2 gridPosition, bool isWalkable, int movementCost)
    {
        GridPosition = gridPosition;
        IsWalkable = isWalkable;
        MovementCost = movementCost;
    }
}

