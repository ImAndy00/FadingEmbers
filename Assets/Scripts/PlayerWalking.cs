using UnityEngine;

public class PlayerWalking : MonoBehaviour
{
    [Header("Assign your limb spheres (GameObjects)")]
    public GameObject leftArmGO;
    public GameObject rightArmGO;
    public GameObject leftLegGO;
    public GameObject rightLegGO;

    [Header("Animation Parameters")]
    public float swingAmplitude  = 30f;    // degrees
    public float swingFrequency  = 4f;     // swings per second
    public float idleBobAmplitude = 0.05f; // units
    public float idleBobFrequency = 1f;    // bobs per second
    public float walkThreshold    = 0.01f; // min movement (units/frame) to count as walking

    private Vector3 _lastPosition;
    private Vector3 _initialBodyPos;

    private Vector3 _initialLeftFootPos;
    private Vector3 _initialRightFootPos;

    void Start()
    {
        _initialLeftFootPos = leftLegGO.transform.localPosition;
        _initialRightFootPos = rightLegGO.transform.localPosition;
        _initialBodyPos = transform.localPosition;
    }

    void Awake()
    {
        _initialBodyPos = transform.localPosition;
        _lastPosition   = transform.position;
    }

    void Update()
{
    // 1) Compute how far we moved since last frame
    float distanceThisFrame = (transform.position - _lastPosition).magnitude;
    _lastPosition = transform.position;

    bool isWalking = distanceThisFrame > walkThreshold;

    if (isWalking)
    {
        // Make arm swinging slower
        float phase = Time.time * (swingFrequency * 0.5f) * Mathf.PI * 2f; // 50% slower
        float armAngle = Mathf.Sin(phase) * swingAmplitude;

        // Move feet forward/back rather than rotating
        float footOffset = Mathf.Sin(phase + Mathf.PI) * 0.1f; // adjust 0.1f as needed

        // Arm rotation
        leftArmGO .transform.localRotation = Quaternion.Euler( armAngle, 0, 0);
        rightArmGO.transform.localRotation = Quaternion.Euler(-armAngle, 0, 0);

        // Feet movement (forward and back in local Z axis)
        leftLegGO .transform.localPosition = _initialLeftFootPos  + Vector3.forward * footOffset;
        rightLegGO.transform.localPosition = _initialRightFootPos - Vector3.forward * footOffset;

        // Keep body steady
        transform.localPosition = _initialBodyPos;
    }
    else
    {
        // Reset arm rotation
        leftArmGO .transform.localRotation = Quaternion.identity;
        rightArmGO.transform.localRotation = Quaternion.identity;

        // Reset foot position
        leftLegGO .transform.localPosition = _initialLeftFootPos;
        rightLegGO.transform.localPosition = _initialRightFootPos;
    }
}
}
