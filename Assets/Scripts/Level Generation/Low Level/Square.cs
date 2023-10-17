using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square
{
    ControlNode topLeft, topRight, bottomRight, bottomLeft;
    Node leftCenter, topCenter, rightCenter, bottomCenter;

    public Square(ControlNode topLeftNode, ControlNode topRightNode, ControlNode bottomRightNode, ControlNode bottomLeftNode) {
        topLeft = topLeftNode; topRight = topRightNode;
        bottomRight = bottomRightNode; bottomLeft = bottomLeftNode;

        leftCenter = bottomLeft.AboveNode; topCenter = topLeft.RightNode;
        rightCenter = bottomRight.AboveNode; bottomCenter = bottomLeft.RightNode;
    }

    public ControlNode TopLeft { get => topLeft; }
    public ControlNode TopRight { get => topRight; }
    public ControlNode BottomRight { get => bottomRight; }
    public ControlNode BottomLeft { get => bottomLeft; }

    public Node LeftCenter { get => leftCenter; }
    public Node TopCenter { get => topCenter; }
    public Node RightCenter { get => rightCenter; }
    public Node BottomCenter { get => bottomCenter; }
}
