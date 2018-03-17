using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellGenerator : MonoBehaviour {

    public SquareGrid squareGrid;

    public void GenerateMesh(int[,] map, float squareSize)
    {
        squareGrid = new SquareGrid(map, squareSize);
    }

    void OnDrawGizmos()
    {
        if(squareGrid != null)
        {
            for (int i = 0; i < squareGrid.squares.GetLength(0); i++)
            {
                for(int j = 0; j < squareGrid.squares.GetLength(1); j++)
                {
                    Gizmos.color = squareGrid.squares[i, j].topLeft.active ? Color.black : Color.white;
                    Gizmos.DrawCube(squareGrid.squares[i, j].topLeft.position, Vector3.one * 0.4f);

                    Gizmos.color = squareGrid.squares[i, j].topRight.active ? Color.black : Color.white;
                    Gizmos.DrawCube(squareGrid.squares[i, j].topRight.position, Vector3.one * 0.4f);

                    Gizmos.color = squareGrid.squares[i, j].bottomRight.active ? Color.black : Color.white;
                    Gizmos.DrawCube(squareGrid.squares[i, j].bottomRight.position, Vector3.one * 0.4f);

                    Gizmos.color = squareGrid.squares[i, j].bottomLeft.active ? Color.black : Color.white;
                    Gizmos.DrawCube(squareGrid.squares[i, j].bottomLeft.position, Vector3.one * 0.4f);

                    Gizmos.color = Color.grey;
                    Gizmos.DrawCube(squareGrid.squares[i, j].centreTop.position, Vector3.one * 0.15f);
                    Gizmos.DrawCube(squareGrid.squares[i, j].centreRight.position, Vector3.one * 0.15f);
                    Gizmos.DrawCube(squareGrid.squares[i, j].centreBottom.position, Vector3.one * 0.15f);
                    Gizmos.DrawCube(squareGrid.squares[i, j].centreLeft.position, Vector3.one * 0.15f);
                }
            }
        }
    }

    public class SquareGrid
    {
        public Square[,] squares;

        public SquareGrid(int[,] map, float squareSize)
        {
            int nodeCountX = map.GetLength(0);
            int nodeCountY = map.GetLength(1);
            float mapWidth = nodeCountX * squareSize;
            float mapHeight = nodeCountY * squareSize;

            ControlNode[,] controlNodes = new ControlNode[nodeCountX, nodeCountY];

            for(int i = 0; i < nodeCountX; i++)
            {
                for(int j = 0; j < nodeCountY; j++)
                {
                    Vector3 pos = new Vector3(-mapWidth / 2 + i * squareSize + squareSize / 2, 0, -mapHeight / 2 + j * squareSize + squareSize / 2);
                    controlNodes[i, j] = new ControlNode(pos, map[i, j] == 1, squareSize);
                }
            }
            squares = new Square[nodeCountX - 1, nodeCountY - 1];

            for (int i = 0; i < nodeCountX - 1; i++)
            {
                for (int j = 0; j < nodeCountY - 1; j++)
                {
                    squares[i, j] = new Square(controlNodes[i, j + 1], controlNodes[i + 1, j + 1], controlNodes[i + 1, j], controlNodes[i, j]);
                }
            }
        }
    }

    public class Square
    {
        public ControlNode topLeft, topRight, bottomRight, bottomLeft;
        public Node centreTop, centreRight, centreBottom, centreLeft;

        public Square(ControlNode _topLeft, ControlNode _topRight, ControlNode _bottomRight, ControlNode _bottomLeft)
        {
            topLeft = _topLeft;
            topRight = _topRight;
            bottomRight = _bottomRight;
            bottomLeft = _bottomLeft;

            centreTop = topLeft.right;
            centreRight = bottomRight.above;
            centreBottom = bottomLeft.right;
            centreLeft = bottomLeft.above;
        }
    }

    public class Node
    {
        public Vector3 position;
        public int vertexIndex;

        public Node(Vector3 pos)
        {
            position = pos;
        }
    }

    public class ControlNode : Node
    {
        public bool active;
        public Node above, right;

        public ControlNode(Vector3 pos, bool _active, float squareSize) : base(pos)
        {
            active = _active;
            above = new Node(position + Vector3.forward * squareSize / 2f);
            right = new Node(position + Vector3.right * squareSize / 2f);
        }
    }
}
