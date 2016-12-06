using UnityEngine;
using System.Collections;

public class GameDirector : MonoBehaviour
{
    #region Variables
    public static GameDirector instance;

    public SceneDirector sceneDirector;
    public AudioDirector audioDirector;

    public Screens currentScreen;

    public enum Screens
    {
        Splash,
        MainMenu,
        Gameplay,
        Settings,
        WeaponInfo,
        EndScreen
    }
    #endregion

    void Awake()
    {
        instance = this;
    }

	void Start()
    {
        GoToSplashScreen();
	}

    public void GoToSplashScreen()
    {
        currentScreen = Screens.Splash;
        sceneDirector.LoadScene("Splash Screen");
    }

    public void GoToMainMenuScreen()
    {
        currentScreen = Screens.MainMenu;
        sceneDirector.LoadScene("Main Menu");
        sceneDirector.UnloadScene("Splash Screen");
        sceneDirector.UnloadScene("Game Play");
        sceneDirector.UnloadScene("Settings");
        sceneDirector.UnloadScene("Weapon Info");
        sceneDirector.UnloadScene("End Screen");
        sceneDirector.UnloadScene("Pause Menu");
    }

    public void GoToSettingsScreen()
    {
        currentScreen = Screens.Settings;
        sceneDirector.LoadScene("Settings");
        sceneDirector.UnloadScene("Main Menu");
    }

    public void GoToGamePlayScreen()
    {
        currentScreen = Screens.Gameplay;
        sceneDirector.LoadScene("Game Play");
        sceneDirector.UnloadScene("Main Menu");
    }

    public void GoToWeaponInfoScreen()
    {
        currentScreen = Screens.WeaponInfo;
        sceneDirector.LoadScene("Weapon Info");
        sceneDirector.UnloadScene("Main Menu");
    }

    public void GoToEndScreen()
    {
        currentScreen = Screens.EndScreen;
        sceneDirector.LoadScene("End Screen");
        sceneDirector.UnloadScene("Game Play");
    }
}
