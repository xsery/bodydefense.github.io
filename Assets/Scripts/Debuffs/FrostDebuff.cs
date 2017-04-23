/// <summary>
/// This is a debuff subclass for frost debuffs
/// </summary>
class FrostDebuff : Debuff
{
    /// <summary>
    /// The factor that the monster's speed will be reduced with
    /// </summary>
    private float slowingFactor;

    /// <summary>
    /// Indicates if the debuff is applied
    /// </summary>
    private bool applied;

    /// <summary>
    /// The frost debuff's constructor
    /// </summary>
    /// <param name="slowingFactor">Reduction of the monsters movement speed</param>
    /// <param name="duration">The duration of the debuffs</param>
    /// <param name="target">The target that this debuff will be applied to</param>
    public FrostDebuff(float slowingFactor, float duration, Monster target) : base(duration,target)
    {
        //sets the slowing factor
        this.slowingFactor = slowingFactor;
    }

    /// <summary>
    /// Updates the frsot debuff
    /// </summary>
    public override void Update()
    {
        if (target != null) //If we have a target
        {
            if (!applied) //If the deubfgf isn't applied then apply it
            {
                applied = true; //Sets appiled to true

                //Reduces the speed
                target.Speed -= (target.MaxSpeed*slowingFactor)/100;
            }

            //Calss the base class's update
            base.Update();
        }

    }

    /// <summary>
    /// removes the debuff from the target
    /// </summary>
    public override void Remove()
    {
        if (target != null)
        {
            target.Speed = target.MaxSpeed;

            base.Remove();
        }

    }
}