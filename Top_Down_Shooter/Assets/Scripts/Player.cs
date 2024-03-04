using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerControll playerControll;

    private void Awake()
    {
        playerControll = new PlayerControll();
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
