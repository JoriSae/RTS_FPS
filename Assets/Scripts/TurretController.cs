using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEditor;

public class TurretController : MonoBehaviour
{
    #region Variables
    //States
    public enum WeaponState
    {
        firstEnum = -1,
        normalProjectile,
        laserBeam,
        lastEnum
    }

    public WeaponState weaponState;

    //Editor Toggle Variables
    bool toggleColours = false;

    //External Objects
    public List<Color> colourList;
    
    private MeshRenderer turretMaterial;
    private MeshRenderer muzzelMaterial;

    public List<ProjectileController> projectiles;
    public ProjectileController currentProjectile;

    public Transform barrel;
    public Transform firePoint;

    public GameObject muzzel;
    public GameObject top;

    public Vector3 barrelOrigin;

    //Firing Variables
    private bool firing;

    private float fireRate;
    private float timeOfFire;

    //Speed Variables
    public float rotateSpeed;

    //Back Fire Variables
    public AnimationCurve inBarrelRecoilCurve;
    public AnimationCurve outBarrelRecoilCurve;

    Vector3 barrelStartPosition;

    public float minBackFire;
    public float maxBackFire;

    public float fireAnimationTime;
    private float reloadAnimationTime;

    private float backFireTime;

    //Muzzel Variables
    private Vector3 initialMuzzelSize;
    public float muzzelExpansionSize;

    //Weapon Swap Variables
    public float weaponSwapDelay;
    private float weaponSwapTimer;

    private int scrollInput;
    #endregion

    void Start()
    {
        //Get component
        muzzelMaterial = muzzel.GetComponent<MeshRenderer>();
        turretMaterial = top.GetComponent<MeshRenderer>();

        //Set start position
        barrelStartPosition = gameObject.transform.localPosition + barrelOrigin;

        initialMuzzelSize = muzzel.GetComponent<Transform>().localScale;
    }

    void Update()
    {
        UpdateInput();
        UpdateState();
        RotateTurret();
        UpdateTimers();
        UpdateWeapon();
    }

    void UpdateTimers()
    {
        //Reduce weapon swap timer
        weaponSwapTimer -= Time.deltaTime;
    }

    void UpdateInput()
    {
        //Check if the player can fire and if the assigned fire key is pressed, if so fire projectile
        if (Input.GetMouseButton(0) && !firing) { StartCoroutine("FireCoroutine"); }

        //Get scroll wheel input and alter weapon state accordingly
        scrollInput = Mathf.Clamp((int)Input.mouseScrollDelta.y, -1, 1);

        //Update weapon state using scroll input
        if (weaponSwapTimer <= 0 && scrollInput != 0 && !firing)
        {
            weaponState += scrollInput;
            weaponSwapTimer = weaponSwapDelay;
        }
    }

    void UpdateWeapon()
    {
        //Update turret colour
        turretMaterial.material.color = colourList[(int)weaponState];

        //Update turret projectile
        currentProjectile = projectiles[(int)weaponState];

        //Update firerate
        fireRate = currentProjectile.fireRate;
    }

    void UpdateState()
    {
        if (weaponState == WeaponState.firstEnum) { weaponState = WeaponState.lastEnum - 1; }
        else if (weaponState == WeaponState.lastEnum) { weaponState = WeaponState.firstEnum + 1; }

        //Switch between weapon states
        switch (weaponState)
        {
            case WeaponState.normalProjectile:
                break;
            case WeaponState.laserBeam:
                break;
        }
    }

    void FireProjectile()
    {
        //Spawn projectile, set position and rotation
        GameObject newProjectile = Instantiate(currentProjectile, firePoint.position, transform.rotation) as GameObject;
    }

    void RotateTurret()
    {
        //Calculate turret angle using mouse position
        Vector3 currentPosition = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 mouseDirection = Input.mousePosition - currentPosition;
        float angle = Mathf.Atan2(mouseDirection.x, mouseDirection.y) * Mathf.Rad2Deg;
        
        //Rotate turret towards mouse position
        //Debug.Log("Angle: " + angle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(angle, Vector3.up), Time.deltaTime * rotateSpeed);
        
        //Debug.DrawRay(firePoint.transform.position, firePoint.transform.forward);
    }

    IEnumerator FireCoroutine()
    {
        firing = true;

        //Fire Projectile
        FireProjectile();

        //Get the time that the projectile was fired at
        timeOfFire = Time.time;

        while (timeOfFire + fireAnimationTime > Time.time)
        {
            //Calulate the position that the barrel will translate using animation curves and clamp the value between 0 and 1
            backFireTime = inBarrelRecoilCurve.Evaluate((Time.time - timeOfFire) / fireAnimationTime);
            backFireTime = Mathf.Clamp(backFireTime, minBackFire, maxBackFire);

            //Lerp colours
            muzzelMaterial.material.color = Color.Lerp(Color.gray, colourList[(int)weaponState], (Time.time - timeOfFire) / fireAnimationTime);
            muzzel.transform.localScale = Vector3.Lerp(initialMuzzelSize, initialMuzzelSize * muzzelExpansionSize, (Time.time - timeOfFire) / fireAnimationTime);

            //Translate the position of the barrel using animation curves
            barrel.transform.localPosition = new Vector3(barrelStartPosition.x, barrelStartPosition.y, barrelStartPosition.z + backFireTime);

            yield return null;
        }

        while (timeOfFire + fireRate > Time.time)
        {
            //Calulate the position that the barrel will translate using animation curves and clamp the value between 0 and 1
            backFireTime = outBarrelRecoilCurve.Evaluate((Time.time - timeOfFire) / fireRate - fireAnimationTime);
            backFireTime = Mathf.Clamp(backFireTime, minBackFire, maxBackFire);

            //Lerp colours
            muzzelMaterial.material.color = Color.Lerp(colourList[(int)weaponState], Color.gray, (Time.time - timeOfFire) / fireRate);
            muzzel.transform.localScale = Vector3.Lerp(initialMuzzelSize * muzzelExpansionSize, initialMuzzelSize, (Time.time - timeOfFire) / fireRate);

            //Translate the position of the barrel using animation curves
            barrel.transform.localPosition = new Vector3(barrelStartPosition.x, barrelStartPosition.y, barrelStartPosition.z + backFireTime);

            yield return null;
        }

        firing = false;
    }
    #region Ignore
    void FireProjectile(Vector3[] directions, int ammo)
    {

        List<Action> MyActions = new List<Action>();

        MyActions.Add(FireProjectile);
        MyActions.Add(FireProjectile);

        //Spawn projectile, set position and rotation
        GameObject newProjectile = Instantiate(currentProjectile, firePoint.position, transform.rotation) as GameObject;

        timeOfFire = Time.time;

        //StartCoroutine("FireCoroutine");
    }

    void FireProjectile(int Ammo)
    {
        List<Action> MyActions = new List<Action>();

        MyActions.Add(FireProjectile);
        MyActions.Add(FireProjectile);

        //Spawn projectile, set position and rotation
        GameObject newProjectile = Instantiate(currentProjectile, firePoint.position, transform.rotation) as GameObject;
        //newProjectile.GetComponent<>()
        timeOfFire = Time.time;
        //StartCoroutine("FireCoroutine");
    }
    #endregion
}