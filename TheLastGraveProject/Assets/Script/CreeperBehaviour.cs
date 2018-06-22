using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreeperBehaviour : MonoBehaviour
{
    public enum State {Idle, Patrol, Chase, Explosion, Dead};
    public State state;

    private Animator anim;
    private NavMeshAgent agent;
    private SoundPlayer sound;

    private float timeCounter;
    public float idleTime = 1;

    [Header("Path properties")]
    public Transform[] nodes;
    public int curentNode;
    public float minDistance = 0.1f;
    public bool stopAtEachNode = false;

    [Header("Target properties")]
    public float radius;
    public LayerMask targetMask;
    public bool targetDetected;
    public Transform targetTransform;

    [Header("Explosion properties")]
    public float explosionDistance = 2.5f;
    public float explosionRadius;
    public float explosionForce;
    public ParticleSystem ExplosionSP;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        sound = GetComponentInChildren<SoundPlayer>();
        agent = GetComponent<NavMeshAgent>();

        GoToNearNode();
        SetIdle();
    }

    private void Update()
    {
        switch(state)
        {
            case State.Idle:
                IdleUpdate();
                break;
            case State.Patrol:
                PatrolUpdate();
                break;
            case State.Chase:
                ChaseUpdate();
                break;
            default:
                break;
        }
    }

    private void FixedUpdate()
    {
        targetDetected = false;
        Collider[] hitCollider = Physics.OverlapSphere(transform.position, radius, targetMask);
        if(hitCollider.Length != 0)
        {
            //ha detectado un target
            targetDetected = true;
            targetTransform = hitCollider[0].transform;
        }
    }

    void IdleUpdate()
    {
        //primera condicion patrol
        if(timeCounter >= idleTime) SetPatrol();
        else timeCounter += Time.deltaTime;

        //segunda condicion target
        if(targetDetected) SetChase();
    }


    void PatrolUpdate()
    {
        //Si hay target SetChase
        if(targetDetected) SetChase();
        //Si se para en cada punto Idle else siguiente punto
        if(Vector3.Distance(transform.position, nodes[curentNode].position) < minDistance)
        {
            GoToNextNode();
            if(stopAtEachNode) SetIdle();
        }
    }
    void ChaseUpdate()
    {
        //Explosion Cuando la distancia sea menor que X
        if(Vector3.Distance(transform.position, targetTransform.position) <= explosionDistance) SetExplosion();
        //Idle Cuando salgamos del overlap
        if(!targetDetected)
        {
            GoToNearNode();
            SetIdle();
            return;
        }
        //Chase: perseguir al target
        agent.SetDestination(targetTransform.position);
    }

    #region Sets
    void SetIdle()
    {
        //sound.Play(Random.Range(1,5), 1);
        timeCounter = 0;
        anim.SetBool("isMoving", false);
        agent.isStopped = true;
        agent.stoppingDistance = 0;
        radius = 5;
        state = State.Idle;
    }
    void SetPatrol()
    {
        anim.SetBool("isMoving", true);
        agent.isStopped = false;
        //Darle destino

        state = State.Patrol;
    }
    void SetChase()
    {
        anim.SetBool("isMoving", true);
        agent.isStopped = false;
        agent.stoppingDistance = 2;
        radius = 7;
        state = State.Chase;
    }
    void SetExplosion()
    {
        //sound.Play(9, 1);
        anim.SetTrigger("Explode");
        agent.isStopped = true;

        state = State.Explosion;
    }
    void SetDead()
    {
        Destroy(this.gameObject);
        state = State.Dead;
    }
    #endregion

    void GoToNearNode()
    {
        float minDist = Mathf.Infinity;
        for(int i = 0; i < nodes.Length; i++)
        {
            float dist = Vector3.Distance(transform.position, nodes[i].position);
            if(dist < minDist)
            {
                curentNode = i;
                minDist = dist;
            }
        }

        agent.SetDestination(nodes[curentNode].position);
    }

    void GoToNextNode()
    {
        //Close path
        curentNode++;
        if(curentNode >= nodes.Length) curentNode = 0;
        agent.SetDestination(nodes[curentNode].position);
    }

    public void Beam()
    {
        //VFX
        ExplosionSP.Play();
        ExplosionSP.transform.parent = null;
        //sound.Play(Random.Range(5, 9), 1);
        //

        Collider[] hitCollider = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach(Collider col in hitCollider)
        {
            if(col.attachedRigidbody != null)
            {
                col.attachedRigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius, 1.0f, ForceMode.Impulse);
            }
            if(col.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                Debug.Log("Muere viejo puto");
            }
        }
        SetDead();
    }

    private void OnDrawGizmos()
    {
        Color c = Color.red;
        c.a = 0.1f;
        Gizmos.color = c;
        Gizmos.DrawSphere(transform.position, radius);
    }


}
