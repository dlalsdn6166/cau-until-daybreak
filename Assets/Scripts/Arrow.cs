using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Draggable
{

    protected override void OnCollisionEnter(Collision collision)
    {


        // TODO collision on instantiate
        base.OnCollisionEnter(collision);



        if (Vector3.Dot(transform.forward, Rigidbody.velocity) < 0)
            return;
        if (collision.impulse.magnitude < 1)
            return;
        transform.parent = collision.transform;
        gameObject.layer = LayerMask.NameToLayer("Through");
        Rigidbody.isKinematic = true;
    }
}