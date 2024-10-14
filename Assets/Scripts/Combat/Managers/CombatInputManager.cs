using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatInputManager : MonoBehaviour
{

    public GameObject temp;
    private GameObject guy;
    
    void Start()
    {
        guy = Instantiate(temp);
        CombatEventBus<MouseLeftClickEvent>.OnEvent += CheckInput;
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
        }
    }

    void CheckInput(MouseLeftClickEvent mEvent)
    {
        
        if (mEvent.position.stationedUnit != null)
        {
            Debug.Log(mEvent.position.stationedUnit.name + " is stationed on clicked node");
        }
        else
        {
            Debug.Log("No stationed on clicked node");
        }
    }
}
