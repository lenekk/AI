using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding
{
    public Grid<PathNode> grid;
    private List<PathNode> open;
    private List<PathNode> close;

    public static PathFinding Instance { get; set; }
    private const int STRAIGHT_COST = 10;
    private const int DIAGONAL_COST = 14;

    public PathFinding(int w, int h)
    {
        Instance = this;
        grid = new Grid<PathNode>(w, h, 10f, Vector3.zero, (Grid<PathNode> g, int x, int y) => new PathNode(g,x,y));
    }

    public List<Vector3> FindPath(Vector3 startworldpos, Vector3 endworldpos)
    {
        grid.getXY(startworldpos, out int startX, out int startY);
        grid.getXY(endworldpos, out int endX, out int endY);

        List<PathNode> path = FindPath(startX, startY, endX, endY);
        if(path == null)
        {
            return null;
        }
        else
        {
            List<Vector3> vectorPath = new List<Vector3>();
            foreach(PathNode pathNode in path)
            {
                vectorPath.Add(new Vector3(pathNode.x, pathNode.y) * grid.getCellSize() + Vector3.one * grid.getCellSize() * 0.5f);
            }

            return vectorPath;
        }
    }

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        PathNode startNode = grid.GetGridObject(startX, startY);
        PathNode endNode = grid.GetGridObject(endX, endY);
        open = new List<PathNode> { startNode };
        close = new List<PathNode>();

        for(int i=0; i<grid.getWidth(); i++)
        {
            for(int j=0; j<grid.getHeight(); j++)
            {
                PathNode pathnode = grid.GetGridObject(i, j);
                pathnode.gCost = int.MaxValue;
                pathnode.calculateFcost();
                pathnode.cameFromNode = null;
            }
        }

        if (endNode != null)
        {

            startNode.gCost = 0;
            startNode.hCost = CalculateDistanceCost(startNode, endNode);
            startNode.calculateFcost();

            while (open.Count > 0)
            {
                PathNode currentNode = getTheLowestFcostNode(open);
                if (currentNode == endNode)
                {
                    return calculatePath(endNode);
                }

                open.Remove(currentNode);
                close.Add(currentNode);

                foreach (PathNode neighbourNode in getNeighbours(currentNode))
                {
                    if (close.Contains(neighbourNode)) continue;
                    if (!neighbourNode.isWalkable)
                    {
                        close.Add(neighbourNode);
                        continue;
                    }
                    int tGcost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                    if (tGcost < neighbourNode.gCost)
                    {
                        neighbourNode.cameFromNode = currentNode;
                        neighbourNode.gCost = tGcost;
                        neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                        neighbourNode.calculateFcost();

                        if (!open.Contains(neighbourNode))
                        {
                            open.Add(neighbourNode);
                        }
                    }
                }
            }
        }
        return null;

    }

    private List<PathNode> getNeighbours(PathNode currentNode)
    {
        List<PathNode> neighbours = new List<PathNode>();

        if(currentNode.x -1 >= 0)
        {
            neighbours.Add(GetNode(currentNode.x - 1, currentNode.y));
            if(currentNode.y -1 >= 0)
            {
                neighbours.Add(GetNode(currentNode.x - 1, currentNode.y - 1));
            }
            if(currentNode.y + 1 < grid.getHeight())
            {
                neighbours.Add(GetNode(currentNode.x - 1, currentNode.y + 1));
            }
        }

        if(currentNode.x +1 < grid.getWidth())
        {
            neighbours.Add(GetNode(currentNode.x + 1, currentNode.y));
            if (currentNode.y - 1 >= 0)
            {
                neighbours.Add(GetNode(currentNode.x + 1, currentNode.y - 1));
            }
            if (currentNode.y + 1 < grid.getHeight())
            {
                neighbours.Add(GetNode(currentNode.x + 1, currentNode.y + 1));
            }
        }

        if(currentNode.y - 1 >= 0)
        {
            neighbours.Add(GetNode(currentNode.x, currentNode.y - 1));
        }

        if(currentNode.y + 1 < grid.getHeight())
        {
            neighbours.Add(GetNode(currentNode.x, currentNode.y + 1));
        }

        return neighbours;
    }

    public Grid<PathNode> GetGrid()
    {
        return grid;
    }


    public PathNode GetNode(int x, int y)
    {
        return grid.GetGridObject(x, y);
    }

    private List<PathNode> calculatePath(PathNode pathNode)
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(pathNode);
        PathNode currentNode = pathNode;
        while (currentNode.cameFromNode != null)
        {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }

        path.Reverse();

        return path;
    }

    private int CalculateDistanceCost(PathNode a, PathNode b)
    {
        
        int xDist = Mathf.Abs(a.x - b.x);
        int yDist = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDist - yDist);
        return DIAGONAL_COST * Mathf.Min(xDist, yDist) + STRAIGHT_COST * remaining;


    }

    private PathNode getTheLowestFcostNode(List<PathNode> pathNodelist)
    {
        PathNode lowestFCostNode = pathNodelist[0];
        for(int i=1; i < pathNodelist.Count; i++)
        {
            if(pathNodelist[i].fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = pathNodelist[i];
            }
        }
        return lowestFCostNode;
    }

}
