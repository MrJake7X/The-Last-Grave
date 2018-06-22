using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CrawlerAngel : MonoBehaviour {

    public enum State { Idle, Patrol, Chase};
    public State state;

    //private Animator anim;
    private NavMeshAgent agent;
    private Animator anim;
    private SoundPlayer sound;

    private float timeCounter;
    public float idleTime = 1;
    public bool detected;

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
    
    [Header("Attack properties")]
    public float attackDistance = 2.5f;
    public float attackRadius;
    //public float explosionForce;
    
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
        if (detected)
        {
            SetIdle();
            return;
        }
        //segunda condicion target
        if(targetDetected) SetChase();
    }


    void PatrolUpdate()
    {
        if (detected)
        {
            SetIdle();
            return;
        }
        //Si se para en cada punto Idle else siguiente punto
        if (Vector3.Distance(transform.position, nodes[curentNode].position) < minDistance)
        {
            Debug.Log("E llegao");
            SetIdle();
        }
    }
    void ChaseUpdate()
    {
        if (detected)
        {
            SetIdle();
            return;
        }
        //Explosion Cuando la distancia sea menor que X
        if(Vector3.Distance(transform.position, targetTransform.position) <= attackDistance) SetAttack();
        //Idle Cuando salgamos del overlap
        if(!targetDetected)
        {
            GoToNearNode();
            SetPatrol();
            return;
        }
        Debug.Log("Xq no se mueve?");
        //Chase: perseguir al target
        agent.SetDestination(targetTransform.position);
    }

    #region Sets
    void SetIdle()
    {
        //sound.Play(Random.Range(1, 5), 1); Sonidos Idle
       // anim.Play("idle");
        timeCounter = 0;
        //anim.SetBool("isMoving", false); //Animacion Caminar False
        agent.isStopped = true;
        agent.stoppingDistance = 0;
        radius = 6;
        state = State.Idle;
    }
    void SetPatrol()
    {
        //anim.SetBool("isMoving", true); //Animacion Caminar False
        agent.isStopped = false;
        //Darle destino

        state = State.Patrol;
    }
    void SetChase()
    {
        //anim.SetBool("isMoving", true); //Animacion Caminar True
        agent.isStopped = false;
        agent.stoppingDistance = 2;
        radius = 15;
        state = State.Chase;
    }
    void SetAttack()
    {
        //sound.Play(9, 1); Sonido Ataque
        //anim.SetTrigger("Explode"); //Animacion Ataque
        agent.isStopped = true;

    }
    void SetDead()
    {
        Destroy(this.gameObject);

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

    
    public void Attack()
    {
        //VFX
        //sound.Play(Random.Range(5, 9), 1); Sonido Explosion
        //

        Collider[] hitCollider = Physics.OverlapSphere(transform.position, attackRadius);
        foreach(Collider col in hitCollider)
        {
            if(col.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                Debug.Log("Atacao");
            }
        }
        SetChase();
    }

    public void SetDetected(bool estado)
    {
        detected = estado;
    }
    
    private void OnDrawGizmos()
    {
        // GIZMO DETECTAR PLAYER
        Color c = Color.red;
        c.a = 0.1f;
        Gizmos.color = c;
        Gizmos.DrawSphere(transform.position, radius);

        // GIZMO ATTACK
        Color a = Color.yellow;
        a.a = 0.1f;
        Gizmos.color = a;
        Gizmos.DrawSphere(transform.position, attackRadius);
    }
}
