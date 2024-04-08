using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerWeaponVisuals : MonoBehaviour
{
    private Player player;
    private Animator animator;
    private bool isGrabingWeapon;




    [SerializeField] private WeaponModel[] weaponModels;

    [Header("Rig")]
    [SerializeField] private float rigWeightIncreaseRate;
    private bool shouldIncrease_RigWeight;
    private Rig rig;

    [Header("Left Hand IK")]
    [SerializeField] TwoBoneIKConstraint leftHandIK;
    [SerializeField] private Transform leftHandIK_Target;
    [SerializeField] private float leftHandIKWeightIncreaseRate;
    private bool shouldIncrease_LeftHandIKWeight;




    private void Start()
    {
        player = GetComponent<Player>();
        rig = GetComponentInChildren<Rig>();
        animator = GetComponentInChildren<Animator>();

        weaponModels = GetComponentsInChildren<WeaponModel>(true);

       
    }

    private void Update()
    {
        Che�kWeaponSwitch();

        UpdateRigWeight();

        UpdateLeftHandIKWeight();
    }

    public WeaponModel CurrentWeaponModel()
    {
        WeaponModel weaponModel = null;

        WeaponType weaponType = player.weapon.CurrentWeapon().weaponType;

        for (int i = 0; i < weaponModels.Length; i++)
        {
            if (weaponModels[i].weaponType == weaponType)
                weaponModel = weaponModels[i];
        }

        return weaponModel;
    }

    public void PlayReloadAnimation()
    {
        if (isGrabingWeapon)
            return;
        animator.SetTrigger("Reload");
        ReduceRigWeight();
        
    }

    private void UpdateLeftHandIKWeight()
    {
        if (shouldIncrease_LeftHandIKWeight)
        {
            leftHandIK.weight += leftHandIKWeightIncreaseRate * Time.deltaTime;

            if (leftHandIK.weight >= 1)
            {
                shouldIncrease_LeftHandIKWeight = false;
            }
        }
    }

    private void UpdateRigWeight()
    {
        if (shouldIncrease_RigWeight)
        {
            rig.weight += rigWeightIncreaseRate * Time.deltaTime;

            if (rig.weight >= 1)
            {
                shouldIncrease_RigWeight = false;
            }
            
        }
    }

    private void ReduceRigWeight()
    {
        rig.weight = 0;
    }

    private void PlayWeaponGrabAnimation(GrabType grabType)
    {
        leftHandIK.weight = 0;
        ReduceRigWeight();
        animator.SetFloat("Weapon Grab Type", ((float)grabType));
        animator.SetTrigger("Grab Weapon");

        SetBusyGrabWeaponTo(true);

    }

    public void SetBusyGrabWeaponTo(bool busy)
    {
        isGrabingWeapon = busy;
        animator.SetBool("BusyGrabWeapon", isGrabingWeapon);
    }

    public void MaximazeRigWeight() => shouldIncrease_RigWeight = true;
    public void MaximazeLeftHandWeight() => shouldIncrease_LeftHandIKWeight = true;




    private void SwitchOn()
    {
        SwitchOffWeaponModels();
        CurrentWeaponModel().gameObject.SetActive(true);

        AttachLeftHand();
    }

    private void SwitchOffWeaponModels()
    {
        for (int i = 0; i < weaponModels.Length; i++)
        {
            weaponModels[i].gameObject.SetActive(false);
        }
    }

    private void AttachLeftHand()
    {
        Transform targetTransform = CurrentWeaponModel().holdPoint; 

        leftHandIK_Target.localPosition = targetTransform.localPosition;
        leftHandIK_Target.localRotation = targetTransform.localRotation;
    }

    private void SwitchAnimationLayer(int indexLayer)
    {
        for (int i = 1; i < animator.layerCount; i++)
        {
            animator.SetLayerWeight(i, 0);
        }

        animator.SetLayerWeight(indexLayer, 1);
    }
    private void Che�kWeaponSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchOn();
            SwitchAnimationLayer(1);
            PlayWeaponGrabAnimation(GrabType.SideGrab);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchOn();
            SwitchAnimationLayer(1);
            PlayWeaponGrabAnimation(GrabType.SideGrab);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchOn();
            SwitchAnimationLayer(1);
            PlayWeaponGrabAnimation(GrabType.BackGrab);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchOn();
            SwitchAnimationLayer(2);
            PlayWeaponGrabAnimation(GrabType.BackGrab);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SwitchOn();
            SwitchAnimationLayer(1);
            PlayWeaponGrabAnimation(GrabType.BackGrab);
        }
    }
}

