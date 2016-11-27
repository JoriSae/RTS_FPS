using UnityEngine;
using System.Collections;

public class Laser : ProjectileController
{
    #region Variables
    //Laser Raycast Variables
    [SerializeField] private LayerMask layerMask;

    [SerializeField] private float laserDistance;

    //Laser Line Renderer Variables
    private Vector3[] lineRendererPositions = new Vector3[2];

    private LineRenderer lineRenderer;

    [SerializeField] private float minLaserWidth;
    [SerializeField] private float maxLaserWidth;
    [SerializeField] private float laserSizeExpansionRate;

    private float laserSizeTimer;

    //Particle Variables
    private new ParticleSystem particleSystem;

    private ParticleSystem.EmissionModule particleEmission;

    //External Objects
    [SerializeField] private string firingPointName = "Firing Point";

    private GameObject firingPoint;
    #endregion

    protected override void Start()
    {
        //Find game object
        firingPoint = GameObject.Find(firingPointName);

        //Get Component
        lineRenderer = GetComponent<LineRenderer>();
        particleSystem = GetComponent<ParticleSystem>();

        //Set particle emission module
        particleEmission = particleSystem.emission;
    }

    protected override void Update()
    {
        base.Update();

        UpdateTimers();
        AdjustLineRenderer();

        //Check if the left mouse button is yeilding an input, if not destroy game object
        if (!Input.GetMouseButton(0))
        {
            Destroy(gameObject);
        }
    }

    void AdjustLineRenderer()
    {
        RaycastHit hitInfo;

        //Set line renderer positions
        lineRendererPositions[0] = firingPoint.transform.position;
        lineRendererPositions[1] = firingPoint.transform.position + firingPoint.transform.forward * laserDistance;

        //Set particle emission to false
        particleEmission.enabled = false;

        //Check if laser hits anything, if the laser hits anything change line render position
        if (Physics.Raycast(firingPoint.transform.position, firingPoint.transform.forward, out hitInfo, laserDistance, layerMask))
        {
            lineRendererPositions[1] = hitInfo.point;

            print(hitInfo.transform.gameObject.name);

            //Set particle emission to true
            particleEmission.enabled = true;
        }

        //Assign line renderer the array of stored positions
        lineRenderer.SetPositions(lineRendererPositions);

        transform.position = lineRendererPositions[1];
        transform.rotation = firingPoint.transform.rotation;

        //Calculate laser width
        float width = Mathf.Lerp(minLaserWidth, maxLaserWidth, laserSizeTimer / laserSizeExpansionRate);

        //Set laser width
        lineRenderer.SetWidth(width, width);
    }

    void UpdateTimers()
    {
        //Update Timer
        laserSizeTimer = Mathf.Clamp(laserSizeTimer + Time.deltaTime, 0, laserSizeExpansionRate);
    }
}
