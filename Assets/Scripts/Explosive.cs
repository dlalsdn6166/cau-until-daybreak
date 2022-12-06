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

    private int lm;

    new protected void Awake()
    {
        base.Awake();
        lm = LayerMask.NameToLayer("Default");
    }

    protected override void Dead()
    {
        var center = transform.position;
        var colliders = Physics.OverlapSphere(center, radius, lm);

        Rigidbody rigidbody;
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject == gameObject)
                continue;
            colliders[i].GetComponent<Damagable>()?.Damage(force * 10);
            
            rigidbody = colliders[i].attachedRigidbody;
            if (rigidbody)
            {
                rigidbody.AddExplosionForce(force, center, radius, 1, ForceMode.Impulse);
            }
        }

        var z = FindObjectsOfType<Zombie>();
        for (int i = 0; i < z.Length; i++)
        {
            var s = z[i].transform.position - center;
            if (s.magnitude > radius)
                continue;
            z[i].Damage(force * 10);
            z[i].AddForce(s.normalized * force);
        }

        base.Dead();
    }
}