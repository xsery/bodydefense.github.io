using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

/// <summary>
/// This class handles the level generation
/// </summary>
public class LevelManager : Singleton<LevelManager>
{
    /// <summary>
    /// A reference to the play button
    /// </summary>
    [SerializeField]
    private GameObject playBtn;

    /// <summary>
    /// A reference to the loading screen
    /// </summary>
    [SerializeField]
    private Canvas loadScreen;

    /// <summary>
    /// An array of tile prefabs
    /// </summary>
    [SerializeField]
    private GameObject[] tilePrefabs;

    ///// <summary>
    ///// A prefab to the player THIS IS ONLY FOR TESTING
    ///// </summary>
    //[SerializeField]
    //private GameObject playerPrefab;

    /// <summary>
    /// A prefab for the blue portal
    /// </summary>
    [SerializeField]
    private GameObject bluePortalPrefab;

    /// <summary>
    /// A prefab for the red portal
    /// </summary>
    [SerializeField]
    private GameObject redPortalPrefab;

    /// <summary>
    /// The maps transform, this is needed for adding new tiles
    /// </summary>
    [SerializeField]
    private Transform map;

    /// <summary>
    /// A reference to the cameramovement script
    /// </summary>
    [SerializeField]
    private CameraMovement cameraMovement;

    /// <summary>
    /// Stat used for the loading bar
    /// </summary>
    [SerializeField]
    private Stat loadingStat;

    /// <summary>
    /// A bool that indicates if we are loading or not
    /// </summary>
    public bool Loading { get; private set; }

    /// <summary>
    /// Property for accessing the blue portal
    /// </summary>
    public Portal BluePortal { get; private set; }

    /// <summary>
    /// The size of the map
    /// </summary>
    private Point mapSize;

    /// <summary>
    /// Location of the red portal
    /// </summary>
    public Point RedSpawn { get; private set; }

    /// <summary>
    /// Location of the blue portal
    /// </summary>
    private Point blueSpawn;

    /// <summary>
    /// The full path from start to goal
    /// </summary>
    private Stack<Node> fullPath;

    /// <summary>
    /// a dictionary of all tiles in the game
    /// </summary>
    public Dictionary<Point, TileScript> Tiles { get; private set; }

    /// <summary>
    /// Proprty for getting the size of a tile
    /// </summary>
    public float TileSize
    {
        get { return tilePrefabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x * tilePrefabs[0].GetComponent<Transform>().localScale.x; }
    }

