using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractSpawner : Triggable
{
    public Poolable prefab;
}

public class Spawner : AbstractSpawner
{
    public override void Trigger()
    {
        // TODO spawner sound/particle
        var result = prefab.Get(transform.position, transform.rotation);
    }
}