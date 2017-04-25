using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Enum for tower and damage elements
/// </summary>
public enum Element { STORM, FIRE, FROST, POISON, NONE }

/// <summary>
/// Delegate for the currency changed event
/// </summary>
public delegate void CurrencyChanged();

/// <summary>
/// This class handles the game mechanics 
/// </summary>
public class GameManager : Singleton<GameManager>
{
    
    #region EVENTS

    /// <summary>
    /// An event that is triggered when the currency changes
    /// </summary>
    public event CurrencyChanged Changed;

    #endregion

    #region VARIABLES

    /// <summary>
    /// The currenct selected tower
    /// </summary>
    private Tower selectedTower;

    /// <summary>
    /// The player's currency
    /// </summary>
    private int currency;

    /// <summary>
    /// The player's lives
    /// </summary>
    private int lives;

    /// <summary>
    /// The current wave
    /// </summary>
    private int wave = 0;

    /// <summary>
    /// The player's health
    /// </summary>
    private int health = 15;

    /// <summary>
    /// Indicates if we have GameOver
    /// </summary>
    private bool gameOver;

    /// <summary>
    /// A list of all active monsters
    /// </summary>
    private List<Monster> activeMonsters = new List<Monster>();

    /// <summary>
    /// An array of all the tower sprites
    /// </summary>
    [SerializeField]
    private Sprite[] towerSprites;

    /// <summary>
    /// A reference to the pauseMenu
    /// </summary>
    [SerializeField]
    private GameObject pauseMenu;

    /// <summary>
    /// A reference to the upgrade panel
    /// </summary>
    [SerializeField]
    private GameObject upgradePanel;

    /// <summary>
    /// A refernce to the statsPanel
    /// </summary>
    [SerializeField]
    private GameObject statsPanel;

    /// <summary>
    /// A reference to the gameOverMenu
    /// </summary>
    [SerializeField]
    private GameObject gameOverMenu;

    /// <summary>
    /// A reference to the wave button
    /// </summary>
    [SerializeField]
    private GameObject waveBtn;

    /// <summary>
    /// A reference to the upgrade price text
    /// </summary>
    [SerializeField]
    private Text upgradePrice;

    /// <summary>
    /// A reference to the livesText
    /// </summary>
    [SerializeField]
    private Text livesText;

    /// <summary>
    /// A reference tot he waveText
    /// </summary>
    [SerializeField]
    private Text waveText;

    /// <summary>
    /// A reference tot he currency text
    /// </summary>
    [SerializeField]
    private Text currencyText;

    /// <summary>
    /// A reference to the sell text
    /// </summary>
    [SerializeField]
    private Text sellText;

    /// <summary>
    /// A reference to the statsTExt
    /// </summary>
    [SerializeField]
    private Text stats;

    /// <summary>
    /// A reference to the size text
    /// </summary>
    [SerializeField]
    private Text sizeText;

    #endregion

    #region PROPERTIES

    /// <summary>
    /// A property for the object pool
    /// </summary>
    public ObjectPool Pool { get; private set; }

    /// <summary>
    /// a property for the towerBtn
    /// </summary>
    public TowerBtn ClickedBtn { get; set; }

    /// <summary>
    /// Property for accessing the lives
    /// </summary>
    public int Lives
    {
        get
        {
            return lives;
        }
        set
        {

            this.lives = value;
           

            if (lives <= 0)
            {
                lives = 0;
                GameOver();
            }

            livesText.text = lives.ToString();
        }
    }

    /// <summary>
    /// Property for accessing the currency
    /// </summary>
    public int Currency
    {
        get
        {
            return currency;
        }

        set
        {
            this.currency = value;

            this.currencyText.text = value.ToString() + " <color=lime>$</color>";

            OnCurrencyChanged();
        }
    }

    /// <summary>
    /// Proprty for accessing the stats
    /// </summary>
    public Text Stats
    {
        get
        {
            return stats;
        }
    }

    /// <summary>
    /// Property for accessing the SizeText
    /// </summary>
    public Text SizeText
    {
        get
        {
            return sizeText;
        }
    }

