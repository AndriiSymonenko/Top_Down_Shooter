using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    private Player player;

    [Header("Bullet seting")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Transform firePoint;

    [SerializeField] private Transform weaponHolder;
    [SerializeField] private Transform aim;


    private void Start()
    {
        player = GetComponent<Player>();
        player.playerControll.Player.Shoot.performed += context => Shoot();
    }
    private void Shoot()
    {


        GameObject bulletPrefab = Instantiate(bullet, firePoint.position, Quaternion.LookRotation(firePoint.forward));
        bulletPrefab.GetComponent<Rigidbody>().velocity = BulletDirection() * bulletSpeed;
        Destroy(bulletPrefab, 10f);

        GetComponentInChildren<Animator>().SetTrigger("Shoot");
    }

    private Vector3 BulletDirection()
    {


        Vector3 direction = (aim.position - firePoint.position).normalized;

        if (!player.playerAim.CanAimPrecisly())
            direction.y = 0;


        weaponHolder.LookAt(aim);
        firePoint.LookAt(aim);

        return direction;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(weaponHolder.position, weaponHolder.position + weaponHolder.forward * 25);
        Gizmos.color = Color.yellow;

        Gizmos.DrawLine(firePoint.position, firePoint.position + BulletDirection() * 25);
    }
}
