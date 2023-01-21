using System;
using UnityEngine;
using DefaultNamespace.Checks;
public class Move : MonoBehaviour
{
    [SerializeField] private InputController inputController = null;
    [SerializeField, Range(0f, 100f)] private float maxSpeed = 4f;
    [SerializeField, Range(0f, 100f)] private float maxGroundAcc = 35f;
    [SerializeField, Range(0f, 100f)] private float maxAirAcc = 20f;

    private Vector2 _direction;
    private Vector2 _desiredVelocity;
    private Vector2 _currentVelocity;
    private Rigidbody2D _playerRigidbody;
    private Ground _ground;
    private float _maxSpeedChange;
    private float _acceleration;
    private bool _onGround;

    private void Awake()
    {
        _playerRigidbody = GetComponent<Rigidbody2D>();
        _ground = GetComponent<Ground>();
    }

    private void Update()
    {
        _direction.x = inputController.RetrieveMovementInput();
        _desiredVelocity = new Vector2(_direction.x, 0f) * Mathf.Max(maxSpeed - _ground.Friction, 0f);
    }

    private void FixedUpdate()
    {
        _onGround = _ground.OnGround;
        _currentVelocity = _playerRigidbody.velocity;
        _acceleration = _onGround ? maxGroundAcc : maxAirAcc;
        _maxSpeedChange = _acceleration * Time.deltaTime;
        _currentVelocity.x = Mathf.MoveTowards(_currentVelocity.x, _desiredVelocity.x, _maxSpeedChange);
        _playerRigidbody.velocity = _currentVelocity;
    }
}
