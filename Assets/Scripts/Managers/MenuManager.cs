using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles all menu interaction
/// The class inherits from Singleton to make it a singleton
/// </summary>
public class MenuManager : Singleton<MenuManager>
{
    /// <summary>
    /// A reference to the MainMenu
    /// </summary>
    [SerializeField]
    private GameObject mainMenu;

    /// <summary>
    /// A reference to the options menu
    /// </summary>
    [SerializeField]
    private GameObject optionMenu;

    /// <summary>
    /// Shows the options menu
    /// </summary>
    public void ShowOptions()
    {
        mainMenu.SetActive(false);
        optionMenu.SetActive(true);
    }

    /// <summary>
    /// Shows the main menu
    /// </summary>
    public void ShowMain()
    {
        optionMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    /// <summary>
    /// Loads the scene
    /// </summary>
    public void Play()
    {
        //Loads the ingame level
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Quits to the main menu
    /// </summary>
    public void QuitToMain()
    {
        //Sets timescale to 1 to unpause
        Time.timeScale = 1;

        //Loads the scene manager
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Shows the inagame menu
    /// </summary>
    public void ShowIngameMenu()
    {
        //checks if the menu is active
        if (optionMenu.activeSelf)
        {
            //Shows the main menu
            ShowMain();
        }
        else//If the options menu isn't active
        {
            //Sets the menu to active
            mainMenu.SetActive(!mainMenu.activeSelf);

            if (!mainMenu.activeSelf) //if we deactivate we unpause
            {
                Time.timeScale = 1;
            }
            else //If we activate we pause
            {
                Time.timeScale = 0;
            }
        }

    }
}
