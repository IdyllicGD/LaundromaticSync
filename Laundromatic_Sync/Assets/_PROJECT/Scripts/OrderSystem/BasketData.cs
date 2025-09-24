using UnityEngine;

public class BasketData : MonoBehaviour
{
    public int NpcID;

    void Start()
    {
        NpcManager.instance.currentBaskets.Add(gameObject);
        //GetParentNpcId
    }
}
