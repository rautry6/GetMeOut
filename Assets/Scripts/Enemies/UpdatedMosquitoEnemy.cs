using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class UpdatedMosquitoEnemy : MonoBehaviour
    {
        [SerializeField] private float inactiveCheckInterval;
        [Header("Shooting Variables")] [SerializeField]
        private GameObject projectile;
        [SerializeField] private float shootingDistance;
        [SerializeField] private Transform firingPoint;
        [SerializeField] private float timeBeforeShooting;

        [Header("Buzz Around Variables")] [SerializeField]
        private float _buzzingRadius;

        [SerializeField] private float _buzzingSpeed;
        [SerializeField] private float activationDistance;
        [SerializeField] private int startingShotCount;

        [Header("Sucking Blood Variables")] [SerializeField]
        private float transitionTimeToSuckBlood;
        [SerializeField] private float timeBetweenDashes;
        [SerializeField] private float dashSpeed;
        [SerializeField] private float dashDuration;
        [SerializeField] private float returnSpeed;

        private Behaviors _currentBehavior;
        private GameObject _player;
        private float _angle;
        private Vector3 _startingPosition;
        private int _currentShotCount;
        private float _currentShotTimer;
        private bool _enteringSuckingBlood;
        private float _currentTimeBetweenTransitionToSuckBlood;
        private Rigidbody2D _rigidbody;
        private float _currentDashCooldown;
        private bool _tweenStarted;
        private float _currentInactiveCheckInterval;
        

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
            Debug.Log($"StartingPosition: {_startingPosition.x}, {_startingPosition.y}, {_startingPosition.z}");
        }

        // Start is called before the first frame update
        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            _currentShotCount = startingShotCount;
            _currentShotTimer = timeBeforeShooting;
            _currentTimeBetweenTransitionToSuckBlood = transitionTimeToSuckBlood;
            _rigidbody = GetComponent<Rigidbody2D>();
            _currentBehavior = Behaviors.Inactive;
        }

        // Update is called once per frame
        private void Update()
        {
            CheckForCorrectFacingOrientation();

            switch (_currentBehavior)
            {
                case Behaviors.Inactive:
                {
                    _currentInactiveCheckInterval -= Time.deltaTime;
                    if (_currentInactiveCheckInterval <= 0)
                    {
                        _currentInactiveCheckInterval = inactiveCheckInterval;
                        if (Vector3.Distance(transform.position, _player.transform.position) <= activationDistance)
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
                    if (IsPlayerWithinShootingDistance()) UpdateCurrentBehavior(Behaviors.Shooting);
                    
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
                        _currentShotTimer = timeBeforeShooting;
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
                        if (_currentDashCooldown <= 0) DashTowardsPlayer();
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
                            UpdateCurrentBehavior(Behaviors.BuzzingAround);
                            _tweenStarted = false;
                        });    
                    }
                    
                    break;
                }

                default:
                    Debug.LogWarning("Default case hit in MosquitoEnemyV2 Update");
                    break;
            }
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

        private void BuzzAround()
        {
            _angle += (_buzzingSpeed + Random.Range(-0.05f, 0.05f)) * Time.deltaTime;
            var x = transform.position.x + _buzzingRadius * Mathf.Cos(_angle);
            var y = transform.position.y + _buzzingRadius * Mathf.Sin(_angle);
            x += Random.Range(-0.05f, 0.05f);
            y += Random.Range(-0.05f, 0.05f);
            transform.position = new Vector3(x, y, 0);
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
            _rigidbody.velocity = (Vector2)direction * dashSpeed;
            StartCoroutine(StopDash());
        }

        private IEnumerator StopDash()
        {
            yield return new WaitForSeconds(dashDuration);
            _rigidbody.velocity = Vector2.zero;
            _currentDashCooldown = timeBetweenDashes;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _currentShotCount = startingShotCount;
                UpdateCurrentBehavior(Behaviors.Returning);
            }
        }
    }
}