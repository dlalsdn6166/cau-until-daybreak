using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Trigger : Interactable
{
    public Triggable target;
    public float seconds;
    private WaitForSeconds wait;
    private AudioSource audioSource;
    public AudioClip click;
    public AudioClip note;

    public override bool IsValid { get; protected set; } = true;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        wait = new WaitForSeconds(seconds);
    }

    public override void Interact()
    {
        if (!IsValid)
            return;
        IsValid = false;
        audioSource.PlayOneShot(click);
        target.Trigger();
        StartCoroutine(waiter());
    }

    IEnumerator waiter()
    {
        // TODO trigger sound/animation
        yield return wait;
        audioSource.PlayOneShot(note);
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