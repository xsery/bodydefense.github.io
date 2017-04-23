using UnityEngine;

/// <summary>
/// This is a debuff subclass for fire debuffs
/// </summary>
public class FireDebuff : Debuff
{
    /// <summary>
    /// The time it takes for the debuff to tick
    /// </summary>
    private float tickTime;
    
    /// <summary>
    /// The damage that the debuff gies
    /// </summary>
    private float tickDamage;

    /// <summary>
    /// Time since last tick, this is used for detrmine if the debuff should tick again
    /// </summary>
    private float timeSinceTick;

    /// <summary>
    /// The Firedebuffs constructor
    /// </summary>
    /// <param name="tickDamage">The damage of the tick</param>
    /// <param name="tickTime">The time between ticks in seconds</param>
    /// <param name="target">The target the debuff will be applied to</param>
    /// <param name="duration">The debuffs duration</param>
    public FireDebuff( float tickDamage ,float tickTime, Monster target, float duration) : base(duration,target)
    {
        //Sets the values
        this.tickDamage = tickDamage;
        this.tickTime = tickTime;
    }

    /// <summary>
    /// The firedebuff's update function
    /// </summary>
    public override void Update()
    {
        if (target != null) //If we have a target
        {
            //Update time since last tick
            timeSinceTick += Time.deltaTime;

            //If the timesinceTick if larget than tick time, then we need to do a tick
            if (timeSinceTick >= tickTime)
            {
                //Resets the values
                timeSinceTick = 0;

                //Dodes a tick
                target.TakeDamage(tickDamage, Element.FIRE);
            }

            //Calls update on the debuff superclass
            base.Update();
        }

    }
}
