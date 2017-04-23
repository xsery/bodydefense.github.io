using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This script is attached to all monsters
/// It's a Unit sub class
/// </summary>
public class Monster : Unit
{
    /// <summary>
    /// The monsters health stat
    /// </summary>
    [SerializeField]
    private Stat monsterHealth;

    /// <summary>
    /// The monster's element type
    /// </summary>
    [SerializeField]
    private Element elementType;

    /// <summary>
    /// The monsters debuffs
    /// </summary>
    private List<Debuff> debuffs = new List<Debuff>();

    /// <summary>
    /// New debuffs that we will be adding to the monsters
    /// </summary>
    public List<Debuff> newDebuffs = new List<Debuff>();

    /// <summary>
    /// Debuffs that we will be removing from the monster
    /// </summary>
    public List<Debuff> DebuffsToRemove { get; private set; }

    /// <summary>
    /// The amount of damage reduction the monster will take from it's own type
    /// </summary>
    private int invulnerability = 2;

    /// <summary>
    /// The monster's max movement speed
    /// </summary>
    public float MaxSpeed { get; private set; }

    /// <summary>
    /// Proprty for checking if the monster is alive
    /// </summary>
    public bool Alive
    {
        get
        {
            return monsterHealth.CurrentValue > 0;
        }
    }

    /// <summary>
    /// Property for getting the element type of the monster
    /// </summary>
    public Element ElementType
    {
        get
        {
            return elementType;
        }
    }

    protected override void Awake()
    {
        //Call awake on the Unit class
        base.Awake();

        ///Instantiates the debuffs to remove list
        DebuffsToRemove = new List<Debuff>();

        //Sets the monsters maxspeed
        MaxSpeed = Speed;

        //Initializes the monsters health bar
        monsterHealth.Initialize();
    }

    protected override void Update()
    {
        //Updates all buffs
        HandleDebuffs();

        //Calls update on the Unit class
        base.Update();
    }

    /// <summary>
    /// Adds a debuff to the monster
    /// </summary>
    /// <param name="debuff">The type of debuff to add</param>
    public void AddDebuff(Debuff debuff)
    {
        //Checks if the debuff already exists before adding it
        //If it doesn't exist then we add it
        if (!debuffs.Exists(x => x.GetType() == debuff.GetType()))
        {
            //Adds the debuff to new debuffs
            newDebuffs.Add(debuff);
        }
    }

    /// <summary>
    /// Handles the debuffs
    /// </summary>
    private void HandleDebuffs()
    {
        //If the monster has any new debuffs
        if (newDebuffs.Count > 0)
        {
            //Then we add them to the debuffs list
            debuffs.AddRange(newDebuffs);

            //Then clear new debuffs so that they only will be added once
            newDebuffs.Clear();
        }

        //Checks if we need to remove any debuffs
        foreach (Debuff debuff in DebuffsToRemove)
        {
            //If so then remove it
            debuffs.Remove(debuff);
        }

        //Clears the debuffs to remove, so that we only try to remove them once
        DebuffsToRemove.Clear();

        //Updates all debuffs
        foreach (Debuff debuff in debuffs) 
        {
            debuff.Update();
        }
    }

    /// <summary>
    /// Makes the monster take damage
    /// </summary>
    /// <param name="damage">The amount of damage to take</param>
    /// <param name="dmgSource">The damage source type</param>
    public void TakeDamage(float damage, Element dmgSource)
    {
        if (IsActive)//Checks if the monster is in play
        {
            //Calculates the amount of damage that the monster should take
            if (dmgSource == elementType)
            {
                damage = damage / invulnerability;
                invulnerability++;
            }

            //Reduces the monster's health
            monsterHealth.CurrentValue -= damage;

            //Checks if the monster is dead
            if (monsterHealth.CurrentValue <= 0)
            {
                //Plays a sound effect
                SoundManager.Instance.PlaySFX("Splat");

                //Adds some currency to the player
                GameManager.Instance.Currency += 2;

                //Handles the death
                myAnimator.SetTrigger("Die");
                GetComponent<SpriteRenderer>().sortingOrder--;
                IsActive = false;       
            }
        }
    }

    /// <summary>
    /// This function is used for scaling the monster up and down when it exits or enters a portal
    /// </summary>
    /// <param name="from">The scale it will scale from</param>
    /// <param name="to">The scale it will scale to</param>
    /// <param name="remove">Indicates if we need to remove the monster after scaling</param>
    /// <returns></returns>
    public IEnumerator Scale(Vector3 from, Vector3 to, bool remove)
    {
        //The scaling progress
        float progress = 0;

        //As long as the progress is les than 1, then we need to keep scaling
        while (progress <= 1)
        {
            //Scales themonster
            transform.localScale = Vector3.Lerp(from, to, progress);
            progress += Time.deltaTime * 1;
            yield return null;
        }

        //Makes sure that is has the correct scale after scaling
        transform.localScale = to;

        IsActive = true;

        //Checks if we need to remove the monster, if so then we remove it
        if (remove)
        {
            Release();
        }

    }

    /// <summary>
    /// Spawns the monster in our world
    /// </summary>
    /// <param name="health">The monsters health</param>
    public void Spawn(int health)
    {
        //Resets the health
        this.monsterHealth.MaxVal = health;
        this.monsterHealth.CurrentValue = health;

        //removes all debuffs
        debuffs.Clear();

        //Sets the position
        transform.position = LevelManager.Instance.BluePortal.transform.position;

        //Starts to scale the monsters
        StartCoroutine(Scale(new Vector3(0.1f, 0.1f), new Vector3(1, 1), false));

        //Sets the monsters path
        SetPath(LevelManager.Instance.Path, false);
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);

        //If we hit the red portal, then we need to despawn
        if (other.name == "RedPortal")
        {
            //Animates the portal
            other.GetComponent<Portal>().Animate();

            //Scales the monster down
            StartCoroutine(Scale(new Vector3(1, 1), new Vector3(0.1f, 0.1f), true));

            //Reduces the amount of player lives
            GameManager.Instance.Lives--;
        }
    }

    /// <summary>
    /// Releases the monster from the game
    /// </summary>
    public void Release()
    {
        //removes all debuffs
        foreach (Debuff debuff in debuffs)
        {
            debuff.Remove();
        }

        //Resets the monsters position
        GridPosition = new Point(0, 0);

        //Resets the monsters speed back to normal
        Speed = MaxSpeed;

        //Removes the monster from the game
        GameManager.Instance.RemoveMonster(this);

        //Releases the monster from the object pool
        GameManager.Instance.Pool.ReleaseObject(gameObject);
    }
}
