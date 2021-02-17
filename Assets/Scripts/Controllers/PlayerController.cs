﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour, IExecute
{
    private Rigidbody _rig;
    private bool _delayCounted;    
    private Vector3 _delay;
    private Vector3 _startTouchPosition;
    private InputController _inputController;
    private Transform _playerTransform;
    private Vector3 _startPosition;
    private Vector3 _movingVector;
    [SerializeField] private KolobotManager _idlePrefab;
    [SerializeField] private SpinningBotManager _runnerPrefab;
    private Animator _animatorIdle;
    private Animator _animatorRunner;
    [SerializeField] private Transform _rotateParentTransform;
    [SerializeField][Range(1, 10)] private float _sensetivity = 1;
    [SerializeField][Range(1f, 1000)] private float _speedModifyer = 1f;
    private float _smoothAnimation;
    private float _animationBlend;

    public bool _stateSpinning = false;
    [SerializeField] [Range(2f, 100f)] private float _smoothRange = 5;

    private JoystickController _input;



    private void Awake()
    {
        _rig = GetComponent<Rigidbody>();
        _inputController = FindObjectOfType<InputController>();
        _playerTransform = GetComponent<Transform>();
        

        _animatorIdle = _idlePrefab.GetComponent<Animator>();
        _animatorRunner = _runnerPrefab.GetComponent<Animator>();

        _input = FindObjectOfType<JoystickController>();
    }

    void Start()
    {
        _runnerPrefab.SetRenderersState(false);
    }

    public void Execute()
    {        
        
    }

    public void FixedUpdate()
    {
        OnMovement();
    }

    public void ChangeSpinningState(bool State)
    {
        _stateSpinning = State;
        if (!State)
        {
            _idlePrefab.SetRenderersState(true);
            _runnerPrefab.SetRenderersState(false);            
        }
        else
        {
            _idlePrefab.SetRenderersState(false);
            _runnerPrefab.SetRenderersState(true);
        }
    }

    private void OnMovement()
    {

        _movingVector = _input.GetDirection();

        if (_input.TouchStart)
        {
            _animatorIdle.SetBool("Spinning", true);

            _rig.AddForce(_movingVector * _speedModifyer * Time.fixedDeltaTime);


            _rotateParentTransform.rotation = Quaternion.RotateTowards(_rotateParentTransform.rotation, Quaternion.LookRotation(_movingVector), 30.0f);
            _smoothAnimation = _inputController.Magnitude;
            _animationBlend = _smoothAnimation / _smoothRange;
            if (_animationBlend > 1.0f)
            {
                _animationBlend = 1.0f;
            }
            _animatorRunner.SetFloat("Blend", _animationBlend);
            Debug.Log($"Blend:{_animationBlend}");

        }
        else 
        {            
            _delayCounted = false;
            _animatorIdle.SetBool("Spinning", false);
        }        

    }
    
}