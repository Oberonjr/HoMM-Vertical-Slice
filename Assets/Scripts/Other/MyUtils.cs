using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyUtils
{
    public static IEnumerator LateStart(float seconds, System.Action action)
    {
        yield return new WaitForSeconds(seconds);
        action();
    }
}
