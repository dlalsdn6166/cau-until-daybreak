using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Effect : MonoBehaviour
{
    private Queue<Effect> pool = new Queue<Effect>();
    private ParticleSystem particle;
    private AudioSource audioSource;
    const int max = 10;
    
    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
        var main = particle.main;
        main.stopAction = ParticleSystemStopAction.Callback;
    }

    public void Play(Transform tr)
    {
        Effect result;
        if (pool.Count > 0)
        {
            result = pool.Dequeue();
            result.transform.position = tr.position;
            result.transform.rotation = tr.rotation;
        }
        else
        {
            result = Instantiate(this, tr.position, tr.rotation);
            result.pool = pool;
        }
        result.gameObject.SetActive(true);
        result.particle.Play();
        result.audioSource.Play();
    }

    private void OnParticleSystemStopped()
    {
        if (pool.Count < max)
        {
            pool.Enqueue(this);
            gameObject.SetActive(false);
            return;
        }
        Destroy(gameObject);
    }
}
