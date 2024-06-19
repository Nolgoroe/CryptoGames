using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBall : BallBase
{
    [Header("Detection Stats")]
    [SerializeField] LayerMask layerToDetect;
    [SerializeField] float radius = 5f;

    [Header("Bomb Stats")]
    [SerializeField] private float bombPower = 6000;
    [SerializeField] private bool isExploding;
    [SerializeField] private float timeToExplode = 0.5f;
    private float explosionDuration;

    Animator bombEffectPrefab;

    private void Start()
    {
        base.Start();
        isExploding = true; //temp until decide on trigger

        layerToDetect = (1 << 6); //flag hardcoded

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
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, layerToDetect);
        foreach (Collider hit in colliders)
        {
            //if (hit.gameObject == gameObject) continue;

            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
            {
                Vector2 distanceVec = hit.transform.position - transform.position;
                if (distanceVec.magnitude > 0)
                {
                    float explodeForce = bombPower;
                    rb.AddForce((distanceVec.normalized * explodeForce), ForceMode.Impulse);
                }
            }
        }

        Animator anim = Instantiate(bombEffectPrefab, transform);
        AnimatorClipInfo[] animInfo = anim.GetCurrentAnimatorClipInfo(0);
        float animatorSpeed = anim.GetCurrentAnimatorStateInfo(0).speed;
        explosionDuration = animInfo[0].clip.length / animatorSpeed;

        Destroy(anim.gameObject, explosionDuration);
        Destroy(this, explosionDuration);
    }

    public void SetBombEffect(Animator bombEffect)
    {
        bombEffectPrefab = bombEffect;
    }
}
