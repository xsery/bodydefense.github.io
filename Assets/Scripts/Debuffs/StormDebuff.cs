using System;

/// <summary>
/// Sub class for the storm debuff
/// </summary>
[Serializable]
public class StormDebuff : Debuff
{
    //The reference to the monsters original speed
    private float speed;

    /// <summary>
    /// The storm debuff's constructor
    /// </summary>
    /// <param name="target">The target to apply the debuff to</param>
    /// <param name="duration">The debuff's duration</param>
    public StormDebuff(Monster target, float duration) : base(duration,target)
    {
        if (target != null) //If we have a target, then we set the speed to 0, so that it is stunned
        {
            this.speed = target.Speed;
            target.Speed = 0;
        }
     
    }

    /// <summary>
    /// Removes the debuff from the target
    /// </summary>
    public override void Remove()
    {
        if (target != null)
        {
            //resets the speed
            target.Speed = target.MaxSpeed;
            base.Remove();
        }

    }
}
