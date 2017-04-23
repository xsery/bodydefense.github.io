using UnityEngine;

/// <summary>
/// The hover icon, inherits from singleton to make it into a singleton
/// </summary>
public class Hover : Singleton<Hover>
{
    /// <summary>
    /// A reference to the icon's spriterenderer
    /// </summary>
    private SpriteRenderer spriteRenderer;

    /// <summary>
    /// A referenceo to the rangedcheck on the tower
    /// </summary>
    private SpriteRenderer rangeSpriteRenderer;

    /// <summary>
    /// Indicates if the hover icon is visible
    /// </summary>
    public bool Visible { get; set; }

    void Awake()
    {
        //Creates the references
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.rangeSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (spriteRenderer.enabled) //If the hover is enabled
        {
            //Makes sure that it follows the mouse
            FollowMouse();
        }
   
	}

    /// <summary>
    /// Makes the hover icon follow the mouse
    /// </summary>
    private void FollowMouse()
    {
        //Translates the mouse on screen position into a world position
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //Resets the Z value to 0
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    /// <summary>
    /// Activates the hover icon
    /// </summary>
    /// <param name="sprite">The sprite to show on the hover icon</param>
    public void Activate(Sprite sprite)
    {
        Visible = true;
        spriteRenderer.enabled = true;
        rangeSpriteRenderer.enabled = true;
        spriteRenderer.sprite = sprite;
    }

    /// <summary>
    /// Deactivates the hover icon (hides it)
    /// </summary>
    public void Deactivate()
    {
        Visible = false;
        rangeSpriteRenderer.enabled = false;
        spriteRenderer.enabled = false;
    }
}
