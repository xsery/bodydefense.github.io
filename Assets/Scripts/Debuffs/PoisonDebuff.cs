using UnityEngine;
/// <summary>
/// The sub class for poison debuffs
/// </summary>
[System.Serializable]
class PoisonDebuff : Debuff
{
    /// <summary>
    /// How often the debuff will tick in seconds
    /// </summary>
    private float tickTime;

    /// <summary>
    /// Time since last tick, this is used for detrmine if the debuff should tick again
    /// </summary>
    private float timeSinceTick;

    /// <summary>
    /// Prefab for the splash on the ground
    /// </summary>
    private PoisonSplash splashPrefab;

    /// <summary>
    /// Damage that the splash debuff will make
    /// </summary>
    private int splashDamage;

    /// <summary>
    /// The poison debuff's constructor
    /// </summary>
    /// <param name="splashDamage">The damage that the splash will dow</param>
    /// <param name="tickTime">how often the debuff will tick</param>
    /// <param name="splashPrefab">Prefab for the splash</param>
    /// <param name="target">The target that we will apply de thebuff til</param>
    /// <param name="duration">The duration of the debuff</param>
    public PoisonDebuff(int splashDamage, float tickTime, PoisonSplash splashPrefab, Monster target, float duration) : base(duration, target)
    {
        this.splashDamage = splashDamage;
        this.tickTime = tickTime;
        this.splashPrefab = splashPrefab;
    }

    /// <summary>
    /// Update's the debuff
    /// </summary>
    public override void Update()
    {
        if (target != null) //If  we have a target
        {
            //Increase the time since last tick
            timeSinceTick += Time.deltaTime;

            //If the time since tick is larger than the time since tick then we need to tick
            if (timeSinceTick >= tickTime)
            {
                timeSinceTick = 0;
                Splash();
            }

            // calls base update
            base.Update();
        }

    }

    /// <summary>
    /// Spawns a splash on the ground
    /// </summary>
    private void Splash()
    {
        //Spawns the splash at the monster's position
        PoisonSplash tmp = GameObject.Instantiate(splashPrefab, target.transform.position, Quaternion.identity) as PoisonSplash;

        //Sets the damage equal to the splash damage
        tmp.Damage = splashDamage;

        //Makes sure that the monster that spawned the splash can't collide with it
        Physics2D.IgnoreCollision(target.GetComponent<Collider2D>(), tmp.GetComponent<Collider2D>());
    }
}
