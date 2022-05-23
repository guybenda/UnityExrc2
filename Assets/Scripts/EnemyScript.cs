using UnityEngine;

using UnityEngine.AI;
using System.Collections;

enum EnemyState
{
    idle,
    chasing,
    dead
}

public class EnemyScript : MonoBehaviour
{

    public int health { get; private set; } = 100;
    public int playerNoticeTreshold = 120;
    public float detectDistance = 30;

    public Transform[] points;

    private int destPoint = 0;
    private NavMeshAgent agent;
    private PlayerScript player;
    private Camera playerCamera;
    private Animator animator;
    private EnemyState state = EnemyState.idle;
    private int playerNoticing = 0;

    // Start is called before the first frame update
    void Start()
    {

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerScript>();
        playerCamera = GameObject.FindWithTag("Player").GetComponentInChildren<Camera>();


        GotoNextPoint();

        animator.Play("Z_Run_InPlace");
    }

    void Update()
    {
        //Physics.Raycast() 
    }

    void FixedUpdate()
    {
        if (state == EnemyState.dead) return;

        if (TryFindPlayer())
        {
            Debug.Log(gameObject.name + " CAN SEE PLAYER - " + playerNoticing);
            playerNoticing = Mathf.Clamp(playerNoticing + 1, 0, playerNoticeTreshold);
        }
        else
        {
            playerNoticing = Mathf.Clamp(playerNoticing - 2, 0, playerNoticeTreshold);
        }

        switch (state)
        {
            case EnemyState.idle:
                if (playerNoticing >= playerNoticeTreshold)
                {
                    state = EnemyState.chasing;
                    agent.autoBraking = true;
                }
                else if (!agent.pathPending && agent.remainingDistance < 0.5f)
                {
                    GotoNextPoint();
                }

                break;
            case EnemyState.chasing:
                agent.destination = player.transform.position;

                if ((player.transform.position - transform.position).magnitude < 2f)
                {
                    player.TakeDamage(1);
                }

                if (playerNoticing == 0)
                {
                    state = EnemyState.idle;
                    agent.autoBraking = false;
                    GotoNextPoint();
                }
                break;
        }
    }

    bool TryFindPlayer()
    {
        const int mask = ~(1 << 9);

        //Debug.DrawRay(transform.position + Vector3.up * 2f, ((playerCamera.transform.position - Vector3.up * 3) - transform.position) * detectDistance, Color.yellow, 0.1f);

        if (Physics.Raycast(transform.position + Vector3.up * 2f, (playerCamera.transform.position - Vector3.up * 3) - transform.position, out RaycastHit hit, detectDistance, mask))
        {
            if (hit.collider.gameObject.layer == 8)
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
        if (state == EnemyState.dead)
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
        state = EnemyState.dead;
        StartCoroutine(DieRoutine());
    }

    IEnumerator DieRoutine()
    {
        animator.Play("Z_FallingBack");
        agent.isStopped = true;
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);
    }
}