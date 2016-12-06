using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class TurretController : MonoBehaviour
{
    #region Variables
    //States
    public enum WeaponState
    {
        firstEnum = -1,
        normalProjectile,
        mine,
        laserBeam,
        lastEnum
    }

    public WeaponState weaponState;

    public List<WeaponState> weaponStates = new List<WeaponState>();

    //External Objects
    public List<Color> colourList;
    
    private MeshRenderer turretMaterial;
    private MeshRenderer muzzelMaterial;

    public List<ProjectileController> projectiles;
    public ProjectileController currentProjectile;

    public Transform barrel;
    public Transform firePoint;

    public GameObject muzzle;
    public GameObject top;
    public GameObject barrelAnchor;

    //Audio
    public AudioClip firingMine;
    public AudioClip firingLaser;
    public AudioClip firingNormalProjectile;
    public AudioClip laserRecharge;

    //Firing Variables
    private ProjectileController newProjectile;

    private bool isfiring;

    private float fireRate;
    private float timeOfFire;

    //Speed Variables
    public float rotateSpeed;

    //Back Fire Variables
    public AnimationCurve inBarrelRecoilCurve;
    public AnimationCurve outBarrelRecoilCurve;

    private Vector3 barrelStartPosition;
    private Vector3 barrelOffset;

    public float barrelBackFirePercentage;
    public float minBackFire;
    public float maxBackFire;

    public float fireAnimationTime;
    private float reloadAnimationTime;

    private float backFireTime;

    //Muzzel Variables
    private Vector3 initialMuzzelSize;
    public float muzzelExpansionSize;

    //Top Variables
    private Vector3 topOffset;
    private Vector3 initialTopSize;
    private Vector3 topStartPosition;
    public float topExpansionSize;
    public float topBackFirePercentage;

    //Weapon Swap Variables
    public float weaponSwapDelay;
    private float weaponSwapTimer;

    private int scrollInput;
    #endregion

    void Start()
    {
        //Get component
        muzzelMaterial = muzzle.GetComponent<MeshRenderer>();
        turretMaterial = top.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();

        //Set offset
        topOffset.z = -topBackFirePercentage;
        barrelOffset.z = -barrelBackFirePercentage;

        //Set start position
        barrelStartPosition = gameObject.transform.localPosition + barrelOffset;
        topStartPosition = top.transform.localPosition + topOffset;

        //Get current scale
        initialMuzzelSize = muzzle.GetComponent<Transform>().localScale;
        initialTopSize = top.GetComponent<Transform>().localScale;
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
        if (Input.GetMouseButton(0) && !isfiring) { CheckState(); }

        //Get scroll wheel input and alter weapon state accordingly
        scrollInput = Mathf.Clamp((int)Input.mouseScrollDelta.y, -1, 1);

        //Update weapon state using scroll input
        if (weaponSwapTimer <= 0 && scrollInput != 0 && !isfiring)
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
        if (newProjectile != null) { fireRate = newProjectile.fireRate; }
    }

    void UpdateState()
    {
        if (weaponState == WeaponState.firstEnum) { weaponState = WeaponState.lastEnum - 1; }
        else if (weaponState == WeaponState.lastEnum) { weaponState = WeaponState.firstEnum + 1; }
    }

    void CheckState()
    {
        //Switch between weapon states
        switch (weaponState)
        {
            case WeaponState.normalProjectile:
                StartCoroutine(FireCoroutine(firingNormalProjectile, null, null, weaponStates));
                break;
            case WeaponState.mine:
                StartCoroutine(FireCoroutine(firingMine, null, null, weaponStates));
                break;
            case WeaponState.laserBeam:
                StartCoroutine(FireCoroutine(null, firingLaser, laserRecharge, weaponStates));
                break;
        }
    }

    void FireProjectile()
    {
        //Spawn projectile, set position and rotation
        GameObject newProjectileObject = Instantiate(currentProjectile.gameObject, firePoint.position, transform.rotation) as GameObject;
        newProjectile = newProjectileObject.GetComponent<ProjectileController>();
    }

    void RotateTurret()
    {
        //Get mouse position in world units
        Vector3 mousePosition = Camera.main.WorldToScreenPoint(transform.position);

        //Get mouse heading
        Vector3 mouseHeading = Input.mousePosition - mousePosition;

        //Get mouse angle
        float angle = Mathf.Atan2(mouseHeading.x, mouseHeading.y) * Mathf.Rad2Deg;
        
        //Rotate turret towards mouse position
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(angle, Vector3.up), Time.deltaTime * rotateSpeed);
    }

    IEnumerator FireCoroutine(AudioClip _initialFire, AudioClip _constantFire, AudioClip _recharge, List<WeaponState> _weaponState)
    {
        //Set firing to true
        isfiring = true;

        //Fire Projectile
        FireProjectile();

        //Play audio
        GameDirector.instance.audioDirector.PlayAudio(GameDirector.instance.audioDirector.soundEffectsSource, _initialFire);

        //Get the time that the projectile was fired at
        timeOfFire = Time.time;

        while (timeOfFire + fireAnimationTime + Time.deltaTime > Time.time)
        {
            //Calulate the position that the barrel will translate using animation curves and clamp the value between 0 and 1
            backFireTime = inBarrelRecoilCurve.Evaluate((Time.time - timeOfFire) / fireAnimationTime);
            backFireTime = Mathf.Clamp(backFireTime, minBackFire, maxBackFire);
            float timeSinceProjectileWasFired = (Time.time - timeOfFire) / fireAnimationTime;

            //Lerp muzzel scale and colour using animation curves
            muzzelMaterial.material.color = Color.Lerp(Color.gray, colourList[(int)weaponState], timeSinceProjectileWasFired);
            muzzle.transform.localScale = Vector3.Lerp(initialMuzzelSize, initialMuzzelSize * muzzelExpansionSize, timeSinceProjectileWasFired);

            //Lerp top scale and position using animation curves
            Vector3 turretTopBackFirePosition = new Vector3(topStartPosition.x, topStartPosition.y, topStartPosition.z + (backFireTime * topBackFirePercentage));
            top.transform.localPosition = turretTopBackFirePosition;
            top.transform.localScale = Vector3.Lerp(initialTopSize, new Vector3(initialTopSize.x * topExpansionSize, initialTopSize.y * topExpansionSize, initialTopSize.z), timeSinceProjectileWasFired);

            //Translate the position of the barrel using animation curves
            Vector3 barrelBackFirePosition = new Vector3(barrelStartPosition.x, barrelStartPosition.y, barrelStartPosition.z + (backFireTime * barrelBackFirePercentage));
            barrel.transform.localPosition = barrelBackFirePosition;

            yield return null;
        }

        GameDirector.instance.audioDirector.PlayAudio(GameDirector.instance.audioDirector.loopedSoundEffectsSource, _constantFire);

        //Check if weapon is laser, if so yeild and update time of fire until the left mouse button is no longer pressed
        for (int index = 0; index < _weaponState.Count; index++)
        {
            while (weaponState == _weaponState[index] && newProjectile)
            {
                timeOfFire = Time.time - fireAnimationTime;

                yield return null;
            }
        }

        GameDirector.instance.audioDirector.StopAudio(GameDirector.instance.audioDirector.loopedSoundEffectsSource);

        GameDirector.instance.audioDirector.PlayAudio(GameDirector.instance.audioDirector.loopedSoundEffectsSource, _recharge);

        while (timeOfFire + fireRate > Time.time)
        {
            //Calulate the position that the barrel will translate using animation curves and clamp the value between 0 and 1
            backFireTime = outBarrelRecoilCurve.Evaluate((Time.time - timeOfFire) / fireRate - fireAnimationTime);
            backFireTime = Mathf.Clamp(backFireTime, minBackFire, maxBackFire);
            float timeSinceProjectileWasFired = (Time.time - timeOfFire) / fireRate;

            //Lerp muzzel scale and colour using animation curves
            muzzelMaterial.material.color = Color.Lerp(colourList[(int)weaponState], Color.gray, timeSinceProjectileWasFired);
            muzzle.transform.localScale = Vector3.Lerp(initialMuzzelSize * muzzelExpansionSize, initialMuzzelSize, timeSinceProjectileWasFired);

            //Lerp top scale and position using animation curves
            Vector3 turretTopBackFirePosition = new Vector3(topStartPosition.x, topStartPosition.y, topStartPosition.z + (backFireTime * topBackFirePercentage));
            top.transform.localPosition = turretTopBackFirePosition;
            top.transform.localScale = Vector3.Lerp(new Vector3(initialTopSize.x * topExpansionSize, initialTopSize.y * topExpansionSize, initialTopSize.z), initialTopSize, timeSinceProjectileWasFired);

            //Translate the position of the barrel using animation curves
            Vector3 barrelBackFirePosition = new Vector3(barrelStartPosition.x, barrelStartPosition.y, barrelStartPosition.z + (backFireTime * barrelBackFirePercentage));
            barrel.transform.localPosition = barrelBackFirePosition;

            yield return null;
        }

        GameDirector.instance.audioDirector.StopAudio(GameDirector.instance.audioDirector.loopedSoundEffectsSource);

        //Set firing to false
        isfiring = false;
    }
    #region Ignore
    void FireProjectile(Vector3[] directions, int ammo)
    {

        List<Action> MyActions = new List<Action>();

        MyActions.Add(FireProjectile);
        MyActions.Add(FireProjectile);

        //Spawn projectile, set position and rotation
        //GameObject newProjectile = Instantiate(currentProjectile, firePoint.position, transform.rotation) as GameObject;

        timeOfFire = Time.time;

        //StartCoroutine("FireCoroutine");
    }

    void FireProjectile(int Ammo)
    {
        List<Action> MyActions = new List<Action>();

        MyActions.Add(FireProjectile);
        MyActions.Add(FireProjectile);

        //Spawn projectile, set position and rotation
        //GameObject newProjectile = Instantiate(currentProjectile, firePoint.position, transform.rotation) as GameObject;
        //newProjectile.GetComponent<>()
        timeOfFire = Time.time;
        //StartCoroutine("FireCoroutine");
    }
    #endregion
}