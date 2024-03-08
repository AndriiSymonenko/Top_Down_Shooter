using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    private WeaponVisualController visualController;

    private void Start()
    {
        visualController = GetComponentInParent<WeaponVisualController>();


    }

    public void ReloadIsOver()
    {
        visualController.ReturnRigWeigthToOne();

        //Refill-bulets
    }

    public void ReturnRig()
    {
        visualController.ReturnRigWeigthToOne();
        visualController.ReturnWeightToLeftHand();

    }

    public void WeaponGrabIsOver()
    {
        visualController.SetBusyGrabWeaponTo(false);
    }
}
