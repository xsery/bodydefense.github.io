using System;

/// <summary>
/// The storm tower script
/// </summary>
class StormTower : Tower
{
    private void Start()
    {
        //Sets the element type
        ElementType = Element.STORM;

        //Creates the upgrades
        Upgrades = new TowerUpgrade[]
        {
            new TowerUpgrade(2,2,1,2),
            new TowerUpgrade(5,3,1,2),
        };
    }

    /// <summary>
    /// Gets a debuff
    /// </summary>
    /// <returns>A storm debuff</returns>
    public override Debuff GetDebuff()
    {
        return new StormDebuff(Target, DebuffDuration);
    }

    /// <summary>
    /// Returns the towers current stats and upgraded stats
    /// </summary>
    /// <returns>Tool tip</returns>
    public override string GetStats()
    {
        return String.Format("<color=#add8e6ff>{0}</color>{1}", "<size=20><b>Storm</b></size>", base.GetStats());
    }
}
