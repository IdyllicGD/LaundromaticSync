using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class NPCBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject basket;
    public bool hasBasket = true;
    bool orderComplete = false;
    public int ID;
    public int CurrentLinePosition;
    public int currentLinePosition  {get => CurrentLinePosition; set {CurrentLinePosition = value ; OnLinePositionChanged(value);}}
    
    NPCManagerFrontDesk npcManager;

    private void Awake()
    {
        npcManager = NPCManagerFrontDesk.instance;
        OnLinePositionChanged(CurrentLinePosition);
    }

    private void OnLinePositionChanged(int linePosition)
    {
        if (linePosition != 0) return;
        FrontOfLine();
    }
    

    public void FrontOfLine()
    {
        if (!hasBasket) return;
        Debug.Log($"[NPCBehaviour] Current NPC: {ID} has Basket");
        if (npcManager.hasBasket)return;
        Debug.Log($"[NPCBehaviour] Current NPC: {ID} npc manager has Basket");
        if(CurrentLinePosition != 0)return;
        basket.transform.DOMove(npcManager.basketPlacePosition.position, 0.5f).OnComplete(MoveToWaitingArea);
        
        
    }

    void MoveToWaitingArea()
    {
        npcManager.hasBasket = true;
        hasBasket = false;
        npcManager.GoToWaitingArea(ID);
        npcManager.npcs.RemoveAt(ID);
        npcManager.npcsWaiting.Add(gameObject);
        npcManager.MoveLineForward();
        
        basket.transform.SetParent(npcManager.basketPlacePosition.transform);
    }
    
    
    
}
