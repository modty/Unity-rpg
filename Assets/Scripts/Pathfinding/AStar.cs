using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;


public class AStar : MonoBehaviour
{
    [SerializeField]
    private Tilemap tilemap;

    private Node current;

    private Stack<Vector3> path;

    private HashSet<Node> openList;

    private HashSet<Node> closedList;

    private Dictionary<Vector3Int, Node> allNodes = new Dictionary<Vector3Int, Node>();

    private static HashSet<Vector3Int> noDiagonalTiles = new HashSet<Vector3Int>();



    private Vector3Int startPos, goalPos;

    public Tilemap MyTilemap
    {
        get
        {
            return tilemap;
        }
    }

    public static HashSet<Vector3Int> NoDiagonalTiles
    {
        get
        {
            return noDiagonalTiles;
        }
    }

    public Stack<Vector3> Algorithm(Vector3 start, Vector3 goal)
    {
        startPos = MyTilemap.WorldToCell(start);
        goalPos = MyTilemap.WorldToCell(goal);

        current = GetNode(startPos);

        openList = new HashSet<Node>();

        closedList = new HashSet<Node>();

        foreach (KeyValuePair<Vector3Int, Node> node in allNodes)
        {
            node.Value.Parent = null;
        }

        allNodes.Clear();

        openList.Add(current);

        path = null;

        while (openList.Count > 0 && path == null)
        {
            List<Node> neighbours = FindNeighbours(current.Position);

            ExamineNeighbours(neighbours, current);

            UpdateCurrentTile(ref current);

            path = GeneratePath(current);
        }

        if (path != null)
        {
            return path;
        }


        return null;

    }

    private List<Node> FindNeighbours(Vector3Int parentPosition)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (y != 0 || x != 0)
                {
                    Vector3Int neighbourPosition = new Vector3Int(parentPosition.x - x, parentPosition.y - y, parentPosition.z);

                    if (neighbourPosition != startPos && !GameManager.MyInstance.Blocked.Contains(neighbourPosition))
                    {
                        Node neighbour = GetNode(neighbourPosition);
                        neighbours.Add(neighbour);
                    }

                }
            }
        }

        return neighbours;
    }

    private void ExamineNeighbours(List<Node> neighbours, Node current)
    {
        for (int i = 0; i < neighbours.Count; i++)
        {
            Node neighbour = neighbours[i];

            if (!ConnectedDiagonally(current, neighbour))
            {
                continue;
            }

            int gScore = DetermineGScore(neighbour.Position, current.Position);

            if (gScore == 14 && NoDiagonalTiles.Contains(neighbour.Position) && NoDiagonalTiles.Contains(current.Position))
            {
                continue;
            }

            if (openList.Contains(neighbour))
            {
                if (current.G + gScore < neighbour.G)
                {
                    CalcValues(current, neighbour, goalPos, gScore);
                }
            }
            else if (!closedList.Contains(neighbour))
            {
                CalcValues(current, neighbour, goalPos, gScore);

                if (!openList.Contains(neighbour))
                {
                    openList.Add(neighbour);
                }
            }
        }
    }

    private bool ConnectedDiagonally(Node currentNode, Node neighbour)
    {
        Vector3Int direction = currentNode.Position - neighbour.Position;

        Vector3Int first = new Vector3Int(currentNode.Position.x + (direction.x * -1), currentNode.Position.y, currentNode.Position.z);
        Vector3Int second = new Vector3Int(currentNode.Position.x, currentNode.Position.y + (direction.y * -1), currentNode.Position.z);

        if (GameManager.MyInstance.Blocked.Contains(first) || GameManager.MyInstance.Blocked.Contains(second))
        {
            return false;
        }

        return true;
    }

    private int DetermineGScore(Vector3Int neighbour, Vector3Int current)
    {
        int gScore = 0;

        int x = current.x - neighbour.x;
        int y = current.y - neighbour.y;

        if (Math.Abs(x - y) % 2 == 1)
        {
            gScore = 10; 
        }
        else
        {
            gScore = 14;
        }

        return gScore;
    }

    private void UpdateCurrentTile(ref Node current)
    {
        openList.Remove(current);

        closedList.Add(current);

        if (openList.Count > 0)
        {
            current = openList.OrderBy(x => x.F).First();
        }
    }

    private Stack<Vector3> GeneratePath(Node current)
    {
        if (current.Position == goalPos)
        {
            Stack<Vector3> finalPath = new Stack<Vector3>();

            while (current != null)
            {
                finalPath.Push(MyTilemap.CellToWorld(current.Position));
                current = current.Parent;
            }

            return finalPath;
        }

        return null;

    }

    private void CalcValues(Node parent, Node neighbour, Vector3Int goalPos, int cost)
    {
        neighbour.Parent = parent;

        neighbour.G = parent.G + cost;

        neighbour.H = ((Math.Abs((neighbour.Position.x - goalPos.x)) + Math.Abs((neighbour.Position.y - goalPos.y))) * 10);

        neighbour.F = neighbour.G + neighbour.H;
    }



    private Node GetNode(Vector3Int position)
    {
        if (allNodes.ContainsKey(position))
        {
            return allNodes[position];
        }
        else
        {
            Node node = new Node(position);
            allNodes.Add(position, node);
            return node;
        }
    }
}

public class Node
{
    public int G { get; set; }
    public int H { get; set; }
    public int F { get; set; }
    public Node Parent { get; set; }
    public Vector3Int Position { get; set; }

    public Node(Vector3Int position)
    {
        this.Position = position;
    }
}

