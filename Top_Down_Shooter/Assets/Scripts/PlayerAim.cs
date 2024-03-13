using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    private Player player;
    private PlayerControll playerControll;

    [Header("Aim control")]
    [SerializeField] private Transform aim;

    [SerializeField] private bool isAimingPrecisly;

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

    private Vector2 aimInput;
    private RaycastHit lastKnowMouseHit;


    private void Start()
    {
        player = GetComponent<Player>();
        AssignInputIvents();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            isAimingPrecisly = !isAimingPrecisly;

        UpdateAimPosition();
        UpdateCameraPosition();

    }

    private void UpdateCameraPosition()
    {
        cameraTarget.position = Vector3.Lerp(cameraTarget.position, DesiredCameraPosition(), cameraSensitivity * Time.deltaTime);
    }

    private void UpdateAimPosition()
    {
        aim.position = GetMouseHitInfo().point;

        if (!isAimingPrecisly)
            aim.position = new Vector3(aim.position.x, transform.position.y + 1, aim.position.z);
    }

    public RaycastHit GetMouseHitInfo()
    {
        Ray ray = Camera.main.ScreenPointToRay(aimInput);

        if (Physics.Raycast(ray, out var hit, Mathf.Infinity, aimLayerMask))
        {
            lastKnowMouseHit = hit;
            return hit;
        }

        return lastKnowMouseHit;
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

    public bool CanAimPrecisly()
    {
        if (isAimingPrecisly)
            return true;

        return false;
    }

    private void AssignInputIvents()
    {
        playerControll = player.playerControll;

        playerControll.Player.AimControll.performed += context => aimInput = context.ReadValue<Vector2>();
        playerControll.Player.AimControll.canceled += context => aimInput = Vector2.zero;
    }
}
