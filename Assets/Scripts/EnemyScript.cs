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
    public int playerNoticeTreshold = 60;

    public Transform[] points;

    private int destPoint = 0;
    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {

        agent = GetComponent<NavMeshAgent>();

        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        agent.autoBraking = false;

        GotoNextPoint();
    }

    // Update is called once per frame
    void Update()
    {
        //Physics.Raycast() 
        // Choose the next destination point when the agent gets
        // close to the current one.
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GotoNextPoint();
    }

    void FixedUpdate()
    {/*
        switch (state)
        {
            case EnemyState.idle:
                TryFindPlayer();
                //TODO
                break;
            case EnemyState.chasing:
                //TODO
                break;
            case EnemyState.attacking:
                //TODO
                break;
        }*/
    }

    bool TryFindPlayer()
    {
        return false; // TODO
    }


    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (points.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        agent.destination = points[destPoint].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % points.Length;
    }
}