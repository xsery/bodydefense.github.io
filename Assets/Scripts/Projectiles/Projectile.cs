using UnityEngine;

/// <summary>
/// The projectile script
/// </summary>
public class Projectile : MonoBehaviour
{
    /// <summary>
    /// The projectile's target
    /// </summary>
    private Monster target;

    /// <summary>
    /// The projectile's tower
    /// </summary>
    private Tower tower;

    /// <summary>
    /// The projectile's animator
    /// </summary>
    private Animator myAnimator;

    /// <summary>
    /// The element type of the projectile
    /// </summary>
    private Element element;

    /// <summary>
    /// The projectile's awake function
    /// </summary>
    void Awake()
    {
        //Creates a reference to the animator
        this.myAnimator = GetComponent<Animator>();
    }

    /// <summary>
    /// Initializes the projectile
    /// </summary>
    /// <param name="tower"></param>
    public void Initialize(Tower tower)
    {
        //Sets the values
        this.target = tower.Target;
        this.tower = tower;
        this.element = tower.ElementType;
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        if (target != null && target.IsActive) //If the target isn't null and the target isn't dead
        {
            //Move towards the position
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * tower.ProjectileSpeed);

            //Calculates the direction of the projectile
            Vector2 dir = target.transform.position - transform.position;

            //Calculates the angle of the projectile
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            //Sets the rotation based on the angle
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else if(!target.IsActive) //If the target is inactive then we don't need the projectile anymore
        {
            GameManager.Instance.Pool.ReleaseObject(gameObject);
        }
    }

    /// <summary>
    /// When the projectile hits something
    /// </summary>
    /// <param name="other">The object the projectil hit</param>
    public void OnTriggerEnter2D(Collider2D other)
    {
        //If we hit a monster
        if (other.tag == "Monster")
        {
            //Creates a reference to the monster script
            Monster target = other.GetComponent<Monster>();

            //Makes the monster take damage based on the tower stats
            target.TakeDamage(tower.Damage, tower.ElementType);

            //Triggers the impact animation
            myAnimator.SetTrigger("Impact");

            //Tries to apply a debuff
            ApplyDebuff();
        }

    }

    /// <summary>
    /// Tries to apply a debuff to the target
    /// </summary>
    private void ApplyDebuff()
    {
        //Checks if the target is immune to the debuff
        if (target.ElementType != element)
        {
            //Does a roll to check if we have to apply a debuff
            float roll = UnityEngine.Random.Range(0, 100);

            if (roll <= tower.Proc)
            {   
                //applies the debuff
                target.AddDebuff(tower.GetDebuff());
            }
        }

    }
}
