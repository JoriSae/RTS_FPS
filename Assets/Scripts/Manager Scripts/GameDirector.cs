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

	// Use this for initialization
	void Start()
    {
	    
	}
	
	// Update is called once per frame
	void Update()
    {
	
	}

    void GoToSplashScreen()
    {
        currentScreen = Screens.Splash;
        //Load Splash Screen
    }

    void GoToMainMenu()
    {
        //Unload Splash Screen
    }
}
