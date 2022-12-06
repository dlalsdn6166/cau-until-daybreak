using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Camera playerCamera;
    public Transform hold;

    public float dragForce = 20;
    public float fireForce = 20;
    public float dragRange = 20;
    public float holdRange = 2;

    private Draggable draggable;
    private bool dragging = false;

    private int layer;
    private void Start()
    {
        layer = LayerMask.NameToLayer("Default");
    }
    private void Update()
    {
        if (dragging)
        {
            // out of sight
            draggable.transform.SetParent(null);
            draggable.gameObject.layer = layer;
            if (Vector3.Dot(playerCamera.transform.forward, draggable.transform.position - playerCamera.transform.position) < 0 || !Input.GetMouseButton(1))
            {
                draggable = null;
                dragging = false;
            }
            else if (Input.GetMouseButtonDown(0))
            {
                // TODO gravity-gun FIRE sound/particle
                dragging = false;
                draggable.Rigidbody.AddForce(playerCamera.transform.forward * fireForce, ForceMode.Impulse);
                draggable = null;
            }
        }
        else
        {
            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
            var before = draggable;
            Physics.Raycast(ray, out RaycastHit hit, dragRange);
            draggable = hit.collider?.GetComponent<Draggable>();
            if (before && before != draggable)
                before?.Highlight(false);
            if (draggable)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    // TODO gravity-gun DRAG sound/particle
                    draggable.transform.SetParent(null);
                    draggable.gameObject.layer = layer;
                    draggable.Highlight(false);
                    dragging = true;
                }
                else if (before != draggable)
                    draggable.Highlight(true);
            }
            else
                dragging = false;
        }
    }

    private void FixedUpdate()
    {
        if (dragging)
        {
            Debug.Assert(draggable);
            draggable.Rigidbody.isKinematic = false;
            draggable.Rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;

            var delta = hold.position - draggable.transform.position;
            var dist = delta.magnitude;
            if (dist < holdRange)
            {
                draggable.Rigidbody.velocity = Vector3.Lerp(draggable.Rigidbody.velocity, Vector3.ClampMagnitude(delta / Time.fixedDeltaTime, dragRange), Time.fixedDeltaTime * 10);
                draggable.Rigidbody.angularVelocity = Vector3.zero;
                draggable.Rigidbody.MoveRotation(Quaternion.Lerp(draggable.transform.rotation, playerCamera.transform.rotation, Time.fixedDeltaTime * 10));
            }
            else
            {
                draggable.Rigidbody.AddForce(delta.normalized * 10 * dragForce * Time.fixedDeltaTime, ForceMode.VelocityChange);
            }
        }
    }
}