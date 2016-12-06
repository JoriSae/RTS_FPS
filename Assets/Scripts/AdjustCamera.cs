using UnityEngine;
using System.Collections;

public class AdjustCamera : MonoBehaviour
{
    #region Variables
    public enum Projection
    {
        Perspective,
        Orthographic
    }

    public Projection projection;

    public enum ClearFlags
    {
        SoildColour,
        SkyBox
    }

    public ClearFlags clearFlags;

    public Color cameraBackgroundColour;

    [SerializeField] private Vector3 cameraPosition;
    [SerializeField] private Quaternion cameraRotation;

    [SerializeField] private float orthographicCameraSize;
    #endregion

    void Start()
    {
        Camera.main.transform.position = cameraPosition;
        Camera.main.transform.rotation = cameraRotation;

        Camera.main.backgroundColor = cameraBackgroundColour;

        switch (clearFlags)
        {
            case ClearFlags.SoildColour:
                Camera.main.clearFlags = CameraClearFlags.SolidColor;
                break;
            case ClearFlags.SkyBox:
                Camera.main.clearFlags = CameraClearFlags.Skybox;
                break;
        }

        switch (projection)
        {
            case Projection.Perspective:
                Camera.main.orthographic = false;
                break;
            case Projection.Orthographic:
                Camera.main.orthographic = true;
                Camera.main.orthographicSize = orthographicCameraSize;
                break;
        }
	}
}
