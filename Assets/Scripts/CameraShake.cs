using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public bool StartShaking = false;
    public AnimationCurve Curve;
    public float Duration = 1f;

    public float X;
    public float Y;
    public float Z;

    void Update()
    {
        if (StartShaking)
        {
            StartShaking = false;
            StartCoroutine(Shaking());
        }
    }

    private IEnumerator Shaking()
    {
        Debug.Log("Start shaking");
        Vector3 startRotation = transform.eulerAngles;
        float elapsedTime = 0f;

        while (elapsedTime < Duration)
        {
            elapsedTime += Time.deltaTime;
            //float strength = Curve.Evaluate(elapsedTime / Duration);
            //transform.position = startedPosition + Random.insideUnitSphere * strength;
            transform.eulerAngles = new Vector3(X, Y, Z);
            yield return null;
        }

        transform.eulerAngles = startRotation;
    }

    //private IEnumerator Shaking()
    //{
    //    Debug.Log("Start shaking");
    //    Vector3 startedPosition = transform.position;
    //    float elapsedTime = 0f;

    //    while (elapsedTime < Duration)
    //    {
    //        elapsedTime += Time.deltaTime;
    //        float strength = Curve.Evaluate(elapsedTime / Duration);
    //        transform.position = startedPosition + Random.insideUnitSphere * strength;
    //        yield return null;
    //    }

    //    transform.position = startedPosition;
    //}
}
