using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HexGridStrategy", menuName = "Grid Node Generation/HexGridStrategy", order = 1)]
public class HexGridStrategy: INodePlotting
{

    public override Vector2 GetNodePosition(Vector3 tilePosition, Vector3 tileSize)
    {
        return CalculateHexWorldPosition(tilePosition.y, tilePosition.x, tileSize);
    }
        
    Vector2 CalculateHexWorldPosition(float x, float y, Vector3 tileSize)
    {
        
        float posX = y * tileSize.x + (x % 2 == 0 ? 0 : 1 / 2f); 
        
        
        float posY = x * (tileSize.x * 0.75f); 
        
        return new Vector2(posX, posY);
    }
}