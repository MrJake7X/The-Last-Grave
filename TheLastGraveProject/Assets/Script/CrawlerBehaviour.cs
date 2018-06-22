using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CrawlerBehaviour : MonoBehaviour {

    public enum State {Eat, Idle, Patrol, Chase, Attack, Dead };
    public State state;

    //private Animator anim;
    private NavMeshAgent agent;
    private Animator anim;
    private SoundPlayer sound;

    private float timeCounter;
    public float idleTime = 1;

    [Header("Eat Properties")]
    public float eatCounter;

    [Header("Path Properties")]
    public Transform[] nodes;
    public int curentNode;
    public float minDistance = 0.1f;
    public bool stopAtEachNode = false;

    [Header("Target Properties")]
    public float radius;
    public LayerMask targetMask;
    public bool targetDetected;
    public Transform targetTransform;
    
    [Header("Attack Properties")]
    public float attackDistance = 2.5f;
    public float attackRadius;
    public int damage;
    //public float explosionForce;

    [Header("Life Properties")]
    public int life;
    private Collider col;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        sound = GetComponentInChildren<SoundPlayer>();
        agent = GetComponent<NavMeshAgent>();
        col = GetComponent<Collider>();

        //GoToNearNode();
        SetEat();
    }

    private void Update()
    {
        switch(state)
        {
            case State.Eat:
                EatUpdate();
                break;
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

    void EatUpdate()
    {
        eatCounter += 1 * Time.deltaTime;
        if(eatCounter >= 5)
        {
            GoToNearNode();
            SetIdle();
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
        if(Vector3.Distance(transform.position, targetTransform.position) <= attackDistance) Attack();
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
    void SetEat()
    {
        sound.Play(1, 1); 
        eatCounter = 0;
    }
    void SetIdle()
    {
        anim.SetTrigger("Go");
        timeCounter = 0;
        anim.SetBool("isMoving", false); //Animacion Walk False
        anim.SetBool("isChasing", false); //Animacion Run False
        agent.isStopped = true;
        agent.stoppingDistance = 0;
        radius = 5;
        state = State.Idle;
    }
    void SetPatrol()
    {
        anim.SetBool("isMoving", true); //Animacion Walk True
        anim.SetBool("isChasing", false); //Animacion Run False
        agent.isStopped = false;
        agent.speed = 2;
        //Darle destino

        state = State.Patrol;
    }
    void SetChase()
    {
        anim.SetBool("isChasing", true); //Animacion Run True
        anim.SetBool("isMoving", false); //Animacion Walk False
        agent.isStopped = false;
        agent.stoppingDistance = 2;
        radius = 6;
        agent.speed = 5;
        state = State.Chase;
    }
    void SetAttack()
    {
        //sound.Play(9, 1); Sonido Ataque
        //anim.SetTrigger("Attack"); //Animacion Attack
        anim.SetBool("isChasing", false);
        anim.SetBool("isMoving", false);
        agent.isStopped = true;
        state = State.Attack;
    }
    void SetDead()
    {
        state = State.Dead;
        anim.SetTrigger("Die");
        /*
        Destroy(this.gameObject);
        */
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
    
    public void Attack()
    {
        //VFX
        sound.Play(2, 1);
        //
        bool hasAttacked = false;
        Collider[] hitCollider = Physics.OverlapSphere(transform.position, attackRadius);
        foreach(Collider col in hitCollider)
        {
            if (!hasAttacked)
            {
                if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    PlayerController player = col.gameObject.GetComponentInParent<PlayerController>();
                    Debug.Log("Atacao");
                    player.Dmg(damage);
                    hasAttacked = true;
                }
            }
        }
        SetIdle();
    }
    
    public void Dmg(int dmg)
    {
        life -= dmg;
        sound.Play(0, 1); 
        if (life <= 0)
        {
            col.enabled = false;
            SetDead();
        }
        Debug.Log("Enemy" + life);
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
