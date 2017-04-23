using System;
using UnityEngine;

class PoisonTower : Tower
{
    /// <summary>
    /// The tick time for the debuff
    /// </summary>
    [SerializeField]
    private float tickTime;

    /// <summary>
    /// Prefab that we need for spawning the splash
    /// </summary>
    [SerializeField]
    private PoisonSplash splashPrefab;

    /// <summary>
    /// The splash damage
    /// </summary>
    [SerializeField]
    private int splashDamage;

    /// <summary>
    /// Property for accessing the tickTime
    /// </summary>
    public float TickTime
    {
        get
        {
            return tickTime;
        }
    }

    /// <summary>
    /// Property for accessing the splashDamage
    /// </summary>
    public int SplashDamage
    {
        get
        {
            return splashDamage;
        }
    }

    private void Start()
    {
        //Sets the element time 
        ElementType = Element.POISON;

        //Sets up the upgrades
        Upgrades = new TowerUpgrade[]
             {
                 new TowerUpgrade(2,1,.5f,5,-0.1f,1),
                    new TowerUpgrade(5,1,.5f,5,-0.1f,1),
             };

    }

    /// <summary>
    /// Gets the tower's debuff
    /// </summary>
    /// <returns>A fire debuff</returns>
    public override Debuff GetDebuff()
    {
        return new PoisonDebuff(SplashDamage, TickTime, splashPrefab, Target, DebuffDuration);
    }

    /// <summary>
    /// Upgrades the tower
    /// </summary>
    public override void Upgrade()
    {
        //Upgrades the tower
        this.splashDamage += NextUpgrade.SpecialDamage;
        this.tickTime += NextUpgrade.TickTime;
        base.Upgrade();

    }

    /// <summary>
    /// Returns the towers current stats and upgraded stats
    /// </summary>
    /// <returns>Tool tip</returns>
    public override string GetStats()
    {
        if (NextUpgrade != null)
        {
            return string.Format("<color=#00ff00ff>{0}</color>{1} \nTick time: {2} <color=#00ff00ff>{4}</color>\nSplash damage: {3} <color=#00ff00ff>+{5}</color>", "<size=20><b>Poison</b></size>", base.GetStats(), TickTime, SplashDamage, NextUpgrade.TickTime, NextUpgrade.SpecialDamage);
        }

        return string.Format("<color=#00ff00ff>{0}</color>{1} \nTick time: {2}\nSplash damage: {3}", "<size=20><b>Poison</b></size>", base.GetStats(), TickTime, SplashDamage);

    }

}