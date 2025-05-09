using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LiftCylinder : MonoBehaviour
{
    [Header("Rise Settings")]
    [Tooltip("How far (in world units) the cylinder will rise")]
    public float riseHeight = 10f;
    [Tooltip("How long (in seconds) the rise animation takes")]
    public float riseDuration = 2f;

    private bool playerInRange = false;
    private bool isRising       = false;
    private bool hasRisen       = false;

    private Vector3 startPos;
    private Vector3 targetPos;

    void Awake()
    {
        startPos  = transform.position;
        targetPos = startPos + Vector3.up * riseHeight;
    }

    void Update()
    {
        if (playerInRange 
         && Input.GetKeyDown(KeyCode.E) 
         && !isRising 
         && !hasRisen)
        {
            StartCoroutine(Rise());
        }
    }

    private IEnumerator Rise()
    {
        isRising = true;
        float elapsed = 0f;

        while (elapsed < riseDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / riseDuration);
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        transform.position = targetPos;
        isRising   = false;
        hasRisen   = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}