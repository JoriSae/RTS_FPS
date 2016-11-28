using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Mine : ProjectileController
{
    #region Variables
    //External Objects
    public Image mineTimer;

    public GameObject lightGameObject;
    private GameObject turret;

    [SerializeField] private string turretName = "Turret";

    //Colour Variables
    public List<Color> mineColourList;

    private MeshRenderer meshRenderer;

    private Light mineLight;

    //Timer Variables
    [SerializeField] private float lightBlinkRate;
    private float durationTimer;

    private float spawnTime;
    private float lifeTime;
    private float moveTimer;

    //Armed Variables
    private bool isArmed = false;
    private bool timerStarted = false;

    //Position Variables
    [SerializeField] private AnimationCurve mineSpeedCurve;

    private Vector3 latePosition;
    private Vector3 startPosition;
    private Vector3 mousePosition;
    private Vector3 targetLocation;

    private float distance;

    //Size Variables
    [SerializeField] private AnimationCurve mineScaleCurve;
    #endregion

    protected override void Start()
    {
        //Set duration timer
        durationTimer = duration;

        //Find turret
        turret = GameObject.Find(turretName);

        //Get Components
        meshRenderer = lightGameObject.GetComponent<MeshRenderer>();
        mineLight = GetComponent<Light>();

        //Setting spawn time
        spawnTime = Time.time;

        CalculateDestination();
    }

    void CalculateDestination()
    {
        //Get mouse position in world units
        mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.transform.position.y;
        mousePosition = (Camera.main.ScreenToWorldPoint(mousePosition));
        mousePosition.y = transform.position.y;

        //Set start position
        startPosition = turret.transform.position;
        startPosition.y = transform.position.y;

        //Calculate distance
        float locationDistance = Vector3.Distance(startPosition, mousePosition);
        float distanceFromStart = Vector3.Distance(startPosition, transform.position);
        distance = locationDistance - distanceFromStart;

        //Calculate target location
        targetLocation = (transform.position + (transform.forward * distance));

        //If distance is in the negatives, set mine destination to be its current position
        targetLocation = locationDistance - distanceFromStart > 0 ? targetLocation : transform.position;

        //Reset rotation
        transform.rotation = Quaternion.identity;
    }

    protected override void Update()
    {
        base.Update();

        UpdateTimers();
        UpdateMovement();
        UpdateImage();

        //Check if the mine is armed
        if (isArmed) { UpdateLight(); }

        if (isArmed && !timerStarted) { timerStarted = true;  Destroy(gameObject, duration); }

        isArmed = latePosition == transform.position ? true : false;
    }

    void UpdateMovement()
    {
        //Set movespeed
        float movement = bulletSpeed * Time.deltaTime;

        //Alter movement speed using animation curves
        movement *= mineSpeedCurve.Evaluate(Vector3.Distance(transform.position, targetLocation) / distance);

        //Alter game object scale using animation curves
        gameObject.transform.localScale = Vector3.one * mineScaleCurve.Evaluate(Vector3.Distance(transform.position, targetLocation) / distance);

        //Move towards target destination
        transform.position = Vector3.MoveTowards(transform.position, targetLocation, movement);
    }

    void UpdateImage()
    {
        //Set image to be active if the mine is active
        if (timerStarted) { mineTimer.gameObject.SetActive(true); }

        //Updates image fill amount
        mineTimer.fillAmount = durationTimer / duration;

        //Lerps image colour
        mineTimer.color = Color.Lerp(mineColourList[3], mineColourList[2], durationTimer / duration);
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

        //Updating duration timer
        if (timerStarted) { durationTimer -= Time.deltaTime; }
    }

    void LateUpdate()
    {
        //Store late position
        latePosition = transform.position;
    }
}
