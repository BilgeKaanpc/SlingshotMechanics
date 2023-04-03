using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private float radius = 10f;
    private float force = 300;
    [SerializeField] GameObject explosionEffect;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var surroundingObjects = Physics.OverlapSphere(transform.position, radius);

            foreach (var item in surroundingObjects)
            {
                var rb = item.GetComponent<Rigidbody>();
                if (rb == null) continue;
                rb.AddExplosionForce(force, transform.position, radius);
            }
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
