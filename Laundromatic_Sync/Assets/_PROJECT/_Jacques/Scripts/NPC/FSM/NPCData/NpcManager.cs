using System;
using System.Collections.Generic;
using UnityEngine;

public class NpcManager : MonoBehaviour
{
    public static NpcManager instance;
    public Dictionary<int, GameObject> basketID = new Dictionary<int, GameObject>();
    public Dictionary<int, GameObject> npcID = new Dictionary<int, GameObject>();
    
    //Line up lists
    [Header("Line Up List")]
    public List<Transform> npcLiningUpPositions = new List<Transform>();
    public List<bool> isLiningUpPositionFull = new List<bool>();
    //Waiting list
    [Header("Waiting Lists")]
    public List<Transform> npcSittingPositions = new List<Transform>();
    public List<bool> isSittingPositionFull = new List<bool>();
    
    [Header("Other Variables")]
    public int npcCount;
    public int ordersCompleted;
    public int maxNpcCount;

    public Transform basketPlacePosition;
    
    [SerializeField] GameObject npcPrefab;
    [SerializeField] Transform spawnPosition;
    

    private void Awake()
    {
        instance = this;
    }

    private void SpawnNpc() { if (npcCount == maxNpcCount) return;
        npcCount++;
        npcID.Add(npcCount ,Instantiate(npcPrefab, spawnPosition));
    }

    private void Update()
    {
        SpawnNpc();
    }
}
