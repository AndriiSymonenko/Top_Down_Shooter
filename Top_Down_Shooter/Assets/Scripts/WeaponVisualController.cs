using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class WeaponVisualController : MonoBehaviour
{

    private Animator animator;

    [SerializeField] private Transform[] gunTransforms;

    [SerializeField] private Transform pistol;
    [SerializeField] private Transform revolver;
    [SerializeField] private Transform autoRifle;
    [SerializeField] private Transform shotgun;
    [SerializeField] private Transform sniperRifle;

    private Transform currentGun;
    private Rig rig;

    [Header("Rig")]
    [SerializeField] private float rigIncreaseStep;
    private bool rigShouldBeIncreased;

    [Header("Left Hand IK")]
    [SerializeField] TwoBoneIKConstraint leftHandIK;
    [SerializeField] private Transform leftHandIK_Target;
    [SerializeField] private float leftHandIKIncreaseStep;
    private bool leftHandWeightIncrease;


    private bool busyGrabWeapon;


    private void Start()
    {
        rig = GetComponentInChildren<Rig>();
        animator = GetComponentInChildren<Animator>();

        SwitchOn(pistol);
    }

    private void Update()
    {
        CheskWeaponSwitch();

        if (Input.GetKeyDown(KeyCode.R) && busyGrabWeapon == false)
        {
            animator.SetTrigger("Reload");
            PauseRig();
        }

        UpdateRigWeight();

        UpdateLeftHandIKWeight();
    }

    private void UpdateLeftHandIKWeight()
    {
        if (leftHandWeightIncrease)
        {
            leftHandIK.weight += leftHandIKIncreaseStep * Time.deltaTime;

            if (leftHandIK.weight >= 1)
            {
                leftHandWeightIncrease = false;
            }
        }
    }

    private void UpdateRigWeight()
    {
        if (rigShouldBeIncreased)
        {
            rig.weight += rigIncreaseStep * Time.deltaTime;

            if (rig.weight >= 1)
            {
                rigShouldBeIncreased = false;
            }
        }
    }

    private void PauseRig()
    {
        rig.weight = 0;
    }

    private void PlayWeaponGrabAnimation(GrabType grabType)
    {
        leftHandIK.weight = 0;
        PauseRig();
        animator.SetFloat("Weapon Grab Type", ((float)grabType));
        animator.SetTrigger("Grab Weapon");

        SetBusyGrabWeaponTo(true);

    }

    public void SetBusyGrabWeaponTo(bool busy)
    {
        busyGrabWeapon = busy;
        animator.SetBool("BusyGrabWeapon", busyGrabWeapon);
    }

    public void ReturnRigWeigthToOne() => rigShouldBeIncreased = true;
    public void ReturnWeightToLeftHand() => leftHandWeightIncrease = true;




    private void SwitchOn(Transform gunTransform)
    {
        SwitchOffGuns();
        gunTransform.gameObject.SetActive(true);
        currentGun = gunTransform;

        AttachLeftHand();
    }

    private void SwitchOffGuns()
    {
        for (int i = 0; i < gunTransforms.Length; i++)
        {
            gunTransforms[i].gameObject.SetActive(false);
        }
    }

    private void AttachLeftHand()
    {
        Transform targetTransform = currentGun.GetComponentInChildren<LeftHandTargetTransform>().transform;

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
    private void CheskWeaponSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchOn(pistol);
            SwitchAnimationLayer(1);
            PlayWeaponGrabAnimation(GrabType.SideGrab);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchOn(revolver);
            SwitchAnimationLayer(1);
            PlayWeaponGrabAnimation(GrabType.SideGrab);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchOn(autoRifle);
            SwitchAnimationLayer(1);
            PlayWeaponGrabAnimation(GrabType.BackGrab);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchOn(shotgun);
            SwitchAnimationLayer(2);
            PlayWeaponGrabAnimation(GrabType.BackGrab);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SwitchOn(sniperRifle);
            SwitchAnimationLayer(1);
            PlayWeaponGrabAnimation(GrabType.BackGrab);
        }
    }
}

public enum GrabType { SideGrab, BackGrab };