using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SquareGridStrategy", menuName = "Grid Node Generation/SquareGridStrategy", order = 1)]
    public class SquareGridStrategy: GridPlottingStrategy
    {

        public override Vector2 GetNodePosition(Vector3 tilePosition, Vector3 tileSize)
        {
            return new Vector2(tilePosition.x + tileSize.x /2, tilePosition.y + tileSize.y /2);
        }
        
    }
