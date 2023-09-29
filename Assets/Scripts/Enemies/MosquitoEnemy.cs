using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MosquitoEnemy : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private float drainFlySpeed;
    [SerializeField] private int numberOfShots;
    [SerializeField] private float activationDistance;
    [SerializeField] private float deactivationDistance;
    [SerializeField] private float delayBeforeBloodSuck;
    [SerializeField] private float durationBetweenShots;

    private float _currentDurationBetweenShots;
    private int _currentNumberOfShotsRemaining;
    private Vector3 _startingPosition;
    private bool _isReturning;
    private GameObject _player;
    private bool _shouldBloodSuck;
    private Rigidbody2D _rigidbody2D;
    private bool _isActive;
    private float _currentBloodSuckTimer;
    private Vector3 direction;
    private bool directionGot = false;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _currentDurationBetweenShots = durationBetweenShots;
        _currentNumberOfShotsRemaining = numberOfShots;
        _startingPosition = transform.position;
        _currentBloodSuckTimer = delayBeforeBloodSuck;
    }

    // Update is called once per frame
    void Update()
    {
   
        CheckForCorrectFacingOrientation();
        var distanceToPlayer = Vector2.Distance(transform.position, _player.transform.position);
        Debug.Log($"isActive {_isActive}, isReturning: {_isReturning} distanceToPlayer: {distanceToPlayer}");
        if (distanceToPlayer <= activationDistance)
        {
            _isActive = true;
        }
        else if (distanceToPlayer > deactivationDistance)
        {
            _isActive = false;
        }

        if (!_isActive)
        {
            transform.DOMove(_startingPosition, 2f).OnComplete(() =>
            {
                directionGot = false;
            });
            return;
        }
        
        if (_isReturning)
        {
            transform.DOMove(_startingPosition, 2f).OnComplete(() =>
            {
                directionGot = false; 
                _isReturning = false;
            });
        }
        if (_currentNumberOfShotsRemaining > 0)
        {
            _currentDurationBetweenShots -= Time.deltaTime;
            if (_currentDurationBetweenShots <= 0)
            {
                _currentNumberOfShotsRemaining--;
                
                // Shoot projectile
                var spawnedProjectile = Instantiate(projectile, firingPoint.position, Quaternion.identity);
                var directionToPlayer = _player.transform.position - firingPoint.position;
                Debug.DrawRay(firingPoint.position, directionToPlayer, Color.green);
                var angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg - 90f;
                spawnedProjectile.transform.rotation = Quaternion.Euler(0,0, angle);
                _currentDurationBetweenShots = durationBetweenShots;
            }
        }
        else
        {
            _currentBloodSuckTimer -= Time.deltaTime;
            _shouldBloodSuck = _currentBloodSuckTimer <= 0f;

            if (_shouldBloodSuck)
            {
                // Go drain blood from player

                if (!directionGot)
                {
                    directionGot = true;
                    direction = _player.transform.position - transform.position;
                }

                var directionNormalized = direction.normalized;
                var movementVector = directionNormalized * drainFlySpeed * Time.deltaTime;
                _rigidbody2D.MovePosition(transform.position + movementVector);    
            }
        }
    }

    private void CheckForCorrectFacingOrientation()
    {
        if (transform.position.x < _player.transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _currentBloodSuckTimer = delayBeforeBloodSuck;
            _shouldBloodSuck = false;
            other.GetComponent<PlayerHealth>().TakeDamage();
            _currentNumberOfShotsRemaining = numberOfShots;
            // go back to start position;
            _isReturning = true;
        }
    }
}
