using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class GridPlottingStrategy: ScriptableObject
{
   public abstract Vector2 GetNodePosition(Vector3 tilePosition, Vector3 tileSize);
}
