using UnityEngine;
using System.Collections;

public class Laser : ProjectileController
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        //translate bullet position forwards
        transform.position = transform.position + transform.forward * Time.deltaTime * bulletSpeed;
    }
}
