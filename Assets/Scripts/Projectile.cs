using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Damage;
    public float LifeTime;

    public bool IsPlayerShot { private get; set; }
    
    public float Speed { get; set; }

    private void Start()
    {
        Destroy(gameObject, LifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other.name + " hit");
        if(IsPlayerShot && other.CompareTag("Enemy"))
        {
            IDamageable damaged = other.gameObject.GetComponent<IDamageable>();
            if(damaged != null)
                damaged.onHit(Damage, transform.position, other.transform.position - transform.position);
        }
        else if(!IsPlayerShot && other.CompareTag("Player"))
        {
            IDamageable damaged = other.gameObject.GetComponent<IDamageable>();
            if (damaged != null)
                damaged.onHit(Damage, transform.position, other.transform.position - transform.position);
        }
        else
        {
            IDamageable damaged = other.gameObject.GetComponent<IDamageable>();
            if (damaged != null)
                damaged.onHit(Damage, transform.position, other.transform.position - transform.position);
        }
        Destroy(gameObject);
    }
}
