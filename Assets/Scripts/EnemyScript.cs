using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       //Physics.Raycast() 
    }

    void FixedUpdate()
    {
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
        }
    }

    bool TryFindPlayer()
    {
        return false; // TODO
    }
}
