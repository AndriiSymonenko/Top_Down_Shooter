
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    private Player player;
    private const float REFERENCE_BULLET_SPEED = 20f;

    [SerializeField] private Weapon currentWeapon;
    

    [Header("Bullet seting")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Transform gunPoint;

    [SerializeField] private Transform weaponHolder;
    [SerializeField] private GameObject weaponFire;

    [Header("Inventory")]
    [SerializeField] private int maxSlots = 2;
    [SerializeField] private List<Weapon> weaponSlots;
    public Weapon CurrentWeapon() => currentWeapon;

    private void Start()
    {
        player = GetComponent<Player>();
        AssignWeaponEvents();

        Invoke("EquipStartingWeapon", .1f);
        //currentWeapon.bulletsInMagazine = currentWeapon.totalReserveAmmo;
    }


    private void EquipStartingWeapon()
    {
        EquipWeapon(0);
    }

    #region Slots menegment weapon
    private void EquipWeapon(int i)
    {
        currentWeapon = weaponSlots[i];

        player.weaponVisuals.PlayWeaponEquipAnimation();
        

    }

    public void PickupWeapon(Weapon newWeapon)
    {
        if (weaponSlots.Count >= maxSlots)
        {
            return;
        }

        weaponSlots.Add(newWeapon);

        player.weaponVisuals.SwitchOnBackupWeaponMode();
    }

    private void DropWeapon()
    {
        if (HasOnlyOneWeapon())
            return;


        weaponSlots.Remove(currentWeapon);

        EquipWeapon(0);

    }
    #endregion

    public bool HasOnlyOneWeapon() => weaponSlots.Count <= 1;

    private void Shoot()
    {


        if (currentWeapon.CanShoot() == false)
            return;

        GameObject bulletPrefab = ObjectPool.instance.GetBullet();
 
        bulletPrefab.transform.position = gunPoint.position;
        bulletPrefab.transform.rotation = Quaternion.LookRotation(gunPoint.forward);
    GameObject fireWeaponFX = Instantiate(weaponFire, gunPoint.position, Quaternion.identity);

        Rigidbody rbBulletPrefab = bulletPrefab.GetComponent<Rigidbody>();

        rbBulletPrefab.mass = REFERENCE_BULLET_SPEED / bulletSpeed;
        rbBulletPrefab.velocity = BulletDirection() * bulletSpeed;
        Destroy(fireWeaponFX, 1f);

        GetComponentInChildren<Animator>().SetTrigger("Shoot");
    }

    public Vector3 BulletDirection()
    {
        Transform aim = player.playerAim.Aim();

        Vector3 direction = (aim.position - gunPoint.position).normalized;

        if (player.playerAim.CanAimPrecisly() == false && player.playerAim.Target() == null)
        {
            direction.y = 0;
        }

        //weaponHolder.LookAt(aim); Reload animation is broken, if this method on.
        //gunPoint.LookAt(aim);

        return direction;
    }

   
    public Transform GunPoint() => gunPoint;

    public Weapon BackupWeapon()
    {
        foreach (Weapon weapon in weaponSlots)
        {
            if (weapon != currentWeapon)
                return weapon;
        }

        return null;
    }

    #region Input Event
    private void AssignWeaponEvents()
    {
        PlayerControll playerControll = player.playerControll;

        playerControll.Player.Shoot.performed += context => Shoot();

        playerControll.Player.EquipSlot1.performed += context => EquipWeapon(0);
        playerControll.Player.EquipSlot2.performed += context => EquipWeapon(1);

        playerControll.Player.DropCurrentWeapon.performed += context => DropWeapon();

        playerControll.Player.Reload.performed += context =>
        {
            if (currentWeapon.CanReload())
            {
                player.weaponVisuals.PlayReloadAnimation();
            }
        };

    }

    #endregion

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawLine(weaponHolder.position, weaponHolder.position + weaponHolder.forward * 25);
    //    Gizmos.color = Color.yellow;

    //    Gizmos.DrawLine(firePoint.position, firePoint.position + (BulletDirection() * 25));
    //}
}
