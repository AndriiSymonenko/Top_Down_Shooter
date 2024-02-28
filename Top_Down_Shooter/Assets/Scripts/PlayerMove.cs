using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    private PlayerControll playerControll;
    private CharacterController characterController;
    private Animator animator;


    [Header("Movement Setup")]
    [SerializeField] private float playerWalkSpeed;
    [SerializeField] private Transform aimTransform;

    [Header("Aim Setup")]
    [SerializeField] private LayerMask layerMaskAim;

    private Vector3 lookDirection;

    private Vector3 moveDirection;

    private Vector2 moveInput;
    private Vector2 aimInput;
    private float vertycalVelocity;


    private void Awake()
    {

        playerControll = new PlayerControll();

        playerControll.Player.PlayerMove.performed += context => moveInput = context.ReadValue<Vector2>();
        playerControll.Player.PlayerMove.canceled += context => moveInput = Vector2.zero;

        playerControll.Player.AimControll.performed += context => aimInput = context.ReadValue<Vector2>();
        playerControll.Player.AimControll.canceled += context => aimInput = Vector2.zero;
    }

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        ApplyMove();
        AimMousePosition();
        AnimatorControll();
    }

    private void AimMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(aimInput);

        if (Physics.Raycast(ray, out var hit, Mathf.Infinity, layerMaskAim))
        {
            lookDirection = hit.point - transform.position;
            lookDirection.y = 0f;
            lookDirection.Normalize();

            transform.forward = lookDirection;

            aimTransform.position = new Vector3(hit.point.x, transform.position.y, hit.point.z);
        }
    }

    private void AnimatorControll()
    {
        float xVelocity = Vector3.Dot(moveDirection.normalized, transform.right);
        float zVelocity = Vector3.Dot(moveDirection.normalized, transform.forward);

        animator.SetFloat("xVelocity", xVelocity, .1f, Time.deltaTime);
        animator.SetFloat("zVelocity", zVelocity, .1f, Time.deltaTime);
    }
    private void ApplyMove()
    {
        moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
        ApllyGravity();

        if (moveDirection.magnitude > 0)
        {
            characterController.Move(moveDirection * playerWalkSpeed * Time.deltaTime);
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

    private void OnEnable()
    {
        playerControll.Enable();
    }

    private void OnDisable()
    {
        playerControll.Disable();
    }
}
