using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    List<Vector3> vertices;
    List<int> triangles;

    Dictionary<int, List<Triangle>> triangleDictionary = new Dictionary<int, List<Triangle>>();
    List<List<int>> outlines = new List<List<int>>();
    HashSet<int> checkedVertices = new HashSet<int>();

    private SquareGrid squareGrid;
    MeshFilter walls;

    public void Clear() {
        vertices = new List<Vector3>();
        triangles = new List<int>();
        triangleDictionary = new Dictionary<int, List<Triangle>>();
        outlines = new List<List<int>>();
        checkedVertices = new HashSet<int>();
        squareGrid = null;
    }

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

        GenerateColliders();
    }

    public Vector3 GetFreePosition() {
        int playerLayerIndex = 6;
        int raycastLayerIndex = ~(1 << playerLayerIndex);

        for (int i = 0; i < 25; i++) {
            Vector3 potentialPosition = squareGrid.GetFreeNode().Position;
            Collider2D col = Physics2D.OverlapCircle(potentialPosition, 1f, raycastLayerIndex, 0f, 0f);
            if (col == null)
                return potentialPosition;
        }

        return Vector3.zero;
    }

#region Edge Triangulation
    private void GenerateColliders() {
        EdgeCollider2D[] curColliders = GetComponents<EdgeCollider2D>();
        for (int i = 0; i < curColliders.Length; i++)
            Destroy(curColliders[i]);

        CalculateMeshOutlines();

        foreach(List<int> outline in outlines) {
            EdgeCollider2D edgeCollider = gameObject.AddComponent<EdgeCollider2D>();
            Vector2[] edgePoints = new Vector2[outline.Count];
            
            for (int i = 0; i < outline.Count; i++) {
                edgePoints[i] = new Vector2(vertices[outline[i]].x, vertices[outline[i]].y);
            }

            edgeCollider.points = edgePoints;
        }
    }

    private void AddTriangleToDictionary(int vertexIndexKey, Triangle triangle) {
        if (triangleDictionary.ContainsKey(vertexIndexKey)) {
            triangleDictionary[vertexIndexKey].Add(triangle);
        } else {
            List<Triangle> triangleList = new List<Triangle>();
            triangleList.Add(triangle);
            triangleDictionary.Add(vertexIndexKey, triangleList);
        }

    }

    private void CalculateMeshOutlines() {
        for (int vertexIndex = 0; vertexIndex < vertices.Count; vertexIndex++) {
            if (checkedVertices.Contains(vertexIndex)) continue;
            int newOutlineVertex = GetConnectedOutlineVertex(vertexIndex);
            if (newOutlineVertex == -1) continue;

            checkedVertices.Add(vertexIndex);
            List<int> newOutline = new List<int>();
            newOutline.Add(vertexIndex);
            outlines.Add(newOutline);
            FollowOutline(newOutlineVertex, outlines.Count - 1);
            outlines[outlines.Count - 1].Add(vertexIndex);
        }
    }

    private void FollowOutline(int vertexIndex, int outlineIndex) {
        outlines[outlineIndex].Add(vertexIndex);
        checkedVertices.Add(vertexIndex);
        int nextVertexIndex = GetConnectedOutlineVertex(vertexIndex);

        if (nextVertexIndex != -1) FollowOutline(nextVertexIndex, outlineIndex);
    }

    private int GetConnectedOutlineVertex(int vertexIndex) {
        List<Triangle> trianglesWithVertex = triangleDictionary[vertexIndex];

        for (int i = 0; i < trianglesWithVertex.Count; i++) {
            Triangle triangle = trianglesWithVertex[i];

            for (int j = 0; j < 3; j++) {
                int vertexB = triangle[j];
                if (vertexB != vertexIndex && !checkedVertices.Contains(vertexB)) {
                    if (IsOutlineEdge(vertexIndex, vertexB))
                        return vertexB;
                }
            }
        }

        return -1;
    }

    private bool IsOutlineEdge(int vertexA, int vertexB) {
        List<Triangle> trianglesWithVertexA = triangleDictionary[vertexA];
        int sharedTriangleCount = 0;

        for (int i = 0; i < trianglesWithVertexA.Count; i++) {
            if (trianglesWithVertexA[i].Contains(vertexB)) {
                sharedTriangleCount++;
                if (sharedTriangleCount > 1) break;
            }
        }

        return (sharedTriangleCount == 1);
    }

#endregion

#region Mesh Generation

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

        Triangle triangle = new Triangle(a.VertexIndex, b.VertexIndex, c.VertexIndex);
        AddTriangleToDictionary(triangle.vertexIndexA, triangle);
        AddTriangleToDictionary(triangle.vertexIndexB, triangle);
        AddTriangleToDictionary(triangle.vertexIndexC, triangle);
    }

#endregion

    public SquareGrid Grid {
        get => squareGrid;
    }
}
