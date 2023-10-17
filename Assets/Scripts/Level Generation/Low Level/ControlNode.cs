using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlNode : Node
{
    bool active;
    Node above, right;

    public ControlNode(Vector3 pos, bool isActive, float squareSize) : base(pos) {
        active = isActive;

        above = new Node(pos + Vector3.up * squareSize / 2f);
        right = new Node(pos + Vector3.right * squareSize / 2f);
    }

    public Node AboveNode {
        get => above;
    }

    public Node RightNode {
        get => right;
    }

    public bool Active {
        get => active;
    }
}
