using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public struct RadaredInfo
{
    public static RadaredInfo None = new();
    public Transform Transform;
    public Transform Block;
    public Vector3 BlockPositon;
    public int DistanceLevel;
}

public class RadarComponent : MonoBehaviour
{
    List<RadaredInfo> targets = new();
    [SerializeField, Min(0.1f)] float maxDistance = 10f;
    [SerializeField, Range(0, 360)] float fieldOfView = 90f;
    [SerializeField] List<float> distances = new();
#if UNITY_EDITOR
    [SerializeField, GameObjectTag]
#endif
    public string TargetTag = "Player";
    [SerializeField] AnimationCurve thresholdMultiply;
    [SerializeField, Range(0, 1)] float threshold;

    public int AlertLevel()
    {
        if (threshold == 0 && targets.Count == 0) return -1;
        var copy = distances.ToList();
        var pivot = GetThresholdDistance()[0];
        copy.Add(pivot);
        copy.Sort();
        return copy.IndexOf(pivot);
    }

    List<float> GetThresholdDistance()
    {
        var copy = distances.ToList();
        for (int i = 0; i < copy.Count; i++) copy[i] = distances[i] + (maxDistance - distances[i]) * Mathf.Min(threshold, 1.0f);
        return copy;
    }

    public List<RadaredInfo> SearchRadar()
    {
        var copy = GetThresholdDistance();
        targets = Radar.SearchRadar(transform, TargetTag, fieldOfView, maxDistance, copy);
        return targets;
    }
    public void SelectTarget(Transform target)
    {
        if (targets.Find(x => x.Transform == target).Equals(RadaredInfo.None))
        {
            Debug.Log("Faild found target");
            return;
        }
        StopAllCoroutines();
        StartCoroutine(UpdateThreshold());
    }

    IEnumerator UpdateThreshold()
    {
        if (thresholdMultiply.keys.Length < 1) yield break;
        threshold = 0;
        float startTime = Time.time;
        float duration = thresholdMultiply.keys[^1].time - thresholdMultiply.keys[0].time;
        float endTime = startTime + duration;
        while (Time.time < endTime)
        {
            threshold = thresholdMultiply.Evaluate(duration - (endTime - Time.time));
            yield return null;
        }
        threshold = 0f;
    }

#if UNITY_EDITOR
    [SerializeField] bool drawAllways;
    private void OnDrawGizmosSelected()
    {
        if (!drawAllways) DrawRadar();
    }
    private void OnDrawGizmos()
    {
        if (drawAllways) DrawRadar();
    }

    public void DrawRadar()
    {
        if (!Application.isPlaying) SearchRadar();
        Radar.DrawRadar(transform, targets, fieldOfView, maxDistance);
    }

    void OnValidate()
    {
        if (distances.Count == 0)
        {
            distances.Add(maxDistance);
        }
        if (distances.Count > 5)
        {
            distances.RemoveRange(5, distances.Count - 5);
        }
        for (int i = 0; i < distances.Count - 1; i++)
        {
            distances[i] = Mathf.Min(maxDistance, distances[i]);
        }
        distances[^1] = maxDistance;
        distances.Sort();
    }
#endif
}