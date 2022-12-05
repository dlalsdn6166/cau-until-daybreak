using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

[RequireComponent(typeof(Damagable))]
public class Explosive : Draggable
{
    public float radius = 5;
    public float force = 30;

    protected override void Dead()
    {
        var center = transform.position;
        var colliders = Physics.OverlapSphere(center, radius);

        Rigidbody rigidbody;
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i] == Collider)
                continue;
            colliders[i].GetComponent<Damagable>()?.Damage(force);
            rigidbody = colliders[i].attachedRigidbody;
            if (rigidbody)
            {
                rigidbody.AddExplosionForce(force, center, radius, 1, ForceMode.Impulse);
            }
        }
        base.Dead();
    }
}