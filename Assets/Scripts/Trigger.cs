using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : Interactable
{
    public Triggable target;
    public float seconds;
    private WaitForSeconds wait;

    public override bool IsValid { get; protected set; } = true;

    private void Start()
    {
        wait = new WaitForSeconds(seconds);
    }

    public override void Interact()
    {
        if (!IsValid)
            return;
        IsValid = false;
        target.Trigger();
        StartCoroutine(waiter());
    }

    IEnumerator waiter()
    {
        // TODO trigger sound/animation
        yield return wait;
        IsValid = true;
    }

    protected void OnDrawGizmos()
    {
        if (target)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, target.transform.position);
        }
    }
}

public abstract class Triggable : MonoBehaviour
{
    public abstract void Trigger();
}