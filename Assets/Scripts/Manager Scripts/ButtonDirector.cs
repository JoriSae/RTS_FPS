using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonDirector : MonoBehaviour
{
    private Button button;
    public string sceneToLoad;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(LeftClick);
    }

	// Use this for initialization
	void Start()
    {
	    
	}
	
    protected void LeftClick()
    {
        GameDirector.instance.sceneDirector.LoadScene(sceneToLoad);
    }
}
