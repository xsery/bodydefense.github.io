using UnityEngine;

/// <summary>
/// Script for moving the camera
/// </summary>
public class CameraMovement : MonoBehaviour
{   
    /// <summary>
    /// The max x value of the camera
    /// </summary>
    public float xMax;

    /// <summary>
    /// The min y balue of the camera
    /// </summary>
    public float yMin;

    /// <summary>
    /// The camera's movement speed
    /// </summary>
    [SerializeField]
    private float cameraSpeed = 0;

    // Update is called once per frame
    void Update()
    {
        //Camera movement
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.up * cameraSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * cameraSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.down * cameraSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * cameraSpeed * Time.deltaTime);
        }
      
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, 0, xMax), Mathf.Clamp(transform.position.y, yMin, 0), -10);
    }

    /// <summary>
    /// Sets the limits
    /// </summary>
    /// <param name="maxtile">The max tile position</param>
    public void SetLimits(Vector3 maxtile)
    {
        //Get the world position of the maxtile
        Vector3 p = Camera.main.ViewportToWorldPoint(new Vector3(1, 0));
        
        //Gets the world pos of the max tile
        xMax = maxtile.x - p.x;
        yMin = maxtile.y - p.y;

    }

}
