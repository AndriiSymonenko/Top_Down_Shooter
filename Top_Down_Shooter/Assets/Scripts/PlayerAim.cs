using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    private Player player;
    private PlayerControll playerControll;

    
    [SerializeField] private Transform aimTransform;
    [SerializeField] private LayerMask aimLayerMask;
    private Vector3 lookDirection;

    private Vector2 aimInput;


    private void Start()
    {
        player = GetComponent<Player>();
        AssignInputIvents();
    }

    private void Update()
    {
        aimTransform.position = new Vector3(GetMousePosition().x, transform.position.y, GetMousePosition().z);
    }

    public Vector3 GetMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(aimInput);

        if (Physics.Raycast(ray, out var hit, Mathf.Infinity, aimLayerMask))
        {
            return hit.point;
        }

        return Vector3.zero;
    }

    private void AssignInputIvents()
    {
        playerControll = player.playerControll;

        playerControll.Player.AimControll.performed += context => aimInput = context.ReadValue<Vector2>();
        playerControll.Player.AimControll.canceled += context => aimInput = Vector2.zero;
    }
}
