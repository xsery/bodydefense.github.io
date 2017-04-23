public class TowerUpgrade
{
    /// <summary>
    /// The tower's price
    /// </summary>
    public int Price { get; private set; }

    /// <summary>
    /// The tower's damage
    /// </summary>
    public int Damage { get; private set; }

    /// <summary>
    /// The tower's debuff duration
    /// </summary>
    public float DebuffDuration { get; private set; }

    /// <summary>
    /// The proc chance of the debuff
    /// </summary>
    public float ProcChance { get; private set; }

    /// <summary>
    /// The ice tower's slowing factor
    /// </summary>
    public float SlowingFactor { get; private set; }

    /// <summary>
    /// How often will the debuff tick
    /// </summary>
    public float TickTime { get; private set; }

    /// <summary>
    /// Special damage
    /// </summary>
    public int SpecialDamage { get; private set; }

    /// <summary>
    /// Constructor used by the storm tower
    /// </summary>
    /// <param name="price">The Price of the upgrade/param>
    /// <param name="damage">How much to increase the tower damage with</param>
    /// <param name="debuffduration">How much to increase the debuff duration with</param>
    /// <param name="procChance">How much to increase the procchance with</param>
    public TowerUpgrade(int price, int damage,  float debuffduration, float procChance)
    {
        this.Damage = damage;
        this.Price = price;
        this.DebuffDuration = debuffduration;
        this.ProcChance = procChance;
    }

    /// <summary>
    /// Constructor used by the Frost tower
    /// </summary>
    /// <param name="price">The Price of the upgrade/param>
    /// <param name="damage">How much to increase the tower damage with</param>
    /// <param name="debuffduration">How much to increase the debuff duration with</param>
    /// <param name="procChance">How much to increase the procchance with</param>
    /// <param name="SlowingFactor">How much to increase the slowing factor with</param>
    public TowerUpgrade(int price,int damage, float debuffduration, float procChance, float SlowingFactor)
    {
        this.Damage = damage;
        this.DebuffDuration = debuffduration;
        this.ProcChance = procChance;
        this.SlowingFactor = SlowingFactor;
        this.Price = price;
    }


    /// <summary>
    /// Constructor used by the Fire and Poison towers
    /// </summary>
    /// <param name="price">The Price of the upgrade/param>
    /// <param name="damage">How much to increase the tower damage with</param>
    /// <param name="debuffduration">How much to increase the debuff duration with</param>
    /// <param name="procChance">How much to increase the procchance with</param>
    /// <param name="tickTime">Reduction of the tick time</param>
    /// <param name="specialDamage">How much to increase the special damage with</param>
    public TowerUpgrade(int price,int damage, float debuffduration, float procChance, float tickTime, int specialDamage)
    {
        this.Damage = damage;
        this.DebuffDuration = debuffduration;
        this.ProcChance = procChance;
        this.TickTime = tickTime;
        this.SpecialDamage = specialDamage;
        this.Price = price;
    }
}
