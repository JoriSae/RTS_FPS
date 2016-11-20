using UnityEngine;
using System.Collections;

public class ReticalController : MonoBehaviour {

    Vector3 target = Vector3.zero;

    [SerializeField]
    float lerpSpeed;


    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        RaycastHit hitInfo;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hitInfo))
        {
            target = new Vector3(hitInfo.point.x, transform.position.y, hitInfo.point.z);
        }

        //Position lerp
        Vector3 newPosition = transform.position;
        newPosition.x = Mathf.Lerp(newPosition.x, target.x, lerpSpeed);
        newPosition.y = Mathf.Lerp(newPosition.y, target.y, lerpSpeed);
        newPosition.z = Mathf.Lerp(newPosition.z, target.z, lerpSpeed);
        transform.position = newPosition;
    }
}
