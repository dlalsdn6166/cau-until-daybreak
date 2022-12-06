using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class AbstractSpawner : Triggable
{
    public Poolable prefab;
    protected AudioSource audioSource;
    public Effect effect;

    protected void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
}

public class Spawner : AbstractSpawner
{
    public override void Trigger()
    {
        var result = prefab.Get(transform.position, transform.rotation);
        effect?.Play(transform);
    }
}