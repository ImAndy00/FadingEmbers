using UnityEngine;
using System.Collections;

public class LootFloater : MonoBehaviour
{
    private float floatSpeed;
    private float floatDuration;

    public void Initialize(float speed, float duration)
    {
        floatSpeed     = speed;
        floatDuration  = duration;
        StartCoroutine(FloatUp());
    }

    private IEnumerator FloatUp()
    {
        float elapsed = 0f;
        Vector3 start = transform.position;
        while (elapsed < floatDuration)
        {
            elapsed += Time.deltaTime;
            transform.position += Vector3.up * floatSpeed * Time.deltaTime;
            yield return null;
        }
        Destroy(this); 
    }
}