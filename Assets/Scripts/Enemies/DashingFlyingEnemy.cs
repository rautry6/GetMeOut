using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashingFlyingEnemy : MonoBehaviour
{
    [SerializeField] private float idleTimer;
    [SerializeField] private float wanderSpeed;
    [SerializeField, Tooltip("Frequency of the sinusoidal wave")] private float wanderFrequency;
    [SerializeField, Tooltip("How high or low the sinusoid wave goes")] private float wanderMagnitude;
    [SerializeField] private float wanderTimer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float detectionRadius;
    [SerializeField] private float attackDashSpeed;
    [SerializeField] private float returnSpeed;
    [SerializeField] private float attackTimer;

    private float _currentAttackTimer;
    private float _currentWanderTimer;
    private float _currentIdleTimer;
    private float _sinWaveTimer;
    private Vector3 _startingPosition;
    private Vector3 _moveDirection;
    private DashingFlyingEnemyStates _currentState;
    private GameObject _player;
    private bool _returningToStartPosition;
    private enum DashingFlyingEnemyStates
    {
        Idle,
        Wander,
        Attack
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _startingPosition = transform.position;
        _currentState = DashingFlyingEnemyStates.Idle;
        _currentIdleTimer = idleTimer;
        _currentWanderTimer = wanderTimer;
        _moveDirection = Random.Range(0, 2) == 0 ? Vector3.left : Vector3.right;
        _player = GameObject.FindGameObjectWithTag("Player");
        _currentAttackTimer = attackTimer;
    }

    // Update is called once per frame
    void Update()
    {
        
        switch (_currentState)
        {
            case DashingFlyingEnemyStates.Idle:
            {
                _currentIdleTimer -= Time.deltaTime;
                if (_currentIdleTimer <= 0f)
                {
                    UpdateCurrentState(DashingFlyingEnemyStates.Wander);
                    _currentIdleTimer = idleTimer;
                }
                break;
            }
            case DashingFlyingEnemyStates.Wander:
            {
                _sinWaveTimer += Time.deltaTime;
                var verticalMovement = Mathf.Sin(_sinWaveTimer * wanderFrequency) * wanderMagnitude;
                var movementVector = _moveDirection * wanderSpeed + new Vector3(0, verticalMovement, 0);
                transform.position += movementVector * Time.deltaTime;

                var raycastHit2D = Physics2D.Raycast(transform.position, _moveDirection, 1.0f, wallLayer);
                if (raycastHit2D.collider != null)
                {
                    _moveDirection = -_moveDirection;
                }

                _currentWanderTimer -= Time.deltaTime;

                if (_currentWanderTimer <= 0)
                {
                    _moveDirection = Random.Range(0, 2) == 0 ? Vector3.left : Vector3.right;
                    UpdateCurrentState(DashingFlyingEnemyStates.Idle);
                    _currentWanderTimer = wanderTimer;
                }

                var hit2DColliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
                foreach (var hit2DCollider in hit2DColliders)
                {
                    if (hit2DCollider.CompareTag("Player"))
                    {
                        UpdateCurrentState(DashingFlyingEnemyStates.Attack);
                        break;
                    }
                }
                break;
            }

            case DashingFlyingEnemyStates.Attack:
            {
                if (_returningToStartPosition)
                {
                    transform.position = Vector3.MoveTowards(transform.position, _startingPosition,
                        returnSpeed * Time.deltaTime);
                    if (Vector3.Distance(transform.position, _startingPosition) < Mathf.Epsilon)
                    {
                        _returningToStartPosition = false;
                        UpdateCurrentState(DashingFlyingEnemyStates.Idle);
                    }
                }
                else
                {
                    var directionToPlayer = (_player.transform.position - transform.position).normalized;
                    transform.position += directionToPlayer * (attackDashSpeed * Time.deltaTime);
                    _currentAttackTimer -= Time.deltaTime;
                    if (_currentAttackTimer <= 0)
                    {
                        _currentAttackTimer = attackTimer;
                        _returningToStartPosition = true;
                    }
                }

                break;
            }
        }
    }

    private void UpdateCurrentState(DashingFlyingEnemyStates newState)
    {
        _currentState = newState;
    }
}
