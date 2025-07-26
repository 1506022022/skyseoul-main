using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using System.Linq;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Radar", story: "Activate [radar] for alert [level] than get [target]", category: "Action/Character", id: "023401229b4a11765d89a6310f4bab22")]
public partial class RadarAction : Action
{
    [SerializeReference] public BlackboardVariable<Transform> Radar, Target;
    [SerializeReference] public BlackboardVariable<int> Level;
    [SerializeReference] public BlackboardVariable<int> outputLevel;

    RadarComponent radar;
    protected override Status OnStart()
    {
        if (!(Radar.Value?.TryGetComponent(out radar) ?? false)) return Status.Failure;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (radar is null) return Status.Failure;
        var targets = radar.SearchRadar();

        var info = targets.Where(x => !x.Block).FirstOrDefault();
        if (info.Equals(RadaredInfo.None))
        {
            Target.Value = null;
            outputLevel.Value = radar.AlertLevel();
        }
        else
        {
            if (info.DistanceLevel <= Level.Value) radar.SelectTarget(info.Transform);
            Target.Value = info.Transform;
            outputLevel.Value = info.DistanceLevel;
        }
        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}