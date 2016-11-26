using UnityEngine;
using System.Collections;

public class Laser : ProjectileController
{
    [SerializeField] private string firingPointName = "Firing Point";

    [SerializeField] private float laserDistance;

    [SerializeField] private LayerMask layerMask;

    private GameObject firingPoint;

    private LineRenderer lineRenderer;

    private Vector3[] lineRendererPositions = new Vector3[2];

    protected override void Start()
    {
        firingPoint = GameObject.Find(firingPointName);

        lineRenderer = GetComponent<LineRenderer>();
    }

    protected override void Update()
    {
        base.Update();

        RaycastHit hitInfo;

        lineRendererPositions[0] = firingPoint.transform.position;
        lineRendererPositions[1] = firingPoint.transform.position + firingPoint.transform.forward * laserDistance;

        if (Physics.Raycast(firingPoint.transform.position, firingPoint.transform.forward, out hitInfo, laserDistance, layerMask))
        {
            lineRendererPositions[1] = hitInfo.point;
        }

        lineRenderer.SetPositions(lineRendererPositions);

        Debug.DrawRay(firingPoint.transform.position, firingPoint.transform.forward);

        if (!Input.GetMouseButton(0))
        {
            Destroy(gameObject);
        }
    }
}
