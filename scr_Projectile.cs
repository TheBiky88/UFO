using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Projectile : MonoBehaviour
{
    [SerializeField] private Transform planet = null;
    [SerializeField] private GameObject owner = null;
    [SerializeField] private float damage = 0f;
    [SerializeField] private float speed = 0f;
    [SerializeField] private float m_LifeTime = 0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != owner)
        {
            other.GetComponent<scr_PlayerControl>().Damage(damage);

            if (gameObject.GetComponent<scr_Explosion>() != null)
            {
                gameObject.GetComponent<scr_Explosion>().Explode();
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    void Update()
    {
        if (m_LifeTime > 0)
        {
            m_LifeTime -= Time.deltaTime;
        }
        else
        {
            if (gameObject.GetComponent<scr_Explosion>() != null)
            {

              gameObject.GetComponent<scr_Explosion>().Explode();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        if (planet != null)
        {
            transform.RotateAround(planet.position, transform.right, speed * Time.deltaTime);
        }
        else
        {
            transform.position += transform.up * speed * 2 * Time.deltaTime;
        }
    }

    public void Instantiation(Transform planet, GameObject owner, float damage, float speed, float lifeTime)
    {
        this.planet = planet;
        this.owner = owner;
        this.damage = damage;
        this.speed = speed;
        this.m_LifeTime = lifeTime;
    }
}
