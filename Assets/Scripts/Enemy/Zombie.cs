using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Zombie : Damagable
{
    public NavMeshAgent Agent { get; private set; }
    public Animator Animator { get; private set; }
    public AudioClip deadClip;

    public Transform head;
    private RagdollChild[] ragdoll;
    public float range;

    private void SimulateRagdoll(bool value)
    {
        value = !value;
        Agent.enabled = value;
        Animator.enabled = value;
        for (int i = 0; i < ragdoll.Length; i++)
            ragdoll[i].Rigidbody.isKinematic = value;
    }

    protected State state;
    protected enum State
    {
        Down,
        Up,
        Follow,
        Attack,
        Dead
    }

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();
        ragdoll = GetComponentsInChildren<RagdollChild>();
        for (int i = 0; i < ragdoll.Length; i++)
            ragdoll[i].Parent = this;
    }

    private int worldMask;
    private void Start()
    {
        worldMask = LayerMask.GetMask("World");
    }

    public static int Count { get; set; }
    public static int KillScore { get; set; }

    protected override void Init()
    {
        base.Init();
        StateChange(State.Down);
        StartCoroutine(Act());
        Count++;
    }

    private float until;
    private IEnumerator Act()
    {
        while (true)
        {
            yield return null;
            switch (state)
            {
                case State.Down:
                    if (until < Time.time)
                        StateChange(State.Up);
                    break;
                case State.Up:
                    // wait animation
                    if (true)
                        StateChange(State.Follow);
                    break;
                case State.Follow:
                    var player = GameManager.Instance.Player;
                    if (until < Time.time)
                        if (Vector3.Distance(player.playerCamera.transform.position, head.position) < range)
                            if (!Physics.Raycast(head.position, player.playerCamera.transform.position - head.position, range, worldMask))
                            {
                                StateChange(State.Attack);
                                continue;
                            }
                    if (Agent.isOnNavMesh)
                    {
                        Agent.SetDestination(player.transform.position);
                        Animator.SetFloat("move", Agent.velocity.magnitude);
                    }
                    break;
                case State.Attack:
                    if (until < Time.time)
                        StateChange(State.Up);
                    break;
                case State.Dead:
                    if (until < Time.time)
                        Dead();
                    break;
            }
        }
    }

    protected virtual void StateChange(State state)
    {
        switch (state)
        {
            case State.Down:
                until = Time.time + 3;
                SimulateRagdoll(true);
                break;
            case State.Up:
                // TODO stand-up animation?
                transform.position = Rigidbody.position;
                SimulateRagdoll(false);
                break;
            case State.Follow:
                break;
            case State.Attack:
                SimulateRagdoll(true);
                var dist = GameManager.Instance.Player.transform.position - transform.position;
                transform.forward = dist;
                var velocity = dist.normalized * range - Physics.gravity * (dist.magnitude / range * 0.5f);
                Rigidbody.velocity = 60 / 9.375f * velocity;
                until = Time.time + 3;
                break;
            case State.Dead:
                SimulateRagdoll(true);
                Count--;
                KillScore++;
                until = Time.time + 5;
                break;

        }
        this.state = state;
    }

    public override void Damage(float damage)
    {
        if (state == State.Dead)
            return;

        current -= damage;
        audioSource.PlayOneShot(collisionClip, Mathf.Max(1, damage / 50));
        if (current <= 0)
        {
            audioSource.PlayOneShot(deadClip);
            StateChange(State.Dead);
        }
        else
        {
            if (!audioSource.isPlaying)
                audioSource.PlayOneShot(collisionClip, Mathf.Max(1, damage / 50));
            StateChange(State.Down);
        }
    }
}