    /// <summary>
    /// A property for getting the world start position
    /// </summary>
    private Vector3 WorldStartPos
    {
        get
        {
            return Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));
        }
    }

    /// <summary>
    /// Property for accessing the path
    /// </summary>
    public Stack<Node> Path
    {
        get
        {
            if (fullPath == null)
            {
                GeneratePath();
            }

            return new Stack<Node>(new Stack<Node>(fullPath));
        }
    }

    /// <summary>
    /// The levelmanager's start function
    /// </summary>
    void Start()
    {
        //Initializes the loading bar
        loadingStat.Initialize();

        //Creates the level
        StartCoroutine(CreateLevel());
    }

    /// <summary>
    /// CoRoutine for creating the level
    /// </summary>
    /// <returns></returns>
    private IEnumerator CreateLevel()
    {
        //An array the contains the map data
        string[] mapData;

        //Indicates if we are loading
        Loading = true;

        //Instantiates the tiles dictionary
        Tiles = new Dictionary<Point, TileScript>();

        //Generates the map on webGL
#if UNITY_WEBGL
        mapData = new string[]
          {
                "441444144441111111","141414141141111111","141414141141111111","141414141141111111","141414141141111111","141414141141111111","144414441144111111","111111111111111111"
        };
#else
        //Generates the map on other platforms
        mapData = ReadlLevelText();
#endif

        //Sets the map size based on the map data
        mapSize = new Point(mapData[0].ToCharArray().Length, mapData.Length);

        //Get the max amount of tiles by taking width * height
        float maxTiles = mapSize.X * mapSize.Y;

        //Sets the loading stat as the max amount of tiles
        loadingStat.MaxVal = maxTiles;

        //Amount of tiles placed
        float actualTiles = 0;

        //Places the tiles on the world
        for (int y = 0; y < mapSize.Y; y++)
        {
            char[] horizontalTiles = mapData[y].ToCharArray();

            for (int x = 0; x < mapSize.X; x++)
            {
                PlaceTile(horizontalTiles[x].ToString(), x, y);
                actualTiles++;

                //Updates the loading bar
                loadingStat.CurrentValue = actualTiles;

                yield return null;

            }
        }
        //Gets the  max position of the tile
        Vector3 maxtilePosition = Tiles[new Point(mapSize.X - 1, mapSize.Y - 1)].transform.position;

        //Sets the camera limits to the max tile position
        cameraMovement.SetLimits(new Vector3(maxtilePosition.x + TileSize, maxtilePosition.y - TileSize));

        //Spawns the portals
        SpawnPortals();

        //Sets the playbutton active when we are done loading
        playBtn.SetActive(true);
    }

    //private void SpawnPlayer()
    //{
    //    Point playerStart = mapSize / 2;

    // //   GameObject player = (GameObject)Instantiate(playerPrefab, Tiles[playerStart].GetComponent<TileScript>().WorldPosition, Quaternion.identity);

    //    player.GetComponent<Player>().GridPosition = playerStart;
    //}


    /// <summary>
    /// Spawns the portals
    /// </summary>
    private void SpawnPortals()
    {
        //Spawns the blue portal at position 0,0
        blueSpawn = new Point(0, 0);
        GameObject bluePortal = (GameObject)Instantiate(bluePortalPrefab, Tiles[blueSpawn].GetComponent<TileScript>().WorldPosition, Quaternion.identity);
        bluePortal.name = "BluePortal";
        this.BluePortal = bluePortal.GetComponent<Portal>();

        //Spawns the red portal
        RedSpawn = new Point(11, 6);
        GameObject redPortal = (GameObject)Instantiate(redPortalPrefab, Tiles[RedSpawn].GetComponent<TileScript>().WorldPosition, Quaternion.identity);
        redPortal.name = "RedPortal";
    }

    /// <summary>
    /// Checks if a position if inbounds of the map
    /// </summary>
    /// <param name="position">Position to check</param>
    /// <returns></returns>
    public bool InBounds(Point position)
    {
        return position.X >= 0 && position.Y >= 0 && position.X < mapSize.X && position.Y < mapSize.Y;
    }

    /// <summary>
    /// Reads the level text
    /// </summary>
    /// <returns>A string array with indicators of the tiles to place</returns>
    private string[] ReadlLevelText()
    {
        //Loads the text asset from the resources folder
        TextAsset bindata = Resources.Load("Level") as TextAsset;

        //Get the string
        string data = bindata.text.Replace(Environment.NewLine, string.Empty);

        //Splits the string into an array
        return data.Split('-');
    }

    /// <summary>
    /// Places a tile in the world
    /// </summary>
    /// <param name="tileType">The type of tile to spawn</param>
    /// <param name="x">X position</param>
    /// <param name="y">Y position</param>
    private void PlaceTile(string tileType, int x, int y)
    {
        //The index of til to spawn
        int tileIndex = int.Parse(tileType);

        //Creates a tile
        TileScript tile = Instantiate(tilePrefabs[tileIndex]).GetComponent<TileScript>();

        //Sets up the tile in our world
        tile.Setup(new Point(x, y), new Vector3(WorldStartPos.x + (TileSize * x), WorldStartPos.y - (TileSize * y), 0), map);
    }

    /// <summary>
    /// Generates a path with the AStar algorithm
    /// </summary>
    public void GeneratePath()
    {
        //Generates a path from start to finish and stores it in fullPath
        fullPath = AStar.GetPath(blueSpawn, RedSpawn);
    }

    /// <summary>
    /// Starts the level
    /// </summary>
    public void StartLevel()
    {
        //Removes the loading screen
        loadScreen.enabled = false;

        //Indicates that we aren't loading
        Loading = false;
    }

    /// <summary>
    /// Checks if we can move to the slected tile
    /// </summary>
    /// <param name="checkPoint">The point to check</param>
    /// <returns></returns>
    public bool CheckPath(Point checkPoint)
    {
        //A temp reference to the clicked tile
        TileScript tmp = Tiles[checkPoint];

        if (tmp.IsEmpty) //If the tile is empty then we can move there
        {
            tmp.IsEmpty = false;

            if (AStar.GetPath(RedSpawn, blueSpawn) == null) //If there isn't an available path between the tiles
            {
                tmp.IsEmpty = true; 
                return false;
            }
            else //If there is an available path
            {
                tmp.IsEmpty = true;
                return true;
            }

        }
        else //If the tile isn't empty
        {
            return false;
        }

    }
}
