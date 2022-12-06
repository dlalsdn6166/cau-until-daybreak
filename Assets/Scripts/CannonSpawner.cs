using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonSpawner : AbstractSpawner
{
    public float force = 30;
    public bool inheritRotation = false;

    public override void Trigger()
    {
        // TODO spawner sound/particle
        var result = prefab.Get(transform.position, inheritRotation ? transform.rotation : Quaternion.identity);
        result.Rigidbody?.AddForce(transform.forward * force, ForceMode.VelocityChange);
        effect?.Play(transform);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward);
    }
}