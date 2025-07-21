using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/OnEvent")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "OnEvent", message: "OnEvent", category: "Events", id: "0b9f76b3a0dac49e938bd29c5e2820e4")]
public sealed partial class OnEvent : EventChannel { }

