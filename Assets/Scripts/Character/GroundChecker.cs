#if UNITY_EDITOR
using UnityEditor;
#endif
using Character;
using UnityEngine;

[ExecuteInEditMode]
public class GroundChecker : MonoBehaviour, IGroundCheckable
{
    readonly RaycastHit[] hits = new RaycastHit[5];
    [SerializeField] Vector3 offset;
    [SerializeField] float distance = 0.1f, radius = 0.1f;
    [SerializeField] LayerMask groundMask = 1 << 7;
    public bool IsGrounded { get; private set; }
    Ray Ray => new(offset + transform.position, Vector3.down);

    void Update()
    {
        var hitCount = Physics.SphereCastNonAlloc(Ray, radius, hits, distance, groundMask);
        IsGrounded = hitCount != 0;
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        var target = IsGrounded ? transform.position : Ray.origin + Ray.direction * distance;
        GUIStyle font = new() { fontSize = 20 };
        font.normal.textColor = Color.black;
        Handles.Label(transform.position, "from", font);
        Gizmos.DrawWireSphere(transform.position, radius);

        font.contentOffset = Vector3.up * 20;
        Handles.Label(target, "to", font);
        Gizmos.color = IsGrounded ? Color.red : Color.yellow;
        Gizmos.DrawLine(transform.position, target);
        Gizmos.DrawSphere(target, radius);
    }
#endif
}