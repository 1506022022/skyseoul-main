using Battle;
using UnityEngine;
using static Unity.Behavior.Node;

namespace BehaviorTree
{
    public abstract class Do : ScriptableObject
    {
        public abstract Status OnUpdate(IActor actor);
        public abstract Status OnStart(IActor actor);
        public abstract Status OnEnd(IActor actor);
    }
}