    #endregion

    /// <summary>
    /// The game manager's awake function
    /// </summary>
    private void Awake()
    {
        //Instantiates the object pool
        Pool = GetComponent<ObjectPool>();
    }

    /// <summary>
    /// The gameManager's start function
    /// </summary>
    private void Start()
    {
        //Sets the lives and currency
        Lives = 10;
        Currency = 4;

    }

    /// <summary>
    /// The GameManager's update
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))//If we press the escape button
        {
            //If we haven't selected a tower, then we need to show the ingame menu
            if (selectedTower == null && !Hover.Instance.Visible)
            {
                //Shows the menu
                MenuManager.Instance.ShowIngameMenu();
            }
            else if (Hover.Instance.Visible)//If we are holding a tower in our hand, then we drop it
            {
                //Drops the tower
                DropTower();
            }
            else if (selectedTower != null) //If we have selected a tower, then we deselect it
            {
                //Deselect the tower
                DeselectTower();
            }
        }
    }

    /// <summary>
    /// When the currency changes
    /// </summary>
    public void OnCurrencyChanged()
    {
        if (Changed != null)
        {
            //Executes the event if the event if someone is listening to the event
            Changed(); 
        }
    }

    /// <summary>
    /// Pick a tower then a buy button is pressed
    /// </summary>
    /// <param name="towerBtn">The clicked button</param>
    public void PickTower(TowerBtn towerBtn)
    {
        //If we have enough currency and we aren't in a wave phase
        if (Currency >= towerBtn.Price && activeMonsters.Count <= 0)
        {
            //Stores the clicked button
            ClickedBtn = towerBtn; 

            //Activates the hover icon
            Hover.Instance.Activate(ClickedBtn.TowerSprite);
        }
    }

    /// <summary>
    /// Drops the tower fromt he player's hand
    /// </summary>
    private void DropTower()
    {
        //Sets the clicked button to null
        ClickedBtn = null;

        //Deactivates the hover icon 
        Hover.Instance.Deactivate();
    }

    /// <summary>
    /// Buys a tower
    /// </summary>
    public void BuyTower()
    {
        //If we have enough currency to buy the tower
        if (Currency >= ClickedBtn.Price)
        {
            //Reduce the currency
            Currency -= ClickedBtn.Price;

            //Deselects the button
            ClickedBtn = null;
        }
    }

    /// <summary>
    /// Selects a tower by clicking it
    /// </summary>
    /// <param name="tower">The clicked tower</param>
    public void SelectTower(Tower tower)
    {
        if (selectedTower != null)//If we have selected a tower
        {
            //Selects the tower
            selectedTower.Select();
        }

        //Sets the selected tower
        selectedTower = tower; 

        //Writes the sell price on the tower
        sellText.text = "+ " + (selectedTower.Price / 2).ToString() + " $";

        //Shows the upgradepanel
        upgradePanel.SetActive(true);

        //If the tower has another upgrade
        if (tower.NextUpgrade != null)
        {
            //Shows the upgraded values
            upgradePrice.text = tower.NextUpgrade.Price.ToString() + "$";
        }
      
        //Selects the tower
        selectedTower.Select();
    }

    /// <summary>
    /// Updates the tooltip
    /// </summary>
    public void UpdateTooltip()
    {
        //If we have selected a tower
        if (selectedTower != null)
        {
            //Updates the tooltip text

            sellText.text = "+ " + (selectedTower.Price / 2).ToString() + " $";
            SetTooltipText(selectedTower.GetStats());

            if (selectedTower.NextUpgrade != null)//If  we have a upgrade, then we need to show the upgrade
            {
                upgradePrice.text = selectedTower.NextUpgrade.Price.ToString() + "$";
            }
            else
            {
                upgradePrice.text = string.Empty;
            }
           
        }
    }

    /// <summary>
    /// Sets the text on the tooltip
    /// </summary>
    /// <param name="txt"></param>
    public void SetTooltipText(string txt)
    {
        stats.text = txt; //Sets the stats text
        sizeText.text = txt; //sets the size text

    }

    /// <summary>
    /// Deselect the tower
    /// </summary>
    public void DeselectTower()
    {
        //If we have a selected tower
        if (selectedTower != null)
        {
            //Calls select to deselect it
            selectedTower.Select();
        }

        //Hide the upgrade panel
        upgradePanel.SetActive(false);

        //Remove the reference to the tower
        selectedTower = null;
    }

    /// <summary>
    /// Selss a tower
    /// </summary>
    public void SellTower()
    {
        //If we have a selected tower
        if (selectedTower != null)
        {
            //Selss the tower an refunds half of the price
            Currency += selectedTower.Price / 2;

            //Gets the tile of the tower and makes it walkable again
            selectedTower.GetComponentInParent<TileScript>().IsEmpty = true;

            //Removes the tower
            Destroy(selectedTower.transform.parent.gameObject);

            //Deselcts the tower
            DeselectTower();

        }
    }

    /// <summary>
    /// Upgrades the tower
    /// </summary>
    public void UpgradeTower()
    {
        //If we have selected a tower
        if (selectedTower != null)
        {
            //Upgrades the tower
            if (selectedTower.Level <= selectedTower.Upgrades.Length && Currency >= selectedTower.NextUpgrade.Price)
            {
                selectedTower.Upgrade();
            }
               
        }
    }

    /// <summary>
    /// Shows the towerstats
    /// </summary>
    public void ShowStats()
    {
        //Shows or hides the stats panel
        statsPanel.SetActive(!statsPanel.activeSelf);
    }

    /// <summary>
    /// Starts a wave
    /// </summary>
    public void StartWave()
    {
        //Increases the wave count
        wave++;

        //Updates the wave text, so that the player can see it
        waveText.text = string.Format("Invasion: <color=lime>{0}</color>",wave);

        //Starts to spawn the wave
        StartCoroutine(SpawnWave());

        //Hides the wavebutton
        waveBtn.SetActive(false);
    }

    /// <summary>
    /// Spawns a wave of monsters
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnWave()
    {
        //Generates the path
        LevelManager.Instance.GeneratePath();

        //Spawns an amount of monsters based on the wave
        for (int i = 0; i < wave; i++)
        {
            //Picks a random monster
            int monsterIndex = UnityEngine.Random.Range(0, 4);

            string type = string.Empty;

            switch (monsterIndex)
            {
                case 0:
                    type = "BlueMonster";
                    break;
                case 1:
                    type = "GreenMonster";
                    break;
                case 2:
                    type = "PurpleMonster";
                    break;
                case 3:
                    type = "RedMonster";
                    break;
            }

            //Requests the monster form the pool
            Monster monster = Pool.GetObject(type).GetComponent<Monster>();

            //Spawns a monster 
            monster.Spawn(health);

            //Every third wave we need to increase the health
            if (wave % 3 == 0)
            {
                health += 5;
            }

            //Adds the monster to the activemonster list
            activeMonsters.Add(monster);

            //waits 2.5 seconds before we spawn the next monster
            yield return new WaitForSeconds(2.5f);
        }
    }

    /// <summary>
    /// GameOver
    /// </summary>
    public void GameOver()
    {
        //If we don't have game over then we need to make game over
        if (!gameOver)
        {
            gameOver = true;

            //Shows the game over menu
            gameOverMenu.SetActive(true);
        }
        
    }

    /// <summary>
    /// Restarts the game
    /// </summary>
    public void Restart()
    {
        //Sets the timescale to 1 so that the objects will move again
        Time.timeScale = 1;

        //Reloads the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Removes a monster from the game
    /// </summary>
    /// <param name="monster">Monster to remove</param>
    public void RemoveMonster(Monster monster)
    {
        //Removes the monster from the active list
        activeMonsters.Remove(monster);

        //IF we don't have more active monsters and the game isn't over, then we need to show the wave button
        if (activeMonsters.Count <= 0 && !gameOver)
        {
            //Shows the wave button
            waveBtn.SetActive(true);
        }
    }
}