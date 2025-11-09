using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using BehaviorTree;
using Battle;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Do", story: "[What]", category: "Action", id: "2b344ea50bf85f3e6786240de98e327e")]
public partial class What : Action
{
    [SerializeReference] public BlackboardVariable<Do> what;

    protected override Status OnStart()
    {
        if (!GameObject.TryGetComponent<IActor>(out var actor))
        {
            return Status.Failure;
        }

        return what.Value.OnStart(actor);
    }

    protected override Status OnUpdate()
    {
        if (!GameObject.TryGetComponent<IActor>(out var actor))
        {
            return Status.Failure;
        }

        return what.Value.OnUpdate(actor);
    }

    protected override void OnEnd()
    {
        if (GameObject.TryGetComponent<IActor>(out var actor))
        {
            what.Value.OnEnd(actor);
        }
    }
}