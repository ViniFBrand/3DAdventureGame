using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    private Collider collider;

    public float timeTimeDestroy = 2f;

    public int damageAmount = 1;
    public float speed = 50f;

    public List<string> tagToHit;

    private void OnValidate()
    {
        if(collider == null) collider = GetComponent<Collider>();
    }

    private void Awake()
    {
        Destroy(gameObject, timeTimeDestroy);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        foreach(var t in tagToHit)
        {
            if(collision.transform.tag == t)
            {
                var damageable = collision.transform.GetComponent<IDamageable>();

                if (damageable != null)
                {
                    Vector3 dir = collision.transform.position - transform.position;
                    dir = -dir.normalized;
                    dir.y = 0;

                    damageable.Damage(damageAmount, dir);
                    if (collider != null) collider.enabled = false; //CHECK IF WORKING (BUG DOUBLE COLLISION)
                }
                break;
            }
        }
            Destroy(gameObject);
        
    }
}
