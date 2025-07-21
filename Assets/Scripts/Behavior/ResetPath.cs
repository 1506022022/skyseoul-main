using System;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ResetPath", story: "[agent] reset path", category: "Action/Navigation", id: "25a4efcc013e823d5b5207c1f837e5f9")]
public partial class ResetPath : Action
{
    [SerializeReference] public BlackboardVariable<NavMeshAgent> Agent;

    protected override Status OnStart()
    {
        Agent.Value.ResetPath();
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

