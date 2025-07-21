using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Battle;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "LookAtOther", story: "[Transform] looks at other [Target]", category: "Action", id: "e6469a4a2009f70db300ed5accf8d100")]
public partial class LookAtOther : Action
{
    [SerializeReference] public BlackboardVariable<Transform> Transform;
    [SerializeReference] public BlackboardVariable<Transform> Target;
    [Tooltip("True: the node process the LookAt every update with status Running." +
        "\nFalse: the node process the LookAt only once.")]
    [SerializeReference] public BlackboardVariable<bool> Continuous = new BlackboardVariable<bool>(false);
    [SerializeReference] public BlackboardVariable<bool> LimitToYAxis = new BlackboardVariable<bool>(false);

    protected override Status OnStart()
    {
        if (Transform.Value == null || Target.Value == null)
        {
            LogFailure($"Missing Transform or Target.");
            return Status.Failure;
        }

        ProcessLookAt();
        return Continuous.Value ? Status.Running : Status.Success;
    }

    protected override Status OnUpdate()
    {
        if (Continuous.Value)
        {
            ProcessLookAt();
            return Status.Running;
        }
        return Status.Success;
    }

    void ProcessLookAt()
    {
        var parentActor = Target.Value.GetComponentInParent<IActor>();
        if (parentActor != null && parentActor is IGameObject gameObject && gameObject.transform.Equals(Transform.Value))
        {
            return;
        }

        Vector3 targetPosition = Target.Value.position;

        if (LimitToYAxis.Value)
        {
            targetPosition.y = Transform.Value.position.y;
        }
        Transform.Value.LookAt(targetPosition, Transform.Value.up);
    }
}

