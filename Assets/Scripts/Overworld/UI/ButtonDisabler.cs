using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonDisabler : MonoBehaviour
{
    public Button[] buttons;

    void Start()
    {
        buttons = transform.parent.GetComponentsInChildren<Button>(true);
    }
    
    void OnEnable()
    {
        foreach (Button button in buttons)
        {
            button.enabled = false;
        }
    }

    void OnDisable()
    {
        foreach (Button button in buttons)
        {
            button.enabled = true;
        }
    }
}
