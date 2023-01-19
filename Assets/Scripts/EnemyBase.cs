using UnityEngine;


public class EnemyBase : MonoBehaviour
{
    [SerializeField] protected EnemyStates defaultState;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected int healthPoints;
    [SerializeField] protected Animator enemyAnimator;
    [SerializeField] protected GameObject player;

    private EnemyStates _currentState;

    private void Start()
    {
        _currentState = defaultState;
    }

    private void Update()
    {
        HighLevelStateMachine();
    }

    private void HighLevelStateMachine()
    {
        switch (_currentState)
        {
            case EnemyStates.Idle:
                EnemyIdle();
                break;
            case EnemyStates.Moving:
                EnemyMove();
                break;
            case EnemyStates.Hit: break;
            case EnemyStates.Dead:
                EnemyDead(); break;
        }
    }

    private void EnemyDead()
    {
        
    }

    protected virtual void EnemyIdle()
    {
    }

    protected virtual void EnemyMove()
    {
    }

    protected void UpdateCurrentState(EnemyStates enemyState)
    {
        _currentState = enemyState;
    }
}