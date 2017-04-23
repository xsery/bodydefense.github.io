using UnityEngine;
using System.Collections;

/// <summary>
/// The splash script
/// </summary>
public class PoisonSplash : MonoBehaviour
{
    /// <summary>
    /// The damage of the splash
    /// </summary>
    public int Damage { get; set; }

    /// <summary>
    /// When something hits the splash
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter2D(Collider2D other)
    {
        //If a monster hits the splash
        if (other.tag == "Monster")
        {
            //Gives the monster damage
            other.GetComponent<Monster>().TakeDamage(Damage, Element.POISON);

            //Removes the splash
            Destroy(gameObject);
        }
    }
}
