using UnityEngine;
using System.Collections;

public class ProjectileController : MonoBehaviour
{
    #region Variables
    public float fireRate;

    [SerializeField] protected float bulletSpeed;
    [SerializeField] private float duration;
    #endregion

    protected virtual void Start()
    {
        //Destroy projectile after set duration
        Destroy(gameObject, duration);
    }

    protected virtual void Update()
    {

    }
}
