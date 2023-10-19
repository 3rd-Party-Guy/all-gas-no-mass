using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareGrid
{
    Square[,] squareGrid;

    public SquareGrid(int[,] map, float squareSize) {
        int nodeCountX = map.GetLength(0);
        int nodeCountY = map.GetLength(1);

        float mapWidth = nodeCountX * squareSize;
        float mapHeight = nodeCountY * squareSize;

        ControlNode[,] controlNodes = new ControlNode[nodeCountX, nodeCountY];

        for (int x = 0; x < nodeCountX; x++)
        for (int y = 0; y < nodeCountY; y++) {
            Vector3 pos = new Vector3(-mapWidth / 2f + x * squareSize + squareSize / 2f, -mapHeight / 2f + y * squareSize + squareSize / 2f, 0);
            controlNodes[x, y] = new ControlNode(pos, map[x, y] == 1, squareSize);
        }

        squareGrid = new Square[nodeCountX - 1, nodeCountY - 1];
        for (int x = 0; x < nodeCountX - 1; x++)
        for (int y = 0; y < nodeCountY - 1; y++) {
            squareGrid[x, y] = new Square(controlNodes[x, y + 1], controlNodes[x + 1, y + 1], controlNodes[x + 1, y], controlNodes[x, y]);
        }
    }

    public Node GetFreeNode() {
        int amountOfSquares = squareGrid.GetLength(0) + squareGrid.GetLength(1);
        
        for (int i = 0; i < amountOfSquares; i++) {
            Square s = squareGrid[Random.Range(0, squareGrid.GetLength(0)), Random.Range(0, squareGrid.GetLength(1))];
            ControlNode freeNode = s.GetFreeNode();
            if (freeNode == null) continue;
            return freeNode;
        }

        return null;                                                                    // no free squares left
    }

    public Square[,] Grid {
        get => squareGrid;
    }
}
