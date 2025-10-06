using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/NextStage")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "NextStage", message: "Agent Has Started Order", category: "Events", id: "ae732fa7c5671deb739f1dfa6367b0af")]
public sealed partial class NextStage : EventChannel { }

