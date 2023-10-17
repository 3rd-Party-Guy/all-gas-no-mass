using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    [SerializeField] private float gridSizeMultiplierBig;
    [SerializeField] private float gridSizeMultiplierSmall;

    private SquareGrid squareGrid;

    public void GenerateMesh(int[,] map, float squareSize) {
        squareGrid = new SquareGrid(map, squareSize);
    }

    private void OnDrawGizmos() {
        if (squareGrid == null) return;
        
        for (int x = 0; x < squareGrid.Grid.GetLength(0); x++)
        for (int y = 0; y < squareGrid.Grid.GetLength(1); y++) {
            Gizmos.color = (squareGrid.Grid[x, y].TopLeft.Active) ? Color.black : Color.white;
            Gizmos.DrawCube(squareGrid.Grid[x, y].TopLeft.Position, Vector3.one * gridSizeMultiplierBig);
            Gizmos.color = (squareGrid.Grid[x, y].TopRight.Active) ? Color.black : Color.white;
            Gizmos.DrawCube(squareGrid.Grid[x, y].TopRight.Position, Vector3.one * gridSizeMultiplierBig);
            Gizmos.color = (squareGrid.Grid[x, y].BottomRight.Active) ? Color.black : Color.white;
            Gizmos.DrawCube(squareGrid.Grid[x, y].BottomRight.Position, Vector3.one * gridSizeMultiplierBig);
            Gizmos.color = (squareGrid.Grid[x, y].BottomLeft.Active) ? Color.black : Color.white;
            Gizmos.DrawCube(squareGrid.Grid[x, y].BottomLeft.Position, Vector3.one * gridSizeMultiplierBig);

            Gizmos.color = Color.gray;
            Gizmos.DrawCube(squareGrid.Grid[x, y].LeftCenter.Position, Vector3.one * gridSizeMultiplierSmall);
            Gizmos.DrawCube(squareGrid.Grid[x, y].TopCenter.Position, Vector3.one * gridSizeMultiplierSmall);
            Gizmos.DrawCube(squareGrid.Grid[x, y].RightCenter.Position, Vector3.one * gridSizeMultiplierSmall);
            Gizmos.DrawCube(squareGrid.Grid[x, y].BottomCenter.Position, Vector3.one * gridSizeMultiplierSmall);
        }
    }

    public SquareGrid Grid {
        get => squareGrid;
    }
}
