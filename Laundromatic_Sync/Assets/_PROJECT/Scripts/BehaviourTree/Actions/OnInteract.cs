using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/OnInteract")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "OnInteract", message: "Player Interacted With [Self]", category: "Events", id: "b39f05663d538b938529c4757ce0c07c")]
public sealed partial class OnInteract : EventChannel<GameObject> { }

