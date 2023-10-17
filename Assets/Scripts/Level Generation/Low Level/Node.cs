using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    Vector3 position;
    int vertexIndex = -1;

    public Node(Vector3 pos) {
        position = pos;
    }

    public int VertexIndex {
        get => vertexIndex;
        set => vertexIndex = value;
    }

    public Vector3 Position {
        get => position;
    }
}
