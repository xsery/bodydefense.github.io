using System;
using UnityEngine;

public class FrostTower : Tower
{
    /// <summary>
    /// The towers slowing factor stat
    /// </summary>
    [SerializeField]
    private float slowingFactor;

    /// <summary>
    /// Property for getting the slowing factor
    /// </summary>
    public float SlowingFactor
    {
        get
        {
            return slowingFactor;
        }
    }

    private void Start()
    {
        //Sets the tower elements
        ElementType = Element.FROST;

        //Sets the upgrades for the tower
        Upgrades = new TowerUpgrade[]
            {
                new TowerUpgrade(2,1,1,2,10),
                new TowerUpgrade(2,1,1,2,20),
            };
    }

    /// <summary>
    /// Gets the tower's debuff
    /// </summary>
    /// <returns>A frost debuff</returns>
    public override Debuff GetDebuff()
    {
        return new FrostDebuff(SlowingFactor, DebuffDuration, Target);
    }

    /// <summary>
    /// Upgrades the tower
    /// </summary>
    public override void Upgrade()
    {
        //Upgrades the tower
        this.slowingFactor += NextUpgrade.SlowingFactor;
        base.Upgrade();
    }

    /// <summary>
    /// Returns the towers current stats and upgraded stats
    /// </summary>
    /// <returns></returns>
    public override string GetStats()
    {
        if (NextUpgrade != null)  //If the next is avaliable
        {
            return String.Format("<color=#00ffffff>{0}</color>{1} \nSlowing factor: {2}% <color=#00ff00ff>+{3}</color>", "<size=20><b>Frost</b></size>", base.GetStats(), SlowingFactor, NextUpgrade.SlowingFactor);
        }

        //Returns the current upgrade
        return String.Format("<color=#00ffffff>{0}</color>{1} \nSlowing factor: {2}%", "<size=20><b>Frost</b></size>", base.GetStats(), SlowingFactor);
    }
}
