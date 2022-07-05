using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public int coins = 0;

    public TMP_Text coinText;
    
    public float moveSpeed;
    public float maxVelo;

    public float rayDistance;
    public LayerMask groundLayer;

    public float jumpForce;
    
    private GameInput _gameInput;
    private PlayerInput _playerInput;
    private Camera _mainCamera;
    private Rigidbody _rigidbody;

    private Vector2 _moveInput;

    private bool _isGrounded;

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

        if (obj.action.name.CompareTo(_gameInput.Gameplay.Jump.name) == 0)
        {
            if (obj.performed) Jump();
        }

    }

    private void Move()
    {
        Vector3 camForward = _mainCamera.transform.forward;
        camForward.y = 0;
        
        //Y - calcula o movimento no eixo  da camera par ao movimento frente/tras
        Vector3 moveVertical = camForward * _moveInput.y;
        Vector3 camRight = _mainCamera.transform.right;
        //camRight.y = 0;
        
        //X - calcula o movimento no eixo  da camera par ao movimento esquerda/direita
        Vector3 moveHorizontal = camRight * _moveInput.x;
        
        _rigidbody.AddForce((_mainCamera.transform.forward * _moveInput.y + _mainCamera.transform.right * _moveInput.x) * moveSpeed * Time.fixedDeltaTime);
    }

    private void FixedUpdate()
    {
        Move();
        LimitedVelocity();
    }

    private void LimitedVelocity()
    {
        // pegar a velocidade do player
        Vector3 velocity = _rigidbody.velocity;

        // checar se a velocidade está dentro dos limites nos diferentes eixos
        //limitando o eixo x quando ifs, abs e 
        if (Mathf.Abs(velocity.x) > maxVelo) velocity.x = Mathf.Sign(velocity.x) * maxVelo;
        
        
        
        //maxvelocity < melocity.z < max velocity
        velocity.z = Mathf.Clamp(value: velocity.z, min: -maxVelo, maxVelo);

        //alterar a velocidade do player para ficar dentro dos limites
        _rigidbody.velocity = velocity;
    }
    //* como fazer o jogador pular
    
    //1 - checar se o jogador esta no chao
    
    //a - chegacr colisao a partir do fisico (usando os eventos de colidsão)
    //a - (Vantagens) facil de imprementar (adicionar uma função que ja existe no unity - OnCollisionEnter
    //a - (Desvantagens) nao sabemos a hora exata que a unity vai chamar essa função (pode ser que o jogador toque no chão e demore algunsframes par o jogo saber que ele está no chão)
    //b - atraves do raycast: 0-- a bolinha vai atirar um raio, o raio vai bater em algum objeto e a gente recebe o resultado dessa colisao
    //b - (Vantagens) resposta da colisão é imediata
    //b - (Desvantagens) mais complicado de configurar
    // uma variavel bool que vai dizer para o resto do codigo se o jogador estará no chao (true) ou não (false)
    
    //2 - jogador precisa apertar o botao de pulo
    // precisamos configurar o botao a ser ultilizado, para a acao de pular no nosso Input

    //3 - realizar o pulo atravez do fisico

    private void Jump()
    {
        if(_isGrounded) _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void CheckGround()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, rayDistance, groundLayer);
    }

    private void Update()
    {
        CheckGround();
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, Vector3.down * rayDistance, Color.magenta);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            coins++;
            coinText.text = coins.ToString();
            Destroy(other.gameObject);
            
        }
    }
}


