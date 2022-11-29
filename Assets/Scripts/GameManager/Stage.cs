using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public float interval;
    public Triggable[] triggers;

    public void Trigger()
    {
        // TODO new stage
        for (int i = 0; i < triggers.Length; i++)
            triggers[i]?.Trigger();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        var b = transform.position;
        if (triggers != null)
            for (int i = 0; i < triggers.Length; i++)
                if (triggers[i])
                {
                    Gizmos.DrawLine(b, triggers[i].transform.position);
                    b = triggers[i].transform.position;
                }
    }
}