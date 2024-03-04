using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    private Player player;

    private void Start()
    {
        player = GetComponent<Player>();
        player.playerControll.Player.Shoot.performed += context => Shoot();
    }
    private void Shoot()
    {
        GetComponentInChildren<Animator>().SetTrigger("Shoot");
    }
}
