using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a superclass for all Units in the game
/// Subclasses could be Monster or Player
/// </summary>
abstract public class Unit : MonoBehaviour
{
    /// <summary>
    /// The units movement speed
    /// </summary>
    [SerializeField]
    private float speed;

    /// <summary>
    /// The Unit's grid position
    /// </summary>
    public Point GridPosition { get; set; }

    /// <summary>
    /// A reference to the Unit's animator
    /// </summary>
    protected Animator myAnimator;

    /// <summary>
    /// A reference to the Unit's SpriteRenderer
    /// </summary>
    private SpriteRenderer spriteRenderer;

    /// <summary>
    /// This stack contains the path that the Unit can walk on
    /// This path should be generated with the AStar algorithm
    /// </summary>
    private Stack<Node> path;

    /// <summary>
    /// The unit's next destination
    /// </summary>
    private Vector3 destination;

    /// <summary>
    /// Indicates if the Unit is active
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Property for accessing and setting the Unit's speed
    /// Some debuffs might need this to be able to manipulate the Unit
    /// </summary>
    public float Speed
    {
        get
        {
            return speed;
        }

        set
        {
            this.speed = value;
        }
    }

    // Use this for initialization
    protected virtual void Awake()
    {
        //Sets up references to the components
        myAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void Update()
    {
        Move();
    }

    /// <summary>
    /// Animates the Unit according to the current action
    /// </summary>
    /// <param name="currentPos"></param>
    /// <param name="newPos"></param>
    public void Animate(Point currentPos, Point newPos)
    {
        //The code below animates the unit based on the moving direction

        //If we are moving down
        if (currentPos.Y > newPos.Y)
        {
            myAnimator.SetFloat("Horizontal", 0);

            myAnimator.SetFloat("Vertical", 1);
        }
        //IF we are moving up
        else if (currentPos.Y < newPos.Y)
        {
            myAnimator.SetFloat("Horizontal", 0);
            myAnimator.SetFloat("Vertical", -1);
        }
        //If we aren't moving up or down
        if (currentPos.Y == newPos.Y)
        {
            //If we are moving left
            if (currentPos.X > newPos.X)
            {
                myAnimator.SetFloat("Vertical", 0);
                myAnimator.SetFloat("Horizontal", -1);
            }
            //If we are moving right
            else if (currentPos.X < newPos.X)
            {
                myAnimator.SetFloat("Vertical", 0);
                myAnimator.SetFloat("Horizontal", 1);
            }
        }
        ////This is only ment for the player DEBUG ONLY
        //if (myAnimator.layerCount > 1)
        //{
        //    myAnimator.SetLayerWeight(1, 1);
        //}

    }

    /// <summary>
    /// Makes the unity move along the given path
    /// </summary>
    public void Move()
    {
        if (IsActive) //If the unit is active
        {
            //Move the unit towards the next destination
            transform.position = Vector2.MoveTowards(transform.position, destination, Speed * Time.deltaTime);

            //Checks if we arrived at the destination
            if (transform.position == destination)
            {
                //If we have a path and we have more nodes, then we need to keep moving
                if (path != null && path.Count > 0)
                {
                    //Animates the Unit based on the gridposition
                    Animate(GridPosition, path.Peek().GridPosition);

                    //Sets the new gridPosition
                    GridPosition = path.Peek().GridPosition;

                    //Sets a new destination
                    destination = path.Pop().WorldPosition;

                }
                else //If we don't have a path then we are done moving
                {
                    //THIS CODE IS ONLY MENT FOR THE PLAYER DEBUG ONLY
                    //if (myAnimator.layerCount > 1)
                    //{
                    //    myAnimator.SetLayerWeight(1, 0);
                    //}

                    //Sets the unit to inactive
                    IsActive = false;
                }
            }
        }
    }

    /// <summary>
    /// Gives the Unit a path to walk on
    /// </summary>
    /// <param name="newPath">The unit's new path</param>
    /// <param name="active">Indicates if the unit is active</param>
    public void SetPath(Stack<Node> newPath, bool active)
    {
        if (newPath != null) //If we have a path
        {
            //Sets the new path as the current path
            this.path = newPath;

            //Animates the Unit based on the gridposition
            Animate(GridPosition, path.Peek().GridPosition);

            //Sets the new gridPosition
            GridPosition = path.Peek().GridPosition;

            //Sets a new destination
            destination = path.Pop().WorldPosition;

            this.IsActive = active;
        }
    }

    /// <summary>
    /// This code is executed every time a unit makes a collision
    /// </summary>
    /// <param name="other">The other collider</param>
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Tile") //If we collide with a tile
        {
            //Change the sprite renderer's sorting layer, so that we don't get odd overlaps. This makes sure that the correct units are in the forground
            spriteRenderer.sortingOrder = (int)other.GetComponent<TileScript>().GridPosition.Y;
        }
    }

}