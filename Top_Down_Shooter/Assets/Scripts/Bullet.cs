using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject bulletImpactFX;
    private Rigidbody rigidbody => GetComponent<Rigidbody>();


    private void OnCollisionEnter(Collision collision)
    {
        CreateBulletImpactFX(collision);
        Destroy(gameObject, .01f);
    }

    private void CreateBulletImpactFX(Collision collision)
    {
        if (collision.contacts.Length > 0)
        {
            ContactPoint contact = collision.contacts[0];

            GameObject newBulletImpactFX = Instantiate(bulletImpactFX, contact.point, Quaternion.LookRotation(contact.normal));
            Destroy(newBulletImpactFX, 1f);
        }
    }
}
