using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerControll playerControll { get; private set; } //Read-only;
    public PlayerAim playerAim { get; private set; } //Read-only
    public PlayerMove playerMove { get; private set; } //Read-only

    private void Awake()
    {
        playerControll = new PlayerControll();
        playerAim = GetComponent<PlayerAim>();
        playerMove = GetComponent<PlayerMove>();
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
