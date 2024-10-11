using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CombatInputManager : MonoBehaviour
{

    public GameObject temp;
    
    private GameObject guy;
    // Start is called before the first frame update
    void Start()
    {
        guy = Instantiate(temp);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (MyUtils.ClosestNode(mousePosition) == null) return;
        Node clickedNode = MyUtils.ClosestNode(mousePosition);
        guy.transform.position = clickedNode.GridPosition;
    }
}
