using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerWeaponVisuals : MonoBehaviour
{
    private Player player;
    private Animator animator;
    private bool isEquipWeapon;




    [SerializeField] private WeaponModel[] weaponModels;
    [SerializeField] private BackupWeaponModel[] backupWeaponModels;

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
        backupWeaponModels = GetComponentsInChildren<BackupWeaponModel>(true);
        weaponModels = GetComponentsInChildren<WeaponModel>(true);

       
    }

    private void Update()
    {
        Debug.Log((int)CurrentWeaponModel().holdType);
       
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
        if (isEquipWeapon)
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

    public void PlayWeaponEquipAnimation()
    {
        EquipType equipType = CurrentWeaponModel().equipAnimationType;

        leftHandIK.weight = 0;
        ReduceRigWeight();
        animator.SetFloat("Weapon Equip Type", ((float)equipType));
        animator.SetTrigger("EquipWeapon");

        SetBusyGrabWeaponTo(true);

    }

    public void SetBusyGrabWeaponTo(bool busy)
    {
        isEquipWeapon = busy;
        animator.SetBool("BusyEquipWeapon", isEquipWeapon);
    }

    public void MaximazeRigWeight() => shouldIncrease_RigWeight = true;
    public void MaximazeLeftHandWeight() => shouldIncrease_LeftHandIKWeight = true;




    public void SwitchOnCurrentWeaponModel()
    {

        int animationIndex = ((int)CurrentWeaponModel().holdType);
        Debug.Log(animationIndex);

        SwitchOffWeaponModels();
        SwitchOffBackupWeaponModel();
        


        if (player.weapon.HasOnlyOneWeapon() == false)
        {
            SwitchOnBackupWeaponMode();
        }


        SwitchAnimationLayer(animationIndex);

        CurrentWeaponModel().gameObject.SetActive(true);

        AttachLeftHand();
    }

    public void SwitchOffWeaponModels()
    {
        for (int i = 0; i < weaponModels.Length; i++)
        {
            weaponModels[i].gameObject.SetActive(false);
        }
    }

    private void SwitchOffBackupWeaponModel()
    {
        foreach (BackupWeaponModel backupWeaponModel in backupWeaponModels)
        {
            backupWeaponModel.gameObject.SetActive(false);
        }
    }

    public void SwitchOnBackupWeaponMode()
    {
        WeaponType weaponType = player.weapon.BackupWeapon().weaponType;

        foreach (BackupWeaponModel backupWeaponModel in backupWeaponModels)
        {
            if (backupWeaponModel.weaponType == weaponType)
                backupWeaponModel.gameObject.SetActive(true);
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

}

