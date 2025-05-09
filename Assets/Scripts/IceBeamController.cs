using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class IceBeamController : MonoBehaviour
{
    [Header("Beam Settings")]
    [Tooltip("World distance when localScale.z == 1")]
    public float maxDistance = 18f;

    [Tooltip("Layers the beam can hit for clipping (e.g. walls)")]
    public LayerMask clipLayers;

    [Header("Damage Settings")]
    [Tooltip("Layers that count as enemies")]
    public LayerMask enemyLayers;
    [Tooltip("Damage dealt each tick")]
    public int damagePerTick = 2;
    [Tooltip("Seconds between damage ticks")]
    public float tickInterval = 0.5f;

    private float _initialScaleZ;
    private BoxCollider _collider;
    private Vector3 _initialColliderSize, _initialColliderCenter;
    private float _nextTickTime;

    void Awake()
    {
        _initialScaleZ = transform.localScale.z;
        _collider = GetComponent<BoxCollider>();
        _initialColliderSize   = _collider.size;
        _initialColliderCenter = _collider.center;
    }

    void Update()
    {
        // 1) Determine beam length via raycast against world
        Vector3 origin = transform.position + transform.forward * 0.01f;
        Vector3 dir = transform.forward;
        float worldLength;
        if (Physics.Raycast(origin, dir, out var hit, maxDistance, clipLayers))
            worldLength = hit.distance;
        else
            worldLength = maxDistance;

        ApplyLength(worldLength);

        // 2) Damage any enemy in the beam, on tick interval
        if (Time.time >= _nextTickTime)
        {
            if (Physics.Raycast(origin, dir, out var eHit, worldLength, enemyLayers))
            {
                var enemy = eHit.collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damagePerTick);
                    _nextTickTime = Time.time + tickInterval;
                }
            }
        }
    }

    private void ApplyLength(float worldLength)
    {
        // Scale the beam visual
        float scaleZ = worldLength / maxDistance * _initialScaleZ;
        var ls = transform.localScale;
        ls.z = scaleZ;
        transform.localScale = ls;

        // Adjust collider so size.z * scaleZ == worldLength
        float localLen = worldLength / transform.localScale.z;
        var cs = _initialColliderSize;
        cs.z = localLen;
        _collider.size = cs;

        var cc = _initialColliderCenter;
        cc.z = localLen * 0.5f;
        _collider.center = cc;
    }
}