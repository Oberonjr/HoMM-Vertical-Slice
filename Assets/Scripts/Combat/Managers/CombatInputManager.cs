using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatInputManager : MonoBehaviour
{
    public GameObject temp; //couldve been tempHoverPrefab
    private GameObject guy;
    
    void Start()
    {
        guy = Instantiate(temp);
    }
    
    void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (MyUtils.ClosestNode(mousePosition) == null) return;
        Node clickedNode = MyUtils.ClosestNode(mousePosition);
        guy.transform.position = clickedNode.GridPosition;
        if (Input.GetMouseButtonDown(0))
        {
            CombatEventBus<MouseLeftClickEvent>.Publish(new MouseLeftClickEvent(clickedNode));
            //Debug.Log("Clicked node position is: " + clickedNode.IsWalkable);
        }
    }

    
}
