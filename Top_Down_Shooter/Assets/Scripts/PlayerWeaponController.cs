using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    private const float REFERENCE_BULLET_SPEED = 20f;

    private Player player;

    [Header("Bullet seting")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Transform gunPoint;

    [SerializeField] private Transform weaponHolder;
    


    private void Start()
    {
        player = GetComponent<Player>();
        player.playerControll.Player.Shoot.performed += context => Shoot();
    }
    private void Shoot()
    {


        GameObject bulletPrefab = Instantiate(bullet, gunPoint.position, Quaternion.LookRotation(gunPoint.forward));

        Rigidbody rbBulletPrefab = bulletPrefab.GetComponent<Rigidbody>();

        rbBulletPrefab.mass = REFERENCE_BULLET_SPEED / bulletSpeed;
        rbBulletPrefab.velocity = BulletDirection() * bulletSpeed;
        Destroy(bulletPrefab, 10f);

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

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawLine(weaponHolder.position, weaponHolder.position + weaponHolder.forward * 25);
    //    Gizmos.color = Color.yellow;

    //    Gizmos.DrawLine(firePoint.position, firePoint.position + (BulletDirection() * 25));
    //}
}
