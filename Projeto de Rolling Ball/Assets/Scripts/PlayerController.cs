using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    
    private GameInput _gameInput;
    private PlayerInput _playerInput;
    private Camera _mainCamera;
    private Rigidbody _rigidbody;

    private Vector2 _moveInput;

    private void OnEnable()
    {
        // inicialização de variavel
        _gameInput = new GameInput();

        // Referencia dos componestes no mesmo objeto da unity
        _playerInput = GetComponent<PlayerInput>();
        _rigidbody = GetComponent<Rigidbody>();
        
        //Referencia para camera main guardada na classe Camera
        _mainCamera = Camera.main;
        
        //delegate do action triggered no player input
        _playerInput.onActionTriggered += OnActionTriggered;
    }

    private void OnDisable()
    {
        _playerInput.onActionTriggered -= OnActionTriggered;
    }

    private void OnActionTriggered(InputAction.CallbackContext obj)
    {
        //comparando o nome da action que esta chegando com o nome da action de movement
        if (obj.action.name.CompareTo(_gameInput.Gameplay.Movement.name) == 0)
        {
            //atribuir ao moveInput o valor proveniente do input do jogador como  um vector 2
            _moveInput = obj.ReadValue<Vector2>();
        }
    }

    private void Move()
    {
        //Y - calcula o movimento no eixo  da camera par ao movimento frente/tras
        //X - calcula o movimento no eixo  da camera par ao movimento esquerda/direita
        
        _rigidbody.AddForce((_mainCamera.transform.forward * _moveInput.y + _mainCamera.transform.right * _moveInput.x) * moveSpeed * Time.fixedDeltaTime);
    }

    private void FixedUpdate()
    {
        Move();
    }
}


