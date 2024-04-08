using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GrabType { SideGrab, BackGrab };
public enum HoldType { CommonHold, LowHold };

public class WeaponModel : MonoBehaviour
{
    public WeaponType weaponType;
    public GrabType grabType;
    public HoldType holdType;

    public Transform gunPoint;
    public Transform holdPoint;
}
