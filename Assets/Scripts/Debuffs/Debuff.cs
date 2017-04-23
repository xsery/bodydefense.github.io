using UnityEngine;

/// <summary>
/// This is the super class for all debuffs
/// </summary>
public abstract class Debuff
{
    /// <summary>
    /// The duration of the debuff
    /// </summary>
    protected float duration;

    /// <summary>
    /// Time elapsed since the debuff was applied
    /// </summary>
    protected float elapsed;

    /// <summary>
    /// The target of the debuff
    /// </summary>
    protected Monster target;

    /// <summary>
    /// A constructor of the debuff, this is called when the 
    /// </summary>
    /// <param name="duration">The duratio of the debuff</param>
    /// <param name="target">The target monster</param>
    public Debuff(float duration, Monster target)
    {
        //Sets the values
        this.target = target;
        this.duration = duration;
    }

    /// <summary>
    /// Updates the debuff
    /// </summary>
    public virtual void Update()
    {
        //Checks if the debuff needs to be removed from the target
        elapsed += Time.deltaTime;

        if (elapsed >= duration)
        {
            Remove();
        }
    }
    
    /// <summary>
    /// Removes the debuff from the target
    /// </summary>
    public virtual void Remove()
    {
        if (target != null)
        {
            target.DebuffsToRemove.Add(this);
        }
        
    }
}
