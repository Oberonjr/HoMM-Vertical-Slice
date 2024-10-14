using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICombatState
{
    void EnterState();
    void UpdateState();
    void ExitState();
}
