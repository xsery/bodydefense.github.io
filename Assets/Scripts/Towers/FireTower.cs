using UnityEngine;

/// <summary>
/// The script for the firetower
/// </summary>
public class FireTower : Tower
{
    /// <summary>
    /// The tick time for the debuff
    /// </summary>
    [SerializeField]
    private float tickTime;

    /// <summary>
    /// The tick damage
    /// </summary>
    [SerializeField]
    private float tickDamage;

    /// <summary>
    /// Property for accessing the tickDamage
    /// </summary>
    public float TickDamage
    {
        get
        {
            return tickDamage;
        }
    }

    /// <summary>
    /// Property for accessing the ticktime
    /// </summary>
    public float TickTime
    {
        get
        {
            return tickTime;
        }
    }

    private void Start()
    {
        //Sets the elementtype to fire
        ElementType = Element.FIRE;

        //Sets up the upgrades
        Upgrades = new TowerUpgrade[]
            {
                new TowerUpgrade(2,2,.5f,5,-0.1f,1),
                new TowerUpgrade(5,3,.5f,5,-0.1f,1),
            };


    }

    /// <summary>
    /// Gets the tower's debuff
    /// </summary>
    /// <returns>A fire debuff</returns>
    public override Debuff GetDebuff()
    {
        return new FireDebuff(TickDamage, TickTime, Target, DebuffDuration);
    }

    /// <summary>
    /// Upgrades the tower
    /// </summary>
    public override void Upgrade()
    {
        //Upgrades the tower
        this.tickTime += NextUpgrade.TickTime;
        this.tickDamage += NextUpgrade.SpecialDamage;

        base.Upgrade();

    }

    /// <summary>
    /// Returns the towers current stats and upgraded stats
    /// </summary>
    /// <returns>Tool tip</returns>
    public override string GetStats()
    {
        if (NextUpgrade != null) //If the next is avaliable
        {
            return string.Format("<color=#ffa500ff>{0}</color>{1} \nTick time: {2} <color=#00ff00ff>{4}</color>\nTick damage: {3} <color=#00ff00ff>+{5}</color>", "<size=20><b>Fire</b></size> ", base.GetStats(), TickTime, TickDamage, NextUpgrade.TickTime, NextUpgrade.SpecialDamage);
        }

        //Returns the current upgrade
        return string.Format("<color=#ffa500ff>{0}</color>{1} \nTick time: {2}\nTick damage: {3}", "<size=20><b>Fire</b></size> ", base.GetStats(), TickTime, TickDamage);
    }

}
