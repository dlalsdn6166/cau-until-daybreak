using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Draggable : Damagable
{
    public const float Threshhold = 10;

    public Collider Collider { get; private set; }
    new private Renderer renderer;
    private static Material outline;
    private bool highlighted = false;
    private List<Material> materials = new List<Material>();



    protected static int playerMask;

    protected void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Collider = GetComponent<Collider>();
        renderer = GetComponent<Renderer>();
        if (outline == null)
            outline = Resources.Load<Material>("Outline");

        playerMask = LayerMask.NameToLayer("Player");
    }

    public void Highlight(bool value)
    {
        if (value == highlighted)
            return;
        highlighted = value;
        materials.Clear();
        materials.AddRange(renderer.sharedMaterials);
        if (highlighted)
            materials.Add(outline);
        else
            materials.Remove(outline);
        renderer.sharedMaterials = materials.ToArray();
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == playerMask)
            return;

        Rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;

        if (collision != null)
        {
            if (collision.impulse.magnitude < Threshhold * Rigidbody.mass)
                return;
            Damage(collision.impulse.magnitude);
        }
    }
}

public abstract class Damagable : Poolable
{
    public float hp;
    protected float current;

    public AudioClip collisionClip;
    public AudioSource audioSource;

    public Effect effects;

    protected void OnEnable() => Init();

    public virtual void Damage(float damage)
    {
        if (current <= 0)
            return;
        current -= damage;
        if (current <= 0)
        {
            Dead();
            return;
        }
        if (collisionClip)
            audioSource.PlayOneShot(collisionClip, Mathf.Max(1, damage / 15));
    }
    protected virtual void Dead()
    {
        if (effects)
            effects.Play(transform);
        Enqueue();
    }

    protected override void Init() => current = hp;
}

public abstract class Poolable : MonoBehaviour
{
    private Queue<Poolable> pool = new Queue<Poolable>();
    public Rigidbody Rigidbody;
    const int max = 20;

    private void Reset()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    public Poolable Get(Vector3 position, Quaternion rotation)
    {
        Poolable result;
        if (pool.Count > 0)
        {
            result = pool.Dequeue();
            result.transform.position = position;
            result.transform.rotation = rotation;
        }
        else
        {
            result = Instantiate(this, position, rotation);
            result.pool = pool;
        }
        result.gameObject.SetActive(true);
        return result;
    }

    protected abstract void Init();

    protected void Enqueue()
    {
        if (pool.Count < max)
        {
            pool.Enqueue(this);
            gameObject.transform.parent = null;
            gameObject.SetActive(false);
            return;
        }
        Destroy(gameObject);
    }
}