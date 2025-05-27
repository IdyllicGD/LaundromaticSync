using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class NPCBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject basket;
    bool hasBasket = true;
    bool orderComplete = false;
    public int ID;
    public int currentLinePosition;
    
    NPCManagerFrontDesk npcManager;

    private void Awake()
    {
        npcManager = NPCManagerFrontDesk.instance;
        StartCoroutine(this.TimeToWait(2f));
    }
    

    public void FrontOfLine()
    {
        if (!hasBasket) return;
        Debug.Log($"[NPCBehaviour] Current NPC: {ID} has Basket");
        if (npcManager.hasBasket)return;
        Debug.Log($"[NPCBehaviour] Current NPC: {ID} npc manager has Basket");
        if(currentLinePosition != 0)return;
        basket.transform.DOMove(npcManager.basketPlacePosition.position, 0.5f).OnComplete(MoveToWaitingArea);
        
    }

    void MoveToWaitingArea()
    {
        npcManager.hasBasket = true;
        hasBasket = false;
        basket.transform.SetParent(npcManager.basketPlacePosition.transform);
        npcManager.npcs.RemoveAt(ID);
        npcManager.npcsWaiting.Add(gameObject);
        npcManager.GoToWaitingArea(ID);
        npcManager.MoveLineForward();
    }
    

    IEnumerator TimeToWait(float TimeToWait)
    {
        yield return new WaitForSeconds(TimeToWait);
        FrontOfLine();
        StartCoroutine(this.TimeToWait(2f));
    }
    
}
