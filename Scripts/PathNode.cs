using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    private Grid<PathNode> grid;
    public int x;
    public int y;
    
   
    public int gCost;
    public int fCost;
    public int hCost;

    public PathNode cameFromNode;

    public bool isWalkable;

    public void calculateFcost()
    {
        fCost = gCost + hCost;
    }

    public PathNode(Grid<PathNode> grid, int x, int y)
    {
        this.x = x;
        this.y = y;
        this.grid = grid;
        isWalkable = true;
    }

    public void setToNotWalkable()
    {
        isWalkable = false;
    }
}
