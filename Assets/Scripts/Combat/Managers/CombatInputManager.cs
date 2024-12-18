using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatInputManager : MonoBehaviour
{
    private Camera currentCamera;

    void Start()
    {
        currentCamera = Camera.main;
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = currentCamera.ScreenToWorldPoint(Input.mousePosition);
            if (MyUtils.ClosestNode(mousePosition) == null) return;
            Node clickedNode = MyUtils.ClosestNode(mousePosition);
            CombatEventBus<MouseLeftClickEvent>.Publish(new MouseLeftClickEvent(clickedNode));
            //Debug.Log("Clicked node position is: " + clickedNode.IsWalkable);
        }
    }

    
}
