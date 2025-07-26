using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SearchEnemy", story: "[Actor] search [enemy]", category: "Action/Find", id: "20900d2dd2568ede291ebe606ef64954")]
public partial class SearchEnemy : Action
{
    [SerializeReference] public BlackboardVariable<Transform> Actor;
    [SerializeReference] public BlackboardVariable<string> Enemy;
    [SerializeReference] public BlackboardVariable<float> FOV; // 시야각
    [SerializeReference] public BlackboardVariable<AnimationCurve> Ear; // 청각 임계치
    [SerializeReference] public BlackboardVariable<float> VOD; // 시야거리
    // dpm
    // priority

    protected override Status OnStart()
    {
        
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

