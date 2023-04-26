using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{

    public PlayerState State { get; private set; } = PlayerState.Movement;

    [SerializeField] private FixedStopwatch jumpStopwatch = new FixedStopwatch();

    [SerializeField] private float maxWalkCos = 0.5f;
    [SerializeField] private float walkSpeed = 7;

    [SerializeField] private float jumpSpeed = 3;
    [SerializeField] private float fallSpeed = 12;
    [SerializeField] private int numberOfJumps = 1;
    [SerializeField] private AnimationCurve jumpFallOff = AnimationCurve.Linear(0, 1, 1, 0);

    public Vector2 DesiredDirection { get; private set; }

    public bool IsGrounded => _groundContact.HasValue;
    public Vector2 Velocity => _rigidBody2D.velocity;
    public float JumpCompletion => jumpStopwatch.Completion;
    public bool IsJumping => !jumpStopwatch.IsFinished;
    public int FacingDirection { get; private set; } = 1;
    
    private Rigidbody2D _rigidBody2D;
    private ContactFilter2D _contactFilter;
    private ContactPoint2D? _groundContact;
    private ContactPoint2D? _ceilingContact;
    private ContactPoint2D? _wallContact;
    private readonly ContactPoint2D[] _contacts = new ContactPoint2D[16];

    private bool _wantsToJump;
    private bool _wasOnTheGround;
    private int _jumpsLeft;

    public void OnMovement(InputValue value)
    {
        DesiredDirection = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        _wantsToJump = value.Get<float>() > 0.5f;
        float jumpVal = value.Get<float>();

        if (_wantsToJump)
            RequestJump();
        else
            jumpStopwatch.Reset();
    }


    private void RequestJump()
    {
        if (State != PlayerState.Movement || _jumpsLeft <= 0)
            return;

        _jumpsLeft--;
        jumpStopwatch.Split();
    }

    private void FindContacts()
    {
        _groundContact = null;
        _ceilingContact = null;
        _wallContact = null;

        float groundProjection = maxWalkCos;
        float wallProjection = maxWalkCos;
        float ceilingProjection = -maxWalkCos;

        int numberOfContacts = _rigidBody2D.GetContacts(_contactFilter, _contacts);
        for (var i = 0; i < numberOfContacts; i++)
        {
            var contact = _contacts[i];
            float projection = Vector2.Dot(Vector2.up, contact.normal);

            if (projection > groundProjection)
            {
                _groundContact = contact;
                groundProjection = projection;
            }
            else if (projection < ceilingProjection)
            {
                _ceilingContact = contact;
                ceilingProjection = projection;
            }
            else if (projection <= wallProjection)
            {
                _wallContact = contact;
                wallProjection = projection;
            }
        }
    }

    private void FixedUpdate()
    {
        FindContacts();

        switch (State)
        {
            case PlayerState.Movement:
                UpdateMovementState();
                break;
        }
    }

    private void UpdateMovementState()
    {
        var previousVelocity = _rigidBody2D.velocity;
        var velocityChange = Vector2.zero;

        if (DesiredDirection.x > 0)
            FacingDirection = 1;
        else if (DesiredDirection.x < 0)
            FacingDirection = -1;

        if (_wantsToJump && IsJumping)
        {
            _wasOnTheGround = false;
            float currentJumpSpeed = jumpSpeed;
            currentJumpSpeed *= jumpFallOff.Evaluate(JumpCompletion);
            velocityChange.y = currentJumpSpeed - previousVelocity.y;

            if (_ceilingContact.HasValue)
                jumpStopwatch.Reset();
        }
        else if (_groundContact.HasValue)
        {
             _jumpsLeft = numberOfJumps;
            _wasOnTheGround = true;
        }
        else
        {
            if (_wasOnTheGround)
            {
                _jumpsLeft -= 1;
                _wasOnTheGround = false;
            }

            velocityChange.y = (-fallSpeed - previousVelocity.y) / 8;
        }

        velocityChange.x = (DesiredDirection.x * walkSpeed - previousVelocity.x) / 4;

        if (_wallContact.HasValue)
        {
            var wallDirection = (int) Mathf.Sign(_wallContact.Value.point.x - transform.position.x);
            var walkDirection = (int) Mathf.Sign(DesiredDirection.x);

            if (walkDirection == wallDirection)
                velocityChange.x = 0;
        }

        _rigidBody2D.AddForce(velocityChange, ForceMode2D.Impulse);
    }

    private void Awake()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _contactFilter = new ContactFilter2D();
        _contactFilter.SetLayerMask(LayerMask.GetMask("Ground"));
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
