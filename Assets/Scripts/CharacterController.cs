using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class CharacterController : MonoBehaviour
{
    #region Variables
    public enum WeaponState
    {
        firstEnum = -1,
        normalProjectile,
        laserBeam,
        lastEnum
    }

    public WeaponState weaponState;

    public List<Color> colourList;

    public GameObject projectile;
    public Transform barrel;
    public Transform firePoint;

    [SerializeField] private float fireRate;
    [SerializeField] private float rotateSpeed;

    [SerializeField] private float minBackFireTime;
    [SerializeField] private float maxBackFireTime;

    private float fireRateTimer;
    private float timeOfFire;

    private int scrollInput;

    public AnimationCurve barelRecoilCurve;
    Vector3 barrelStartPosition;
    #endregion

    void Update()
    {
        RotateTurret();
        UpdateState();
        UpdateInput();
        UpdateTimers();
        UpdateColour();
    }

    void UpdateTimers()
    {
        //Reduce fire rate timer
        fireRateTimer -= Time.deltaTime;

        //Check if the projectile was just fired
        if (timeOfFire + Mathf.Clamp(fireRate, minBackFireTime, maxBackFireTime) > Time.time && timeOfFire != 0)
        {
            //Calulate the position that the barrel will translate using animation curves and clamp the value between 0 and 1
            float backFireTime = barelRecoilCurve.Evaluate((Time.time - timeOfFire) / Mathf.Clamp(fireRate, minBackFireTime, maxBackFireTime));
            backFireTime = Mathf.Clamp01(backFireTime);
            Debug.Log("Back Fire Time: " + backFireTime);

            //Translate the position of the barrel using animation curves
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

        //Get scroll wheel input and alter weapon state accordingly
        scrollInput = Mathf.Clamp((int)Input.mouseScrollDelta.y, -1, 1);
        weaponState += scrollInput;
    }

    void UpdateColour()
    {
        //Update turret colour
        GetComponent<MeshRenderer>().material.color = colourList[Mathf.Clamp((int)weaponState, 0, colourList.Count)];
    }

    void UpdateState()
    {
        //Switch between weapon states
        switch (weaponState)
        {
            case WeaponState.firstEnum:
                weaponState = WeaponState.lastEnum - 1;
                break;
            case WeaponState.normalProjectile:
                break;
            case WeaponState.laserBeam:
                break;
            case WeaponState.lastEnum:
                weaponState = WeaponState.firstEnum + 1;
                break;
        }
    }

    void FireProjectile()
    {
        //Reset fire rate timer
        fireRateTimer = fireRate;

        //Spawn projectile, set position and rotation
        GameObject newProjectile = Instantiate(projectile, firePoint.position, transform.rotation) as GameObject;

        //Get the time that the projectile was fired at
        timeOfFire = Time.time;
    }

    void RotateTurret()
    {
        //Calculate turret angle using mouse position
        Vector3 currentPosition = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 mouseDirection = Input.mousePosition - currentPosition;
        float angle = Mathf.Atan2(mouseDirection.x, mouseDirection.y) * Mathf.Rad2Deg;
        
        //Rotate turret towards mouse position
        Debug.Log("Angle: " + angle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(angle, Vector3.up), Time.deltaTime * rotateSpeed);
        
        //Debug.DrawRay(firePoint.transform.position, firePoint.transform.forward);
    }

    #region Ignore
    void FireProjectile(Vector3[] directions, int ammo)
    {

        List<Action> MyActions = new List<Action>();

        MyActions.Add(FireProjectile);
        MyActions.Add(FireProjectile);

        //Reset fire rate timer
        fireRateTimer = fireRate;

        //Spawn projectile, set position and rotation
        GameObject newProjectile = Instantiate(projectile, firePoint.position, transform.rotation) as GameObject;

        timeOfFire = Time.time;

        //StartCoroutine("FireCoroutine");
    }

    void FireProjectile(int Ammo)
    {
        List<Action> MyActions = new List<Action>();

        MyActions.Add(FireProjectile);
        MyActions.Add(FireProjectile);

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
        barrelStartPosition = barrel.transform.localPosition;

        for (int f = 0; f < 60; f++)
        {
            barrel.transform.localPosition = new Vector3(barrelStartPosition.x, barrelStartPosition.y, barrelStartPosition.z + barelRecoilCurve.Evaluate((float)f / 20));
            Debug.Log((float)f / 60);

            yield return null;
        }
    }
    #endregion
}