using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollChild : MonoBehaviour
{
    public Zombie Parent { get; set; }
    public Rigidbody Rigidbody;

    private int damageMask;
    private int playerMask;
    private void Awake()
    {
        damageMask = LayerMask.NameToLayer("Default");
        playerMask = LayerMask.NameToLayer("Player");
    }

    private void Reset()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    public const float Threshhold = 10;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != damageMask)
            return;
        if (collision.gameObject.layer == playerMask)
        {
            //collision.gameObject.
            return;
        }
        if (collision.impulse.magnitude < Threshhold)
            return;
        if (Rigidbody.isKinematic)
        {
            Parent.Damage(collision.impulse.magnitude);
            Rigidbody.AddForceAtPosition(collision.impulse, collision.GetContact(0).point);
        }
        else
            Parent.Damage(collision.impulse.magnitude);
    }
}