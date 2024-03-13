using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rigidbody => GetComponent<Rigidbody>();

    private void OnCollisionEnter(Collision collision)
    {
        rigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }
}
