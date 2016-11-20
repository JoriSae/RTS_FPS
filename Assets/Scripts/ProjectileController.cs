using UnityEngine;
using System.Collections;

public class ProjectileController : MonoBehaviour {

    public float bulletSpeed;
    public float duration;

    void Start()
    {
        Destroy(gameObject, duration);
    }

    void Update()
    {
        transform.position = transform.position + transform.forward * Time.deltaTime * bulletSpeed;
    }
}
