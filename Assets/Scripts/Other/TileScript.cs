using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

/// <summary>
/// The tile script
/// </summary>
public class TileScript : MonoBehaviour
{
    /// <summary>
    /// The tile's grid position
    /// </summary>
    public Point GridPosition { get; private set; }

    /// <summary>
    /// Indicates if the tile is empty or not
    /// </summary>
    public bool IsEmpty { get; set; }

    /// <summary>
    /// the color of the tile, when its full, this is used while hovering the tile with the mouse
    /// </summary>
    private Color32 fullColor = new Color32(255, 118, 118, 255);

    /// <summary>
    /// The color of the tile when is empty, this is used when hover the tile with the mouse
    /// </summary>
    private Color32 emptyColor = new Color32(96, 255, 90, 255);

    /// <summary>
    /// A reference to the tiles sprite renderer
    /// </summary>
    private SpriteRenderer spriteRenderer;

    /// <summary>
    /// A reference to the tower on the tile. If the tile is empty, this is null
    /// </summary>
    private Tower myTower;

    /// <summary>
    /// The tiles worldposition
    /// </summary>
    public Vector2 WorldPosition
    {
        get
        {
            return new Vector2(transform.position.x + (GetComponent<SpriteRenderer>().bounds.size.x/2), transform.position.y - (GetComponent<SpriteRenderer>().bounds.size.y / 2));
        }
    }

    /// <summary>
    /// The tiles start function
    /// </summary>
    public void Start()
    {
        //Sets it to be empty as standard
        IsEmpty = true;

        //Creates a reference to the the spriterenderer
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Sets up the tile
    /// </summary>
    /// <param name="gridPosition">The tile's grid position</param>
    /// <param name="worldPosition">The tile's world position</param>
    /// <param name="parent">The tiles parent</param>
    public void Setup(Point gridPosition, Vector3 worldPosition, Transform parent)
    {
        //Sets the values
        this.GridPosition = gridPosition;
        transform.position = worldPosition;
        transform.SetParent(parent);

        //Adds the tile to the levelmanager
        LevelManager.Instance.Tiles.Add(GridPosition, this);
    }

    /// <summary>
    /// Mouseover, this is executed when the player mouse over the tile
    /// </summary>
    public void OnMouseOver()
    {
        //If the pointer isn't hitting a gameObject, and if we haven't clicked a button
        if (!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickedBtn != null)
        {
            //The tile is empty
            if (IsEmpty)
            {
                ColorTile(emptyColor); //Set the mepty color
            }
            if(!LevelManager.Instance.CheckPath(GridPosition) || GridPosition == LevelManager.Instance.RedSpawn) //Checks if there is still a path avaliable
            {
                ColorTile(fullColor);
            }
            else if (Input.GetMouseButtonDown(0)) //PLaces the tower
            {
                PlaceTower();
            }
        }//If we click a tile
        else if (!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickedBtn == null && Input.GetMouseButtonDown(0))
        {
            if (Input.GetMouseButtonDown(0) && myTower != null) //Selected the tower of there is a tower on the tile
            {
                GameManager.Instance.SelectTower(myTower);
            }
            if (Input.GetMouseButtonDown(0) && myTower == null) //If we click an empty tile deselect the tower
            {
                GameManager.Instance.DeselectTower();
            }
        }
        //if (Input.GetMouseButtonDown(1))//THIS IS MENT FOR TESTING ONLY
        //{
        //    Stack<Node> path = AStar.GetPath(Player.Instance.GridPosition, GridPosition);

        //    Player.Instance.SetPath(path, true);
        //}
    }

    /// <summary>
    /// Places a tower on the tile
    /// </summary>
    public void PlaceTower()
    {
        //Creates the tower
        GameObject tower = (GameObject)Instantiate(GameManager.Instance.ClickedBtn.TowerPrefab, transform.position, Quaternion.identity);

        //Set the sorting layer order on the tower
        tower.GetComponent<SpriteRenderer>().sortingOrder = (int)GridPosition.Y;

        //Sets the tile as transform parent to the tower
        tower.transform.SetParent(transform);

        //Creates the reference to the tower
        this.myTower = tower.transform.GetChild(0).GetComponent<Tower>();

        //Removes the hover icon
        Hover.Instance.Deactivate();

        //Makes sure that it isn't empty
        IsEmpty = false;

        //Sets the color back to white
        ColorTile(Color.white);

        //Sets the price of the tower
        myTower.Price = GameManager.Instance.ClickedBtn.Price;

        //Buys the tower
        GameManager.Instance.BuyTower();

    }

    /// <summary>
    /// When the mouse exits the tile
    /// </summary>
    public void OnMouseExit()
    {
        //Change the color back to white
        ColorTile(Color.white);
    }

    /// <summary>
    /// Sets the color on the tile
    /// </summary>
    /// <param name="newColor"></param>
    public void ColorTile(Color newColor)
    {
        //Sets the color on the tile
        spriteRenderer.color = newColor;
    }
}
