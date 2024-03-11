using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    private PlayerWeaponVisuals visualController;

    private void Start()
    {
        visualController = GetComponentInParent<PlayerWeaponVisuals>();


    }

    public void ReloadIsOver()
    {
        visualController.MaximazeRigWeight();

        //Refill-bulets
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
