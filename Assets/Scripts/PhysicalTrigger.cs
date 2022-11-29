using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalTrigger : Trigger
{
    const float THRESHHOLD = 10;

    private void OnCollisionEnter(Collision collision)
    {
        if (Vector3.Dot(transform.forward, collision.impulse) > THRESHHOLD)
            Interact();
    }

    new protected void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward);
    }
}