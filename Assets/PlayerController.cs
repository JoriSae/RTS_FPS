using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public List<string> inputQueue = new List<string>();

    #region Variables
    enum CharacterState
    {
        firstPerson,
        thirdPerson
    }

    public Camera firstPersonCamera;
    public Camera thirdPersonCamera;

    CharacterState characterState;

    [SerializeField] private float moveSpeed;
    [SerializeField] private bool selected;
    #endregion

    // Use this for initialization
    void Start()
    {
	
	}
	
	// Update is called once per frame
	void Update()
    {
        UpdateInput();
	}
    
    void UpdateInput()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            characterState = (characterState == CharacterState.firstPerson) ? characterState = CharacterState.thirdPerson : characterState = CharacterState.firstPerson;
        }

        switch (characterState)
        {
            case CharacterState.firstPerson:
                FirstPersonMovement();
                break;
            case CharacterState.thirdPerson:
                ThirdPersonMovement();
                break;
        }
    }

    void MoveOrder()
    {
        RaycastHit hitInfo;
        Ray ray = thirdPersonCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
        {
            gameObject.transform.LookAt(new Vector3(hitInfo.point.x, transform.position.y, hitInfo.point.z));
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
    }

    void ThirdPersonMovement()
    {
        if (Input.GetMouseButton(1))
        {
            MoveOrder();
        }
    }

    //void OnDrawGizmos()
    //{
    //    Ray ray;
    //    RaycastHit hitInfo;
    //    Debug.Log(Camera.main.ScreenPointToRay(Input.mousePosition));
    //    ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //    if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
    //    {
    //        Gizmos.DrawCube(hitInfo.point, Vector3.one);
    //    }
    //}

    void FirstPersonMovement()
    {
        RaycastHit hitInfo;
        Ray ray = firstPersonCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
        {
            gameObject.transform.LookAt(new Vector3(hitInfo.point.x, transform.position.y, hitInfo.point.z));
        }

        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        }
    }

    //void MitchellsSolution()
    //{
    //    #region Movement Inputs
    //    //this code allows the most reciently pressed key to always be considered the current key 
    //
    //    //Adding to input queue
    //    if (Input.GetKeyDown(KeyCode.RightArrow))
    //    {
    //        inputQueue.Add("Right");
    //    }
    //    if (Input.GetKeyDown(KeyCode.LeftArrow))
    //    {
    //        inputQueue.Add("Left");
    //    }
    //    if (Input.GetKeyDown(KeyCode.UpArrow))
    //    {
    //        inputQueue.Add("Up");
    //    }
    //    if (Input.GetKeyDown(KeyCode.DownArrow))
    //    {
    //        inputQueue.Add("Down");
    //    }
    //
    //    //Removing from input queue
    //    if (Input.GetKeyUp(KeyCode.RightArrow))
    //    {
    //        inputQueue.Remove("Right");
    //    }
    //    if (Input.GetKeyUp(KeyCode.LeftArrow))
    //    {
    //        inputQueue.Remove("Left");
    //    }
    //    if (Input.GetKeyUp(KeyCode.UpArrow))
    //    {
    //        inputQueue.Remove("Up");
    //    }
    //    if (Input.GetKeyUp(KeyCode.DownArrow))
    //    {
    //        inputQueue.Remove("Down");
    //    }
    //
    //    //Reset inputs
    //    v_input = 0;
    //    h_input = 0;
    //
    //    //Input Check
    //    if (inputQueue.Count > 0)
    //    {
    //        switch (inputQueue[inputQueue.Count - 1])
    //        {
    //            case "Right":
    //                h_input = 1;
    //                break;
    //            case "Left":
    //                h_input = -1;
    //                break;
    //            case "Up":
    //                v_input = 1;
    //                break;
    //            case "Down":
    //                v_input = -1;
    //                break;
    //        }
    //    }
    //    #endregion
    //}
}
