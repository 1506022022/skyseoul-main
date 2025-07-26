using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Battle;
using Character;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Alert", story: "[Actor] [be] alerts", category: "Action/Character", id: "fa7db23248e2c24281f06f155f299469")]
public partial class AlertAction : Action
{
    [SerializeReference] public BlackboardVariable<Transform> Actor;
    [SerializeReference] public BlackboardVariable<bool> Be;

    protected override Status OnStart()
    {
        if (!(Actor.Value?.TryGetComponent(out IAlert alerter) ?? false)) return Status.Failure;

        if (Be.Value) alerter.Alert(); else alerter.Release();
        return Status.Success;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

