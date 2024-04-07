using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerControll playerControll { get; private set; }
    public PlayerAim playerAim { get; private set; }
    public PlayerMove playerMove { get; private set; }
    public PlayerWeaponController weapon { get; private set; }
    public PlayerWeaponVisuals weaponVisuals { get; private set; }

    private void Awake()
    {
        playerControll = new PlayerControll();
        playerAim = GetComponent<PlayerAim>();
        playerMove = GetComponent<PlayerMove>();
        weapon = GetComponent<PlayerWeaponController>();
        weaponVisuals = GetComponent<PlayerWeaponVisuals>();
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
