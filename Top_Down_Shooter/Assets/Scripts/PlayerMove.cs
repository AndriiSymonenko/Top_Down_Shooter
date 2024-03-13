using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{

    private Player player; 

    private PlayerControll playerControll;
    private CharacterController characterController;
    private Animator animator;


    [Header("Movement Setup")]
    [SerializeField] private float playerWalkSpeed;
    [SerializeField] private float playerRunSpeed;
    [SerializeField] private float turnSpeed;
    private float vertycalVelocity;
    private bool isRunning;
    private float speed;

    public Vector2 moveInput { get; private set; }
    private Vector3 moveDirection;







    private void Start()
    {
        player = GetComponent<Player>();

        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();

        speed = playerWalkSpeed;

        AssaingInputEvent();
    }

    private void Update()
    {
        ApplyMove();
        ApllyRotation();
        AnimatorControll();
    }




    private void AssaingInputEvent()
    {
        playerControll = player.playerControll;

        playerControll.Player.PlayerMove.performed += context => moveInput = context.ReadValue<Vector2>();
        playerControll.Player.PlayerMove.canceled += context => moveInput = Vector2.zero;

        playerControll.Player.Run.performed += context =>
        {
            speed = playerRunSpeed;
            isRunning = true;
        };
        playerControll.Player.Run.canceled += context =>
        {
            speed = playerWalkSpeed;
            isRunning = false;
        };
    }


    private void ApllyRotation()
    {
          Vector3  lookDirection = player.playerAim.GetMouseHitInfo().point - transform.position;
          lookDirection.y = 0f;
          lookDirection.Normalize();

        Quaternion desiredRotation = Quaternion.LookRotation(lookDirection);

        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, turnSpeed * Time.deltaTime);
    }

    private void AnimatorControll()
    {
        float xVelocity = Vector3.Dot(moveDirection.normalized, transform.right);
        float zVelocity = Vector3.Dot(moveDirection.normalized, transform.forward);

        animator.SetFloat("xVelocity", xVelocity, .1f, Time.deltaTime);
        animator.SetFloat("zVelocity", zVelocity, .1f, Time.deltaTime);

        bool playRunAnimation = isRunning && moveDirection.magnitude > 0;
        animator.SetBool("isRunning", playRunAnimation);
    }
    private void ApplyMove()
    {
        moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
        ApllyGravity();

        if (moveDirection.magnitude > 0)
        {
            characterController.Move(moveDirection * speed * Time.deltaTime);
        }
    }

    private void ApllyGravity()
    {
        if (!characterController.isGrounded)
        {
            vertycalVelocity -= 9.81f * Time.deltaTime;
            moveDirection.y = vertycalVelocity;
        }
        else
        {
            vertycalVelocity = - 0.5f;
        }
    }

}
