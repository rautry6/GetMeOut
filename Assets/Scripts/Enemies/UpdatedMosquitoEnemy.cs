using System;
using System.Collections;
using System.Numerics;
using DDA;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Enemies
{
    public class UpdatedMosquitoEnemy : MonoBehaviour
    {
        [SerializeField] private float inactiveCheckInterval;
        [SerializeField] private LayerMask groundLayer;
        
        [Header("Shooting Variables")] [SerializeField]
        private GameObject projectile;
        [SerializeField] private float shootingDistance;
        [SerializeField] private Transform firingPoint;
        [SerializeField] private float easyTimeBeforeShooting;
        [SerializeField] private float mediumTimeBeforeShooting;
        [SerializeField] private float hardTimeBeforeShooting;

        [Header("Buzz Around Variables")]
        [SerializeField] private float activationDistance;
        [SerializeField] private int startingShotCount;
        [SerializeField] private float moveRadius;
        [SerializeField] private float moveSpeed;

        [Header("Sucking Blood Variables")] [SerializeField]
        private float transitionTimeToSuckBlood;
        [SerializeField] private float timeBetweenDashes;
        [SerializeField] private float easyDashSpeed;
        [SerializeField] private float mediumDashSpeed;
        [SerializeField] private float hardDashSpeed;
        [SerializeField] private float dashDuration;
        [SerializeField] private int easyMaxDashes;
        [SerializeField] private int mediumMaxDashes;
        [SerializeField] private int hardMaxDashes;

        private Behaviors _currentBehavior = Behaviors.Inactive;
        private GameObject _player;
        private float _angle;
        private Vector3 _startingPosition;
        private Vector3 _globalStartingPosition;
        private int _currentShotCount;
        private float _currentShotTimer;
        private bool _enteringSuckingBlood;
        private float _currentTimeBetweenTransitionToSuckBlood;
        private Rigidbody2D _rigidbody;
        private float _currentDashCooldown;
        private bool _tweenStarted;
        private float _currentInactiveCheckInterval;
        private Coroutine _buzzAroundRoutine;
        private bool _buzzing;
        private int _currentDashCounter;
        private bool _canDash = true;
        private float _dashSpeed;
        private int _currentMaxDashes;
         


        private enum Behaviors
        {
            BuzzingAround,
            Shooting,
            SuckingBlood,
            Returning,
            Inactive,
        }

        private void Awake()
        {
            _startingPosition = transform.localPosition;
            _globalStartingPosition = transform.position;
        }

        private void OnEnable()
        {
            DDA.DDA.EmitDifficultyUpdate += UpdateDifficulty;
        }

        private void OnDisable()
        {
            DDA.DDA.EmitDifficultyUpdate -= UpdateDifficulty;
        }

        // Start is called before the first frame update
        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            _currentShotCount = startingShotCount;
            _currentShotTimer = mediumTimeBeforeShooting;
            _currentTimeBetweenTransitionToSuckBlood = transitionTimeToSuckBlood;
            _rigidbody = GetComponent<Rigidbody2D>();
            _dashSpeed = mediumDashSpeed;
        }

        // Update is called once per frame
        private void Update()
        {
            CheckForCorrectFacingOrientation();
            if (_currentDashCounter >= _currentMaxDashes)
            {
                UpdateCurrentBehavior(Behaviors.Returning);
            }
            
            switch (_currentBehavior)
            {
                case Behaviors.Inactive:
                {
                    _currentInactiveCheckInterval -= Time.deltaTime;
                    if (_currentInactiveCheckInterval <= 0)
                    {
                        _currentInactiveCheckInterval = inactiveCheckInterval;
                        if (GetDistanceFromPlayer() <= activationDistance)
                        {
                            UpdateCurrentBehavior(Behaviors.BuzzingAround);
                        }
                    }
                    break;
                }
                case Behaviors.BuzzingAround:
                {
                    if (!IsPlayerWithinActivationDistance())
                    {
                        UpdateCurrentBehavior(Behaviors.Inactive);
                    }
                    BuzzAround();
                    if (IsPlayerWithinShootingDistance())
                    {
                        if (NoWallBetweenMosquitoAndPlayer())
                        {
                            UpdateCurrentBehavior(Behaviors.Shooting);    
                        }
                        
                    }

                    break;
                }
                case Behaviors.Shooting:
                {
                    BuzzAround();
                    _currentShotTimer -= Time.deltaTime;
                    if (_currentShotCount > 0 && _currentShotTimer <= 0)
                    {
                        _currentShotCount--;
                        ShootProjectile();
                        _currentShotTimer = CheckCurrentDifficultyShotTimer();
                    }

                    if (_currentShotCount <= 0) UpdateCurrentBehavior(Behaviors.SuckingBlood);

                    break;
                }

                case Behaviors.SuckingBlood:
                {
                    if (_enteringSuckingBlood)
                    {
                        _currentTimeBetweenTransitionToSuckBlood -= Time.deltaTime;
                        if (_currentTimeBetweenTransitionToSuckBlood <= 0) _enteringSuckingBlood = false;
                    }
                    else
                    {
                        if (!IsPlayerWithinActivationDistance())
                        {
                            UpdateCurrentBehavior(Behaviors.Returning);
                            return;
                        }

                        _currentDashCooldown -= Time.deltaTime;
                        if (_currentDashCooldown <= 0 && _canDash)
                        {
                            _canDash = false;
                            DashTowardsPlayer();
                        }
                    }

                    break;
                }

                case Behaviors.Returning:
                {
                    if (!_tweenStarted)
                    {
                        transform.DOLocalMove(_startingPosition, 3f).OnStart(() =>
                        {
                            _tweenStarted = true;
                            Debug.LogWarning("Started!");
                        }).OnComplete(() =>
                        {
                            Debug.LogWarning("Finished!");
                            UpdateCurrentBehavior(Behaviors.BuzzingAround);
                            _tweenStarted = false;
                            _currentDashCounter = 0;
                        });    
                    }
                    
                    break;
                }

                default:
                    Debug.LogWarning("Default case hit in MosquitoEnemyV2 Update");
                    break;
            }
        }

        private float CheckCurrentDifficultyShotTimer()
        {
            switch (DDA.DDA.CurrentDifficulty)
            {
                case Difficulties.Easy: return easyTimeBeforeShooting; 
                case Difficulties.Medium: return mediumTimeBeforeShooting; 
                case Difficulties.Hard: return hardTimeBeforeShooting; 
            }
            Debug.LogWarning("UpdatedMosquitoEnemy.CheckCurrentDifficultyShotTimer switch failed!");
            return mediumTimeBeforeShooting;
        }

        private bool NoWallBetweenMosquitoAndPlayer()
        {
            var hit = Physics2D.Raycast(transform.position, GetDirectionToPlayer().normalized, Mathf.Min(activationDistance, GetDistanceFromPlayer()),
                groundLayer);
            if (hit.collider != null)
            {
                return false;
            }

            return true;
        }

        private float GetDistanceFromPlayer()
        {
            return Vector3.Distance(transform.position, _player.transform.position);
        }

        private bool IsPlayerWithinShootingDistance()
        {
            return Vector3.Distance(transform.position, _player.transform.position) <= shootingDistance;
        }

        private void ShootProjectile()
        {
            var spawnedProjectile = Instantiate(projectile, firingPoint.position, Quaternion.identity);
            var directionToPlayer = GetFiringPointDirectionToPlayer();
            Debug.DrawRay(firingPoint.position, directionToPlayer, Color.green);
            var angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg - 90f;
            spawnedProjectile.transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        private Vector3 GetFiringPointDirectionToPlayer()
        {
            return _player.transform.position - firingPoint.transform.position;
        }

        private Vector3 GetDirectionToPlayer()
        {
            return _player.transform.position - transform.position;
        }

        private void UpdateCurrentBehavior(Behaviors newBehavior)
        {
            if (newBehavior == Behaviors.SuckingBlood)
                _enteringSuckingBlood = true;
            else
                _currentTimeBetweenTransitionToSuckBlood = transitionTimeToSuckBlood;

            _currentBehavior = newBehavior;
        }

        private bool IsPlayerWithinActivationDistance()
        {
            var distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);
            Debug.DrawRay(transform.position, _player.transform.position - transform.position, Color.yellow, 1.5f);
            return  distanceToPlayer <= activationDistance;
        }

        private void CheckForCorrectFacingOrientation()
        {
            if (transform.position.x < _player.transform.position.x)
                transform.localScale = new Vector3(-1, 1, 1);
            else
                transform.localScale = new Vector3(1, 1, 1);
        }

        private void DashTowardsPlayer()
        {
            var direction = GetDirectionToPlayer().normalized;
            _rigidbody.velocity = (Vector2)direction * _dashSpeed;
            Debug.Log("Dashing");
            StartCoroutine(StopDash());
        }

        private IEnumerator StopDash()
        {
            yield return new WaitForSeconds(dashDuration);
            _rigidbody.velocity = Vector2.zero;
            _currentDashCounter++;
            _canDash = true;
            _currentDashCooldown = timeBetweenDashes;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _currentShotCount = startingShotCount;
                StopCoroutine(_buzzAroundRoutine);
                UpdateCurrentBehavior(Behaviors.Returning);
            }
        }

        private void BuzzAround()
        {
            var randomDirection = Random.insideUnitCircle.normalized;
            var targetPosition = _globalStartingPosition + new Vector3(randomDirection.x, randomDirection.y, 0) * moveRadius;
            if (!_buzzing)
            {
                _buzzing = true;
                _buzzAroundRoutine = StartCoroutine(Buzz(targetPosition));    
            }
            
        }

        private IEnumerator Buzz(Vector3 targetPosition)
        {
            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                var position = transform.position;
                var moveDirection = (targetPosition - position).normalized;
                var nextPosition = position + moveDirection * moveSpeed;
                _rigidbody.MovePosition(nextPosition);
                yield return null;
            }
            _rigidbody.MovePosition(targetPosition);
            yield return new WaitForSeconds(0.5f);
            _buzzing = false;
            
        }

        public void UpdateDifficulty(Difficulties difficulty)
        {
            switch (difficulty)
            {
                case Difficulties.Easy:
                {
                    _currentMaxDashes = easyMaxDashes;
                    _dashSpeed = easyDashSpeed;
                    break;
                }
                case Difficulties.Medium:
                {
                    _currentMaxDashes = mediumMaxDashes;
                    _dashSpeed = mediumDashSpeed;
                    break;
                }
                case Difficulties.Hard:
                {
                    _currentMaxDashes = hardMaxDashes;
                    _dashSpeed = hardDashSpeed;
                    break;
                }
                default: Debug.LogWarning("Default case hit in UpdatedMosquitoEnemy.UpdateDashSpeed");
                    break;
            }
        }
    }
}