using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mine : ProjectileController
{
    #region Variables
    //External Objects
    public GameObject lightGameObject;

    //Colour Variables
    public List<Color> mineColourList;

    private MeshRenderer meshRenderer;

    private Light mineLight;

    //Timer Variables
    [SerializeField] private float lightBlinkRate;

    private float spawnTime;
    private float lifeTime;
    private float moveTimer;

    //Armed Variables
    private bool isArmed = false;

    //Position Variables
    private Vector3 latePosition;
    private Vector3 startPosition;
    private Vector3 mousePosition;
    private Vector3 direction;
    private Vector3 destination;
    private Vector3 targetLocation;

    private float distance;

    //Size Variables
    [SerializeField] private float sizeOffset;
    #endregion

    protected override void Start()
    {

        base.Start();

        //Get Components
        meshRenderer = lightGameObject.GetComponent<MeshRenderer>();
        mineLight = GetComponent<Light>();

        //Setting spawn time
        spawnTime = Time.time;

        //Get mouse position in world units
        mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.transform.position.y;
        mousePosition = (Camera.main.ScreenToWorldPoint(mousePosition));
        mousePosition.y = transform.position.y;

        //Set start position
        startPosition = Vector3.zero;

        //Calculate distance
        float locationDistance = Vector3.Distance(transform.position, mousePosition);
        float turretDistance = locationDistance - Vector3.Distance(transform.position, startPosition) + sizeOffset;

        //Calculate target location
        targetLocation = (transform.position * 2) + (transform.forward * turretDistance);

        //Reset rotation
        transform.rotation = Quaternion.identity;
    }

    protected override void Update()
    {
        base.Update();

        UpdateTimers();
        UpdateMovement();

        //Check if the mine is armed
        if (isArmed)
        {
            UpdateLight();
        }

        isArmed = latePosition == transform.position ? true : false;
    }

    void UpdateMovement()
    {
        //Get direction
        direction = targetLocation - transform.position;
        distance = direction.magnitude;
        direction = direction.normalized;

        //Set movespeed
        float move = bulletSpeed * Time.deltaTime;

        //Set move conditions
        if (move > distance) { move = distance; }

        //Move in set direction at given speed
        transform.Translate(direction * move);
    }

    void UpdateLight()
    {
        //Bounce number between 0 and 1
        float blinkTimer = Mathf.PingPong(lifeTime, lightBlinkRate) / lightBlinkRate;
        
        //Lerp between colours
        meshRenderer.material.color = Color.Lerp(mineColourList[0], mineColourList[1], blinkTimer);

        //Change light intensity
        mineLight.intensity = blinkTimer;
    }

    void UpdateTimers()
    {
        //Updating life time
        lifeTime = Time.time - spawnTime;

        //Update move timer
        moveTimer = Mathf.Clamp01(moveTimer += Time.deltaTime);
    }

    void LateUpdate()
    {
        //Store late position
        latePosition = transform.position;
    }
}
