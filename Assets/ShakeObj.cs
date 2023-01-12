using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeObj : MonoBehaviour
{
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private float durationShaking = 0f;

    public static ShakeObj Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void StartShakingCam(float addTime)
    {
        StartCoroutine(Shaking(addTime));
    }

    private IEnumerator Shaking(float timeAdded)
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        var _addDuration = timeAdded + durationShaking;

        while (elapsedTime < _addDuration)
        {
            elapsedTime += Time.deltaTime;
            float strength = curve.Evaluate(elapsedTime / _addDuration);
            transform.position = startPosition + Random.insideUnitSphere * strength;
            yield return null;
        }

        transform.position = startPosition;
    }
}
