using System.Collections;
using UnityEngine;
using UnityEngine.AI;

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

    private NavMeshAgent agent;
    private PlayerScript player;
    private Camera playerCamera;
    private Animator animator;
    private SpriteRenderer alertSprite;
    private EnemyState state = EnemyState.idle;
    private int playerNoticing = 0;

    void Start()
    {

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerScript>();
        playerCamera = GameObject.FindWithTag("Player").GetComponentInChildren<Camera>();

        var alert = new GameObject("MinimapSprite");
        alert.transform.parent = gameObject.transform;
        alert.transform.localScale = Vector3.one * 0.3f;
        alert.layer = 11;

        alertSprite = alert.AddComponent<SpriteRenderer>();
        alertSprite.transform.localPosition = Vector3.up * 50f;
        alertSprite.sprite = player.enemySprite;

        GotoNextPoint();

        animator.Play("Z_Run_InPlace");
    }

    void Update()
    {
        var alertEulerRotation = Vector3.zero;
        alertEulerRotation.y = player.transform.rotation.eulerAngles.y;
        alertEulerRotation.x = 90;

        alertSprite.transform.eulerAngles = alertEulerRotation;
    }

    void FixedUpdate()
    {
        if (state == EnemyState.dead) return;

        if (TryFindPlayer())
        {
            Debug.Log(gameObject.name + " CAN SEE PLAYER - " + playerNoticing);
            playerNoticing = Mathf.Clamp(playerNoticing + 4, 0, playerNoticeTreshold);
        }
        else
        {
            playerNoticing = Mathf.Clamp(playerNoticing - 8, 0, playerNoticeTreshold);
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

        if (Physics.Raycast(transform.position + Vector3.up * 2f, (playerCamera.transform.position - Vector3.up * 3) - transform.position, out RaycastHit hit, detectDistance, mask))
        {
            if (hit.collider.gameObject.layer == 8)
            {
                return true;
            }
        }

        return false;
    }


    void GotoNextPoint()
    {
        if (points.Length == 0)
            return;

        agent.destination = points[Random.Range(0, points.Length - 1)].position;
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
        alertSprite.gameObject.SetActive(false);
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);
    }
}