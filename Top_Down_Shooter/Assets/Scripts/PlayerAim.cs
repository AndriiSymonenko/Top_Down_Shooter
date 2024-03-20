using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    private Player player;
    private PlayerControll playerControll;

    [Header("Ain laser pointer")]
    [SerializeField] private LineRenderer aimLaser;



    [Header("Aim control")]
    [SerializeField] private Transform aim;
    

    [SerializeField] private bool isAimingPrecisly;
    [SerializeField] private bool isLockingAtTarget;

    [Header("Camera control")]
    [SerializeField] private Transform cameraTarget;
    [Range(.5f, 1f)]
    [SerializeField] private float minCameraDistance = 1f;
    [Range(1f, 3f)]
    [SerializeField] private float maxCameraDistance = 2.5f;
    [Range(3f, 5f)]
    [SerializeField] private float cameraSensitivity = 7;

    [Space]

    [SerializeField] private LayerMask aimLayerMask;

    private Vector2 mouseInput;
    private RaycastHit lastKnowMouseHit;


    private void Start()
    {
        player = GetComponent<Player>();
        AssignInputIvents();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse2))
            isLockingAtTarget = !isLockingAtTarget;

        if (Input.GetKeyDown(KeyCode.P))
            isAimingPrecisly = !isAimingPrecisly;

        UpdateAimLaser();
        UpdateAimPosition();
        UpdateCameraPosition();

    }


    private void UpdateAimLaser()
    {

        Transform gunPoint = player.weapon.GunPoint();
        Vector3 laserDirection = player.weapon.BulletDirection();

        float laserTipLenght = 0.5f;
        float gunDistance = 6f;

        Vector3 endPoint = gunPoint.position + laserDirection * gunDistance;

        if (Physics.Raycast(gunPoint.position, laserDirection, out RaycastHit hit, gunDistance))
        {
            endPoint = hit.point;
            laserTipLenght = 0;
        }

        aimLaser.SetPosition(0, gunPoint.position);
        aimLaser.SetPosition(1, endPoint);
        aimLaser.SetPosition(2, endPoint + laserDirection * laserTipLenght);
    }
    private void UpdateAimPosition()
    {
        Transform target = Target();

        if (target != null && isLockingAtTarget)
        {
            

            if (target.GetComponent<Renderer>() != null)
            {
                aim.position = target.GetComponent<Renderer>().bounds.center;
            }
            else
                aim.position = target.position;

            return;
        }

        aim.position = GetMouseHitInfo().point;

        if (!isAimingPrecisly)
            aim.position = new Vector3(aim.position.x, transform.position.y + 1, aim.position.z);
    }

    public Transform Aim() => aim;

    public Transform Target()
    {
        Transform target = null;

        if (GetMouseHitInfo().transform.GetComponent<Target>() != null)
        {
            target = GetMouseHitInfo().transform;
        }
        return target;
    }


    public RaycastHit GetMouseHitInfo()
    {
        Ray ray = Camera.main.ScreenPointToRay(mouseInput);

        if (Physics.Raycast(ray, out var hit, Mathf.Infinity, aimLayerMask))
        {
            lastKnowMouseHit = hit;
            return hit;
        }

        return lastKnowMouseHit;
    }


    #region Camera region
    private void UpdateCameraPosition()
    {
        cameraTarget.position = Vector3.Lerp(cameraTarget.position, DesiredCameraPosition(), cameraSensitivity * Time.deltaTime);
    }
    private Vector3 DesiredCameraPosition()
    {
        float actualMaxCameraDistance = player.playerMove.moveInput.y < -.5f ? minCameraDistance : maxCameraDistance;

        Vector3 desiredACameraPosition = GetMouseHitInfo().point;
        Vector3 aimDirection = (desiredACameraPosition - transform.position).normalized;

        float distanceToDesiredPosition = Vector3.Distance(transform.position, desiredACameraPosition);
        float clampedDistance = Mathf.Clamp(distanceToDesiredPosition, minCameraDistance, actualMaxCameraDistance);


        desiredACameraPosition = transform.position + aimDirection * clampedDistance;
        desiredACameraPosition.y = transform.position.y + 1;

        return desiredACameraPosition;
    }
    public bool CanAimPrecisly() => isAimingPrecisly;

    #endregion

    private void AssignInputIvents()
    {
        playerControll = player.playerControll;

        playerControll.Player.AimControll.performed += context => mouseInput = context.ReadValue<Vector2>();
        playerControll.Player.AimControll.canceled += context => mouseInput = Vector2.zero;
    }
}
