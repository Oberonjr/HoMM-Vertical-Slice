using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex_Visualizer : MonoBehaviour
{
    
        public LineRenderer lineRenderer;

        public void HighlightMovementRange(Unit unit)
        {
            // Logic to calculate reachable hexes.
            List<Vector2Int> reachableHexes = GetReachableHexes(unit);

            foreach (var hex in reachableHexes)
            {
                // Use LineRenderer or shading to highlight hex tiles.
            }
        }

        private List<Vector2Int> GetReachableHexes(Unit unit)
        {
            // Basic pathfinding logic based on unit's movement points.
            // Use the hexagonal pathfinding algorithm.
            return null;
        }
    

}
