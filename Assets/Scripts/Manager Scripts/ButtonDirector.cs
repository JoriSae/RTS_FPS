using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonDirector : MonoBehaviour
{
    private Button button;
    public string methodToCall;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(LeftClick);
    }
	
    protected void LeftClick()
    {
        GameDirector.instance.Invoke(methodToCall, 0);
    }
}
