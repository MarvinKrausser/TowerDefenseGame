using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarNode
{
    public Vector2Int position;
    public bool walkAble;

    public int gCost;
    public int hCost;

    public AStarNode parent;

    public AStarNode(Vector2Int position, bool walkAble)
    {
        this.position = position;
        this.walkAble = walkAble;
    }

    public int fCost()
    {
        return gCost + hCost;
    }
}
