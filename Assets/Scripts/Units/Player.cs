

/// <summary>
/// THIS CLASS IS NOT IN USE THIS WAS ONLY MENT FOR TESTING
/// </summary>
public class Player : Unit
{
    private static Player instance;

    public static Player Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Player>();
            }

            return instance;
        }
    }
}
