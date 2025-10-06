using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using DG.Tweening;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "PlaceBasketOnTable", story: "Place [Basket] on [Table] and set [HasBasket] to true", category: "Action", id: "a62fb552c2d0eae4bb2059fb51ca5375")]
public partial class PlaceBasketOnTableAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Basket;
    [SerializeReference] public BlackboardVariable<Transform> Table;
    [SerializeReference] public BlackboardVariable<bool> HasBasket;
    [SerializeReference] public BlackboardVariable<Transform> BasketSpawnPosition;
    [SerializeReference] public BlackboardVariable<GameObject> Self;

    protected override Status OnStart()
    {
        if (HasBasket.Value) return Status.Failure;;
        Basket.Value = Self.Value.transform.GetChild(0).transform.GetChild(0).gameObject;
        HasBasket.Value = true;
        Basket.Value.GetComponent<Collider>().enabled = true;
        for (int i = 0; i <= NpcManager.instance.basketPlacePosition.Count; i++)
        {
            if (NpcManager.instance.basketPlacePosition[i].childCount == 0)
            {
                Table.Value = NpcManager.instance.basketPlacePosition[i];
                Basket.Value.transform.position = Table.Value.position;
                Basket.Value.transform.SetParent(Table.Value);
                return Status.Success;
            }

        }
        Debug.Log($"[PlaceBasketOnTableAction] no table position was found make sure there are enough spots for npcs to put down baskets");
        return Status.Failure;
    }

}

