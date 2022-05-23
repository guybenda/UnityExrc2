using UnityEngine;

using UnityEngine.AI;
using System.Collections;

enum EnemyState
{
    idle,
    chasing,
    attacking
}

public class EnemyScript : MonoBehaviour
{
    EnemyState state = EnemyState.idle;
    int playerNoticing = 0;
    bool dead = false;

    public int health { get; private set; } = 100;
    public int playerNoticeTreshold = 60;
    public float detectDistance = 100;

    public Transform[] points;

    private int destPoint = 0;
    private NavMeshAgent agent;
    private PlayerScript player;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerScript>();

        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        agent.autoBraking = false;

        GotoNextPoint();

        animator.Play("Z_Run_InPlace");
    }

    // Update is called once per frame
    void Update()
    {
        //Physics.Raycast() 
        // Choose the next destination point when the agent gets
        // close to the current one.
        if (state == EnemyState.idle && !agent.pathPending && agent.remainingDistance < 0.5f)
            GotoNextPoint();
    }

    void FixedUpdate()
    {
        if (dead) return;

        if (TryFindPlayer())
        {
            playerNoticing = Mathf.Clamp(playerNoticing + 1, 0, playerNoticeTreshold);
        }

        switch (state)
        {
            case EnemyState.idle:
                if (playerNoticing >= playerNoticeTreshold)
                {
                    state = EnemyState.chasing;
                    agent.autoBraking = true;
                }
                break;
            case EnemyState.chasing:
                if (playerNoticing == 0)
                {
                    state = EnemyState.idle;
                    agent.autoBraking = false;
                    GotoNextPoint();
                }
                break;
            case EnemyState.attacking:
                //TODO
                break;
        }
    }

    bool TryFindPlayer()
    {
        const int mask = ~(1 << 9);

        if (Physics.Raycast(transform.position, player.transform.position - transform.position, out RaycastHit hit, detectDistance, mask))
        {
            if (hit.collider.gameObject.layer == 9)
            {
                return true;
            }
        }

        return false; // TODO
    }


    void GotoNextPoint()
    {
        if (points.Length == 0)
            return;

        agent.destination = points[destPoint].position;

        destPoint = Random.Range(0, points.Length - 1);
    }

    public bool Damage(int damage)
    {
        if (dead)
            return false;

        health -= damage;

        if (health <= 0)
        {
            Kill();
            return true;
        }

        return false;
    }

    void Kill()
    {
        dead = true;
        StartCoroutine(DieRoutine());
    }

    IEnumerator DieRoutine()
    {
        animator.Play("Z_FallingBack");
        yield return new WaitForSeconds(1.4f);
        gameObject.SetActive(false);
    }
}