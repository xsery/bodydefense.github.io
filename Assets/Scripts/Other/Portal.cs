using UnityEngine;

/// <summary>
/// This script controls the portals
/// </summary>
public class Portal : MonoBehaviour
{
    /// <summary>
    /// Animator for the portal
    /// </summary>
    private Animator myAnimator;

    // Use this for initialization
    void Start ()
    {
        myAnimator = GetComponent<Animator>();
	}

    /// <summary>
    /// Animates the portal
    /// </summary>
    public void Animate()
    {
        myAnimator.SetTrigger("Spawn");
    }
}
