using System;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public interface IInteraction
    {
        public Bounds Range { get; }
        public event Action OnStart;
        public event Action OnFail;
        public event Action OnSuccess;
        public void DoStart();
        public void DoFail();
        public void DoSuccess();
    }

    public static class InteractionSystem
    {
        private readonly static List<IInteraction> interactions = new();

        public static bool TryGetInteraction(Transform actor, out IInteraction interaction)
        {
            interaction = null;
            foreach (var item in interactions)
            {
                if (!item.Range.Contains(actor.position))
                {
                    continue;
                }
                interaction = item;
                return true;
            }
            return false;
        }
        public static void AddInteraction(IInteraction interaction)
        {
            interactions.Add(interaction);
        }
        public static void RemoveInteraction(IInteraction interaction)
        {
            interactions.Remove(interaction);
        }
    }

}
