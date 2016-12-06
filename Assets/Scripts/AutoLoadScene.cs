using UnityEngine;
using System.Collections;
using System;

public class AutoLoadScene : MonoBehaviour
{
    #region
    public float autoLoadSceneTime;
    public string methodToCall;
    #endregion

    void Start()
    {
        GameDirector.instance.Invoke(methodToCall, autoLoadSceneTime);
	}

    private void CallFunction()
    {
        GameDirector.instance.GoToMainMenuScreen();
    }
}
