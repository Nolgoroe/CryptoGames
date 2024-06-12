using System.Collections;
using UnityEngine;

public class ShakeObject : MonoBehaviour
{
    [Header("Shake Preset Data")]
    [SerializeField] private float shakePower = 10;
    [SerializeField] private float timeEndShake = 1;

    [Header("Limits")]
    [SerializeField] private bool limitX;
    [SerializeField] private bool limitY;
    [SerializeField] private bool limitZ;

    [Header("Shake Live Data")]
    [SerializeField] private bool isShaking;

    Vector3 originPos;

    private void Start()
    {
        originPos = transform.position;
    }

    private void Update()
    {
        if (isShaking)
        {
            Vector3 newPos = originPos + Random.insideUnitSphere * (shakePower);

            if (limitX)
                newPos.x = transform.position.x;

            if (limitY)
                newPos.y = transform.position.y;

            if (limitZ)
                newPos.z = transform.position.z;

            transform.position = newPos;
        }
    }

    [ContextMenu("Now")]
    public void CallShake()
    {
        StartCoroutine(ShakeNow());
    }

    IEnumerator ShakeNow()
    {
        if (!isShaking)
        {
            isShaking = true;
        }

        yield return new WaitForSeconds(timeEndShake);

        ResetData();
    }

    private void ResetData()
    {
        isShaking = false;
        transform.position = originPos;
    }

    public float ReturnTimeEndShake()
    {
        return timeEndShake;
    }
}
