using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

/// <summary>
/// This Node class is used by the Astar algorithm
/// Each tile in the game has a node connected to it
/// It is Icompareable to make it sortable
/// </summary>
public class Node : IComparable<Node>
{
    /// <summary>
    /// The nodes position in the grid. for example 10,10
    /// </summary>
    public Point GridPosition { get; set; }

    /// <summary>
    /// The nodes position in the world, this is more a reference to the tile that the node is connected to
    /// </summary>
    public Vector2 WorldPosition { get; set; }

    /// <summary>
    /// The nodes parent, the parent is used by the Astar algorithm to backtrack when a path has been found
    /// </summary>
    public Node Parent { get; set; }

    /// <summary>
    /// The G score is the cost to move from start to the current node 
    /// 10 for vertical and horizontal
    /// 14 for diagonal moves
    /// </summary>
    public int G { get; set; }

    /// <summary>
    /// The nodes H score, H means hueristic and is a direct line from the node to the goal
    /// </summary>
    public int H { get; set; }

    /// <summary>
    /// F is the combined value of G + H
    /// </summary>
    public int F { get; set; }

    /// <summary>
    /// The node's constructor
    /// </summary>
    /// <param name="tileScript">The tile that this node is conencted to</param>
    public Node(TileScript tileScript)
    {
        //Sets the grid and world positions
        this.GridPosition = tileScript.GridPosition;
        this.WorldPosition = tileScript.WorldPosition;
    }

    /// <summary>
    /// Calculated the values that is used by the AStar algorithm
    /// </summary>
    /// <param name="parentNode">The parent node</param>
    /// <param name="goalNode">The goal node</param>
    /// <param name="cost">The cost of the move</param>
    public void CalcValues(Node parentNode, Node goalNode, int cost)
    {
        //Sets the parent node
        Parent = parentNode;

        //Calculates this nodes g cost, The parents g cost + what it costs to move tot his node
        G = parentNode.G + cost;

        //H is calucalted, it's the distance from this node to the goal * 10
        H = ((Math.Abs((int)(GridPosition.X - goalNode.GridPosition.X)) + Math.Abs((int)(goalNode.GridPosition.Y - GridPosition.Y))) * 10);

        //F is calcualted 
        F = G + H;
    }

    /// <summary>
    /// Resets the nodes values
    /// </summary>
    public void Reset()
    {
        G = 0;
        F = 0;
        H = 0;
        Parent = null;
    }

    /// <summary>
    /// CompareTo is used while sorting the list of nodes, this function is from the ICompareable interface
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public int CompareTo(Node other)
    {
        if (F > other.F) //If this F is higher, then we need to move it further down the list
        {
            return 1;
        }
        else if (F < other.F)  //If F is lower then we need to move it up the list
        {
            return -1;
        }

        return 0; //Both f values are the same, it doesn't matter which one we pick
    }

}
