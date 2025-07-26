using UnityEngine;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

public static class Radar
{
    static readonly Color[] colorLevel = { Color.red, Color.orangeRed, Color.yellowNice, Color.forestGreen, Color.gray, Color.gray1 };
    static readonly GUIStyle textStyle = new GUIStyle() { normal = new GUIStyleState() { textColor = Color.white } };
    static readonly Dictionary<float, List<Transform>> targetByTagCache = new();
    static readonly RaycastHit[] hits = new RaycastHit[5];
    public static List<RadaredInfo> SearchRadar(Transform transform, string targetTag, float fieldOfView, float maxDistance, List<float> distances = null)
    {
        distances ??= new() { maxDistance };

        if (!targetByTagCache.TryGetValue(Time.time, out List<Transform> allTargets))
        {
            GameObject[] targetsByTag = GameObject.FindGameObjectsWithTag(targetTag);
            allTargets = targetsByTag.Select(x => x.transform).ToList();
            targetByTagCache.Clear();
            targetByTagCache.Add(Time.time, allTargets);
        }

        var targets = new List<RadaredInfo>();
        Vector3 origin = transform.position;
        Vector3 forward = transform.forward;
        Vector3 normal = transform.up;

        foreach (var target in allTargets)
        {
            if (target.root == transform.root) continue;

            Vector3 toTarget = target.position - origin;
            float distance = toTarget.magnitude;
            if (distance > maxDistance) continue;

            Vector3 projected = Vector3.ProjectOnPlane(toTarget, normal).normalized;
            float angle = Vector3.Angle(forward, projected);
            if (angle > fieldOfView * 0.5f) continue;

            var radaredInfo = new RadaredInfo() { Transform = target.transform};
            var distanceLevel = distances.ToList();
            distanceLevel.Add(distance);
            distanceLevel.Sort();
            radaredInfo.DistanceLevel = distanceLevel.IndexOf(distance);

            var ray = new Ray(transform.position, toTarget.normalized * maxDistance);
            var castCount = Physics.RaycastNonAlloc(ray, hits, distance);
            RaycastHit validHit = hits.ToList()
                .GetRange(0, castCount)
                .Where(h => h.transform.root != transform.root)
                .OrderBy(h => h.distance)
                .FirstOrDefault();

            if (validHit.transform != null && validHit.transform.root != target.root)
            {
                radaredInfo.Block = validHit.transform;
                radaredInfo.BlockPositon = validHit.point;
            }

            targets.Add(radaredInfo);
        }

        targets = targets.OrderBy(t => t.DistanceLevel).ToList();
        return targets;
    }

    public static void DrawRadar(Transform transform, List<RadaredInfo> radaredTargets, float fieldOfView, float maxDistance)
    {
#if UNITY_EDITOR
        Vector3 from = (Quaternion.AngleAxis(-fieldOfView / 2, Vector3.up) * transform.forward) * maxDistance;
        Vector3 to = (Quaternion.AngleAxis(fieldOfView / 2, Vector3.up) * transform.forward) * maxDistance;
        Handles.color = Color.cyan;
        Handles.DrawLine(transform.position, transform.position + from);
        Handles.DrawLine(transform.position, transform.position + to);
        Handles.DrawWireArc(transform.position, transform.up, from, fieldOfView, maxDistance);

        foreach (var target in radaredTargets)
        {
            Vector3 p1 = transform.position;
            Gizmos.color = Color.blue;
            if (target.Block)
            {
                p1 = target.BlockPositon;
                Gizmos.DrawLine(transform.position, target.BlockPositon);
            }

            Gizmos.color = colorLevel[Mathf.Clamp(target.DistanceLevel, 0, 4)];
            Gizmos.DrawLine(p1, target.Transform.position);

            float distance = Vector3.Distance(transform.position, target.Transform.position);
            Vector3 midpoint = (transform.position + target.Transform.position) * 0.5f;
            Handles.Label(midpoint + Vector3.up * 0.3f, $"{distance:F1}m", textStyle);
        }
#endif
    }
}