using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/OnInteger")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "OnInteger", message: "On [Integer]", category: "Events", id: "33051050234a1169fa9a1fe48ab686df")]
public sealed partial class OnInteger : EventChannel<int> { }

