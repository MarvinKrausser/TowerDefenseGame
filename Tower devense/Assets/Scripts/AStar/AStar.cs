using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar
{
    private AStarNode[,] nodes;
    private Vector2Int start;
    private Vector2Int end;
    
    public AStar(){}

    public void setNodes(List<List<int>> map)
    {
        nodes = new AStarNode[map.Count, map[0].Count];
        for (int x = 0; x < map.Count; x++)
        {
            for (int y = 0; y < map[0].Count; y++)
            {
                nodes[x, y] = new AStarNode(new Vector2Int(x, y), map[x][y] != 2);
                if (map[x][y] == 3)
                {
                    start = new Vector2Int(x, y);
                }
                else if(map[x][y] == 4)
                {
                    end = new Vector2Int(x, y);
                }
            }
        }
    }
    
    public void setNodes(int[,] map)
    {
        nodes = new AStarNode[map.GetLength(0), map.GetLength(1)];
        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                nodes[x, y] = new AStarNode(new Vector2Int(x, y), map[x,y] != 2 && map[x,y] != 5);
                if (map[x,y] == 3)
                {
                    start = new Vector2Int(x, y);
                }
                else if(map[x,y] == 4)
                {
                    end = new Vector2Int(x, y);
                }
            }
        }
    }
    
    
    public Vector2Int[] calculatePath()
    {
        AStarNode startNode = new AStarNode(start, true);
        AStarNode endNode = new AStarNode(end, true);

        List<AStarNode> openSet = new List<AStarNode>();
        HashSet<AStarNode> closedSet = new HashSet<AStarNode>();
        
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            AStarNode current = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost() < current.fCost() || (openSet[i].fCost() == current.fCost() && openSet[i].hCost < current.hCost))
                {
                    current = openSet[i];
                }
            }

            openSet.Remove(current);
            closedSet.Add(current);

            if (current.position.Equals(endNode.position))
            {
                return retracePath(current, startNode);
            }

            foreach (AStarNode node in getNeighbours(current))
            {
                if (!node.walkAble || closedSet.Contains(node))
                {
                    continue;
                }

                int newMovementCost = current.gCost + getDistance(current, node);
                if (newMovementCost < node.gCost || !openSet.Contains(node))
                {
                    node.gCost = newMovementCost;
                    node.hCost = getDistance(node, endNode);
                    node.parent = current;

                    if (!openSet.Contains(node))
                    {
                        openSet.Add(node);
                    }
                }
            }
            
        }

        return null;
    }

    private int getDistance(AStarNode nodeA, AStarNode nodeB)
    {
        return Mathf.Abs((nodeA.position.x - nodeB.position.x) + (nodeA.position.y - nodeB.position.y));
    }

    private Vector2Int[] retracePath(AStarNode endNode, AStarNode startNode)
    {
        AStarNode current = endNode;
        List<Vector2Int> path = new List<Vector2Int>();

        while (current != startNode)
        {
            path.Add(current.position);
            current = current.parent;
        }
        path.Add(startNode.position);
        
        path.Reverse();
        return path.ToArray();
    }

    private List<AStarNode> getNeighbours(AStarNode node)
    {
        List<AStarNode> neighbours = new List<AStarNode>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (Mathf.Abs(x) == Mathf.Abs(y))
                {
                    continue;
                }
                int checkX = node.position.x + x;
                int checkY = node.position.y + y;

                if (checkX >= 0 && checkX < nodes.GetLength(0) && checkY >= 0 && checkY < nodes.GetLength(1))
                {
                    neighbours.Add(nodes[checkX, checkY]);
                }
            }
        }
        return neighbours;
    }
}
