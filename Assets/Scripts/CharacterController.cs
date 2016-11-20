using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour
{
    #region Variables
    public Transform retical;
    public Transform barrel;
    public GameObject playerBase;

    //Weapon Variables
    public GameObject projectile;
    public Transform firePoint;
    [SerializeField] private float fireRate;
    [SerializeField] private float rotateSpeed;
    private float fireRateTimer;
    private float timeOfFire;

    public AnimationCurve barelRecoilCurve;
    Vector3 barrelStartPosition;

    private bool test = false;
    #endregion

    // Use this for initialization
    void Start ()
    {
        
        
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        RotateTurret();
        UpdateInput();
        //UpdateRotation();
        UpdateTimers();

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            test = !test;
        }
	}

    void UpdateRotation()
    {
        //Look at target
        transform.LookAt(retical.position);
    }

    void UpdateTimers()
    {
        //Reduce fire rate timer
        fireRateTimer -= Time.deltaTime;

        if (timeOfFire + Mathf.Clamp01(fireRate) > Time.time && timeOfFire != 0)
        {
            float backFireTime = barelRecoilCurve.Evaluate((Time.time - timeOfFire) / Mathf.Clamp01(fireRate));
            backFireTime = Mathf.Clamp(backFireTime, 0.3f, 1.0f);
            print(backFireTime);
            barrel.transform.localPosition = new Vector3(barrelStartPosition.x, barrelStartPosition.y, barrelStartPosition.z + backFireTime);
        }
    }

    void UpdateInput()
    {
        //Check if the player can fire and if the assigned fire key is pressed, if so fire projectile
        if (Input.GetMouseButton(0) && fireRateTimer <= 0)
        {
            FireProjectile();
        }

        if (Input.GetKey(KeyCode.W))
        {
            playerBase.transform.Translate(Vector3.forward * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            playerBase.transform.Translate(Vector3.back * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.A))
        {
            playerBase.transform.Rotate(Vector3.down * Time.deltaTime * 20);
        }
        if (Input.GetKey(KeyCode.D))
        {
            playerBase.transform.Rotate(Vector3.up * Time.deltaTime * 20);
        }
    }

    void FireProjectile()
    {
        //Reset fire rate timer
        fireRateTimer = fireRate;

        //Spawn projectile, set position and rotation
        GameObject newProjectile = Instantiate(projectile, firePoint.position, transform.rotation) as GameObject;
        //newProjectile.GetComponent<>()
        timeOfFire = Time.time;
        //StartCoroutine("FireCoroutine");
    }

    IEnumerator FireCoroutine()
    {
        Debug.Log("ONLY ONCE NIGGA!");
        barrelStartPosition = barrel.transform.localPosition;

        

        for (int f = 0; f < 60; f++)
        {
            barrel.transform.localPosition = new Vector3(barrelStartPosition.x, barrelStartPosition.y, barrelStartPosition.z + barelRecoilCurve.Evaluate((float)f / 20));
            Debug.Log((float)f / 60);

            yield return null;
        }
    }

    void RotateTurret()
    {
        Vector3 currentPosition = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 mouseDirection = Input.mousePosition - currentPosition;
        float angle = Mathf.Atan2(mouseDirection.x, mouseDirection.y) * Mathf.Rad2Deg;

        if (test)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.up), rotateSpeed * Time.deltaTime);
        }
        else
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(angle, Vector3.up), Time.deltaTime * 50);
        }
    }
}
