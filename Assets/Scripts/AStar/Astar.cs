using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

/// <summary>
/// This class contains all Astar functionality
/// The class can return a walkable path
/// </summary>
public static class AStar
{
    /// <summary>
    /// This dictionary contains all the nodes in our grid
    /// We are using a dictionary because it is faster than a list
    /// </summary>
    private static Dictionary<Point, Node> nodes;

    /// <summary>
    /// This function creates all the nodes in our grid
    /// </summary>
    private static void CreateNodes()
    {
        //Instantiates our dictionary
        nodes = new Dictionary<Point, Node>();

        //Runs through all the tiles in our game, if we have a tile, then we need to create a node
        //Tiles are the visual squares you see in the game. A node is something invisible we use for pathfinding
        foreach (TileScript tile in LevelManager.Instance.Tiles.Values)
        {
            //Creates a node based on the tile we just found
            nodes.Add(tile.GridPosition, new Node(tile));
        }
    }

    /// <summary>
    /// Returns a walkable path
    /// </summary>
    /// <param name="start">The start position of the path</param>
    /// <param name="goal">The end position of the path</param>
    /// <returns>A stack of nodes with the path, returns null if no path available</returns>
    public static Stack<Node> GetPath(Point start, Point goal)
    {
        if (nodes == null) //If we don't have any nodes, then we need to create them
        {
            CreateNodes();
        }

        //We need to reset our nodes, so that we can find a new path
        //If we don't reset our nodes, old values might be reused and we won't get the shortest path
        foreach (Node node in nodes.Values) 
        {
            node.Reset();
        }

        //Sets the current node as the start node, this is part of the Astar algorithm
        Node currentNode = nodes[start];

        //Creates an open list for nodes that we might want to look at later
        HashSet<Node> openList = new HashSet<Node>();

        //Creates a closed list for nodes that we have examined
        HashSet<Node> closedList = new HashSet<Node>();

        //Adds the current node to the open list (we have examined it)
        openList.Add(currentNode);

        while (openList.Count > 0) //As long as the openlist has nodes in it then we need to keep searching for a path
        {
            for (int x = -1; x <= 1; x++) //These two forloops makes sure that we all nodes around our current node
            {
                for (int y = -1; y <= 1; y++)
                {
                    //Stores the position of the current neighbour we are looking at
                    Point neighbourPos = new Point(currentNode.GridPosition.X - x, currentNode.GridPosition.Y - y);

                    //If there is a neighbout at the position, and it isn't start or the current node, then we need to examine it
                    if (LevelManager.Instance.InBounds(neighbourPos) && neighbourPos != start && LevelManager.Instance.Tiles[neighbourPos].IsEmpty && neighbourPos != currentNode.GridPosition)
                    {
                        //Stores a reference to the node
                        Node neighbour = nodes[new Point(neighbourPos.X, neighbourPos.Y)];

                        //Sets the gCost to 0 to makes sure that we get a value later
                        int gCost = 0;

                        //If the node is horizontal or vertical positioned
                        if (Math.Abs(x - y) % 2 == 1)
                        {
                            gCost = 10; //The gscore for a vertical or horizontal node is 10
                        }
                        else //If the node is diagonally positioned
                        {
                            if (!ConnectedDiagonally(currentNode, neighbour))
                            {
                                continue;
                            }

                            gCost = 14; //The gscore for a diagonally node is 14
                        }

                        if (openList.Contains(neighbour))//If the open list contains the neighbour
                        {
                            if (currentNode.G + gCost < neighbour.G) //Then we need to check if this node is a better parent
                            {
                                neighbour.CalcValues(currentNode, nodes[goal], gCost);
                            }
                        }
                        else if (!closedList.Contains(neighbour)) //If the openlist doesn't contain the neighbout and the close list doesn't contain the neighbour.
                        {
                            neighbour.CalcValues(currentNode, nodes[goal], gCost);//Then we need to calc the nodes values

                            if (!openList.Contains(neighbour)) //An extra check for openlist containing the neighbout
                            {
                                openList.Add(neighbour); //Then we need to add the node to the openlist
                            }
                        }
                    }
                }
            }

            //The current node is removed fromt he open list
            openList.Remove(currentNode);
              
            //The current node is added to the closed list
            closedList.Add(currentNode);

            if (openList.Count > 0) //If the openlist has nodes on it, then we need to sort them by it's F value
            {
                currentNode = openList.OrderBy(x => x.F).First();//Orders the list by the f value, to make it easier to pick the node with the lowest F val
            }

            if (currentNode == nodes[goal]) //If our current node is the goal, then we found a path
            {
                //Creates a stack to contain the final path
                Stack<Node> finalPath = new Stack<Node>();

                //Adds the nodes to the final path
                while (currentNode.GridPosition != start)
                {
                    //Adds the current node to the final path
                    finalPath.Push(currentNode);
                    //Find the parent of the node, this is actually retracing the whole path back to start
                    //By doing so, we will end up with a complete path.
                    currentNode = currentNode.Parent;
                }

                //Returns the complete path
                return finalPath;
            }
        }

        //If we didn't manage to find a path, then we return null
        return null;

    }

    /// <summary>
    /// A helper method, that determines if two nodes are connected diagonally without anything blocking the way
    /// </summary>
    /// <param name="currentNode">The first node</param>
    /// <param name="neighbour">The second node</param>
    /// <returns>True if the nodes are in bounds</returns>
    private static bool ConnectedDiagonally(Node currentNode, Node neighbour)
    {
        //Get's the direction
        Point direction = currentNode.GridPosition - neighbour.GridPosition;

        //Gets the positions of the nodes
        Point first = new Point(currentNode.GridPosition.X + (direction.X * -1), currentNode.GridPosition.Y);
        Point second = new Point(currentNode.GridPosition.X, currentNode.GridPosition.Y + (direction.Y * -1));

        //Checks if both nodes are empty
        if (LevelManager.Instance.InBounds(first) && !LevelManager.Instance.Tiles[first].IsEmpty)
        {
            return false;
        }
        if (LevelManager.Instance.InBounds(second) && !LevelManager.Instance.Tiles[second].IsEmpty)
        {
            return false;
        }

        //The ndoes are empty
        return true;
    }
}
