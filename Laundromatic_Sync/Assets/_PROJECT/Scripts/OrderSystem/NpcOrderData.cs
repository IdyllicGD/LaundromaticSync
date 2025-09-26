
using System.Net.NetworkInformation;
using Unity.Behavior;
using UnityEngine;

public class NpcOrderData : MonoBehaviour
{
    public NpcID npcID;
    public Sprite characterImage;

    public void AddMeToList(NpcID iD)
    {
        NpcManager.instance.AddNpcBack(iD);
    }

}

public enum NpcID
{
    RoundBoy, Grandma, Grandpa, Cop, Lifegaurd, Baby, NoHandMan, Karen, Point, None
}