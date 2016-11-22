using UnityEngine;
using System.Collections;

public class ProjectileController : MonoBehaviour
{
    #region Variables
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float duration;
    #endregion

    void Start()
    {
        //Destroy projectile after set duration
        Destroy(gameObject, duration);
    }

    void Update()
    {
        //translate bullet position forwards
        transform.position = transform.position + transform.forward * Time.deltaTime * bulletSpeed;
    }
}
