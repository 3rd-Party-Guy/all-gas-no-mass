using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    [SerializeField] private float gridSizeMultiplierBig;
    [SerializeField] private float gridSizeMultiplierSmall;

    List<Vector3> vertices;
    List<int> triangles;

    private SquareGrid squareGrid;

    public void GenerateMesh(int[,] map, float squareSize) {
        squareGrid = new SquareGrid(map, squareSize);
        
        vertices = new List<Vector3>();
        triangles = new List<int>();

        for (int x = 0; x < squareGrid.Grid.GetLength(0); x++)
        for (int y = 0; y < squareGrid.Grid.GetLength(1); y++) {
            TriangulateSquare(squareGrid.Grid[x, y]);
        }

        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    private void TriangulateSquare(Square square) {
        switch (square.Configuration) {
        case 0: break;

        // 1 Point
        case 1:
            CreateMeshFromPoints(square.BottomCenter, square.BottomLeft, square.LeftCenter);
            break;
        case 2:
            CreateMeshFromPoints(square.RightCenter, square.BottomRight, square.BottomCenter);
            break;
        case 4:
            CreateMeshFromPoints(square.TopCenter, square.TopRight, square.RightCenter);
            break;
        case 8:
            CreateMeshFromPoints(square.TopLeft, square.TopCenter, square.LeftCenter);
            break;
        
        // 2 Points
        case 3:
            CreateMeshFromPoints(square.RightCenter, square.BottomRight, square.BottomLeft, square.LeftCenter);
            break;
        case 6:
            CreateMeshFromPoints(square.TopCenter, square.TopRight, square.BottomRight, square.BottomCenter);
            break;
        case 9:
            CreateMeshFromPoints(square.TopLeft, square.TopCenter, square.BottomCenter, square.BottomLeft);
            break;
        case 12:
            CreateMeshFromPoints(square.TopLeft, square.TopRight, square.RightCenter, square.LeftCenter);
            break;
        case 5:
            CreateMeshFromPoints(square.TopCenter, square.TopRight, square.RightCenter, square.BottomCenter, square.BottomLeft, square.LeftCenter);
            break;
        case 10:
            CreateMeshFromPoints(square.TopLeft, square.TopCenter, square.RightCenter, square.BottomRight, square.BottomCenter, square.LeftCenter);
            break;
        
        // 3 Points
        case 7:
            CreateMeshFromPoints(square.TopCenter, square.TopRight, square.BottomRight, square.BottomLeft, square.LeftCenter);
            break;
        case 11:
            CreateMeshFromPoints(square.TopLeft, square.TopCenter, square.RightCenter, square.BottomRight, square.BottomLeft);
            break;
        case 13:
            CreateMeshFromPoints(square.TopLeft, square.TopRight, square.RightCenter, square.BottomCenter, square.BottomLeft);
            break;
        case 14:
            CreateMeshFromPoints(square.TopLeft, square.TopRight, square.BottomRight, square.BottomCenter, square.LeftCenter);
            break;

        // 4 Points
        case 15:
            CreateMeshFromPoints(square.TopLeft, square.TopRight, square.BottomRight, square.BottomLeft);
            break;
        }
    }

    private void CreateMeshFromPoints(params Node[] points) {
        AssignVertices(points);

        if (points.Length >= 3)
            CreateTriangle(points[0], points[1], points[2]);
        if (points.Length >= 4)
            CreateTriangle(points[0], points[2], points[3]);
        if (points.Length >= 5)
            CreateTriangle(points[0], points[3], points[4]);
        if (points.Length >= 6)
            CreateTriangle(points[0], points[4], points[5]);
    }

    private void AssignVertices(Node[] points) {
        for (int i = 0; i < points.Length; i++) {
            if (points[i].VertexIndex == -1) {
                points[i].VertexIndex = vertices.Count;
                vertices.Add(points[i].Position);
            }
        }
    }

    private void CreateTriangle(Node a, Node b, Node c) {
        triangles.Add(a.VertexIndex);
        triangles.Add(b.VertexIndex);
        triangles.Add(c.VertexIndex);
    }

    // private void OnDrawGizmos() {
    //     if (squareGrid == null) return;
        
    //     for (int x = 0; x < squareGrid.Grid.GetLength(0); x++)
    //     for (int y = 0; y < squareGrid.Grid.GetLength(1); y++) {
    //         Gizmos.color = (squareGrid.Grid[x, y].TopLeft.Active) ? Color.black : Color.white;
    //         Gizmos.DrawCube(squareGrid.Grid[x, y].TopLeft.Position, Vector3.one * gridSizeMultiplierBig);
    //         Gizmos.color = (squareGrid.Grid[x, y].TopRight.Active) ? Color.black : Color.white;
    //         Gizmos.DrawCube(squareGrid.Grid[x, y].TopRight.Position, Vector3.one * gridSizeMultiplierBig);
    //         Gizmos.color = (squareGrid.Grid[x, y].BottomRight.Active) ? Color.black : Color.white;
    //         Gizmos.DrawCube(squareGrid.Grid[x, y].BottomRight.Position, Vector3.one * gridSizeMultiplierBig);
    //         Gizmos.color = (squareGrid.Grid[x, y].BottomLeft.Active) ? Color.black : Color.white;
    //         Gizmos.DrawCube(squareGrid.Grid[x, y].BottomLeft.Position, Vector3.one * gridSizeMultiplierBig);

    //         Gizmos.color = Color.gray;
    //         Gizmos.DrawCube(squareGrid.Grid[x, y].LeftCenter.Position, Vector3.one * gridSizeMultiplierSmall);
    //         Gizmos.DrawCube(squareGrid.Grid[x, y].TopCenter.Position, Vector3.one * gridSizeMultiplierSmall);
    //         Gizmos.DrawCube(squareGrid.Grid[x, y].RightCenter.Position, Vector3.one * gridSizeMultiplierSmall);
    //         Gizmos.DrawCube(squareGrid.Grid[x, y].BottomCenter.Position, Vector3.one * gridSizeMultiplierSmall);
    //     }
    // }

    public SquareGrid Grid {
        get => squareGrid;
    }
}
