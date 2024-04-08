using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    private PlayerWeaponVisuals visualController;
    private PlayerWeaponController weaponController;



    private void Start()
    {
        visualController = GetComponentInParent<PlayerWeaponVisuals>();
        weaponController = GetComponentInParent<PlayerWeaponController>();

    }

    public void ReloadIsOver()
    {
        visualController.MaximazeRigWeight();
        weaponController.CurrentWeapon().RefillBullets();
    }


    public void ReturnRig()
    {
        visualController.MaximazeRigWeight();
        visualController.MaximazeLeftHandWeight();

    }

    public void WeaponGrabIsOver()
    {
        visualController.SetBusyGrabWeaponTo(false);
    }
}
