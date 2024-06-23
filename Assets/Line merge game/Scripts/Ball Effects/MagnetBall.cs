using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetBall : MonoBehaviour
{    
    [Header("Detection Stats")]
    [SerializeField] LayerMask layerToDetect;
    [SerializeField] float radius = 50f;

    [Header("magnit Stats")]
    [SerializeField] private float magnitPower = 450;
    [SerializeField] private bool isMagnetizing;
    [SerializeField] private float timeToMagnetize = 3;

    [Header("Live Data")]
    [SerializeField] Rigidbody ballToAttract;

    BallBase thisBall;

    private void Start()
    {
        transform.TryGetComponent<BallBase>(out thisBall);
        if (!thisBall) return;

        isMagnetizing = true;

        layerToDetect = (1 << 6); //flag hardcoded

        ChooseBallInRadius();
    }

    private void ChooseBallInRadius()
    {
        //choose ball in radius

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, layerToDetect);

        float minDistance = Mathf.Infinity;
        Vector3 ballPos = transform.position;

        Collider chosenCollider = null;
        foreach (Collider hit in colliders)
        {
            hit.TryGetComponent<BallBase>(out BallBase otherball);
            if (!otherball || otherball.ReturnBallIndex() != thisBall.ReturnBallIndex()) continue;

            if (hit.transform == transform) continue;

            float distance = Vector3.Distance(hit.transform.position, ballPos);
            if (distance < minDistance)
            {
                chosenCollider = hit;
                minDistance = distance;
            }
        }

        if (chosenCollider)
            chosenCollider.transform.TryGetComponent<Rigidbody>(out ballToAttract);
    }

    private void Update()
    {
        if (isMagnetizing)
        {
            timeToMagnetize -= Time.deltaTime;

            MagnetizeAction();

            if (timeToMagnetize < 0)
            {
                isMagnetizing = false;

                Destroy(this);
            }
        }
    }

    private void MagnetizeAction()
    {
        if (!ballToAttract) return;


        Vector3 direction = transform.position - ballToAttract.transform.position;
        float distance = direction.magnitude;

        float magnetizationForceMagnitude = (magnitPower * ballToAttract.mass) / distance;

        Vector3 force = direction.normalized * magnetizationForceMagnitude; // we normalize since we want the -1 or 1 direction that we apply on the rigidbody

        ballToAttract.AddForce(force);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
