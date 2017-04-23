using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script is attached to all the buttons in the right side on the screen
/// These are the buttons, that we use when we buy towers
/// </summary>
public class TowerBtn : MonoBehaviour
{
    /// <summary>
    /// The prefab that this button will spawn
    /// </summary>
    [SerializeField]
    private GameObject towerPrefab;

    /// <summary>
    /// The price of the tower
    /// </summary>
    [SerializeField]
    private int price;

    /// <summary>
    /// A reference to the text price
    /// </summary>
    [SerializeField]
    private Text txtPrice;

    /// <summary>
    /// A reference to the tower's sprite
    /// </summary>
    [SerializeField]
    private Sprite towerSprite;

    /// <summary>
    /// Property for accessing the button's prefab
    /// </summary>
    public GameObject TowerPrefab
    {
        get
        {
            return towerPrefab;
        }
    }

    /// <summary>
    /// Property for accessing the button's price
    /// </summary>
    public int Price
    {
        get
        {
            return price;
        }
    }

    /// <summary>
    /// Property for accessing the text on screen
    /// </summary>
    public Text TxtPrice
    {
        get
        {
            return txtPrice;
        }
    }

    /// <summary>
    /// A reference for accessing the tower sprite
    /// </summary>
    public Sprite TowerSprite
    {
        get
        {
            return towerSprite;
        }
    }

    // Use this for initialization
    private void Start()
    {
        //Writes the price on the button
        TxtPrice.text = price+ "$";

        //Assigns the PriceCheck function to the Changed event on the GameManager
        GameManager.Instance.Changed += new CurrencyChanged(PriceCheck);

        PriceCheck();
    }

    /// <summary>
    /// Checks if we have enough money use this button to buy a tower
    /// </summary>
    private void PriceCheck()
    {
        if (price <= GameManager.Instance.Currency) //If we have enouogh money
        {
            //Set the color to white to make it visible
            GetComponent<Image>().color = Color.white;
            TxtPrice.color = Color.white;
        }
        else //If we don't have enough money
        {
            //Gray out the button
            GetComponent<Image>().color = Color.gray;
            TxtPrice.color = Color.gray;
        }
    }

    /// <summary>
    /// Show infor about the tower when hovering the button
    /// </summary>
    /// <param name="type"></param>
    public void ShowInfo(string type)
    {
        string tooltip = string.Empty;
        switch (type) //Uses the towerprefab, to get the stats for the tooltip on each tower
        {
            case "Fire":
                FireTower fire = towerPrefab.GetComponentInChildren<FireTower>();
                tooltip = string.Format("<color=#ffa500ff><size=20><b>Fire</b></size></color>\nDamage: {0} \nProc: {1}%\nDebuff duration: {2}sec \nTick time: {3} sec \nTick damage: {4}\nCan apply a DOT to the target", fire.Damage,fire.Proc,fire.DebuffDuration,fire.TickTime,fire.TickDamage);
                break;
            case "Frost":
                FrostTower frost = towerPrefab.GetComponentInChildren<FrostTower>();
                tooltip = string.Format("<color=#00ffffff><size=20><b>Frost</b></size></color>\nDamage: {0} \nProc: {1}%\nDebuff duration: {2}sec\nSlowing factor: {3}%\nHas a chance to slow down the target", frost.Damage, frost.Proc, frost.DebuffDuration, frost.SlowingFactor);
                break;
            case "Poison":
                PoisonTower poison = towerPrefab.GetComponentInChildren<PoisonTower>();
                tooltip = string.Format("<color=#00ff00ff><size=20><b>Poison</b></size></color>\nDamage: {0} \nProc: {1}%\nDebuff duration: {2}sec \nTick time: {3} sec \nSplash damage: {4}\nCan apply dripping poison", poison.Damage, poison.Proc, poison.DebuffDuration, poison.TickTime, poison.SplashDamage);
                break;
            case "Storm":
                StormTower storm = towerPrefab.GetComponentInChildren<StormTower>();
                tooltip = string.Format("<color=#add8e6ff><size=20><b>Storm</b></size></color>\nDamage: {0} \nProc: {1}%\nDebuff duration: {2}sec\n Has a chance to stunn the target", storm.Damage, storm.Proc, storm.DebuffDuration);
                break;
 
        }

        //Shows the tooltip and shows the stats
        GameManager.Instance.SetTooltipText(tooltip);
        GameManager.Instance.ShowStats();
    }
}
