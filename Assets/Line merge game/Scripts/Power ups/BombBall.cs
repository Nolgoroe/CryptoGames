using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBall : BallBase
{
    [Header("Detection Stats")]
    [SerializeField] LayerMask layerToDetect;
    [SerializeField] float radius = 5;

    [Header("Bomb Stats")]
    [SerializeField] public float bombPower = 7500;
    [SerializeField] public bool isExploding;
    [SerializeField] public float timeToExplode = 4;

    private void Start()
    {
        base.Start();
        isExploding = true; //temp until decide on trigger
    }

    private void Update()
    {
        if (isExploding)
        {
            timeToExplode -= Time.deltaTime;

            if (timeToExplode < 0)
            {
                isExploding = false;
                Explode();
            }
        }
    }

    private void Explode()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, layerToDetect);
        foreach (Collider2D hit in colliders)
        {
            if (hit.gameObject == gameObject) continue;

            Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                Vector2 distanceVec = hit.transform.position - transform.position;
                if (distanceVec.magnitude > 0)
                {
                    float explodeForce = bombPower;
                    rb.AddForce((distanceVec.normalized * explodeForce), ForceMode2D.Impulse);
                }
            }
        }

        Destroy(gameObject);
    }
}
