using System;
using System.Collections;
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

    public bool tableHasBasket;
    
    Transform LiningUpPosition;
    Transform SittingPosition;

    public Transform basketPlacePosition;
    public List<Transform> basketPlacedPositions = new List<Transform>();
    
    [SerializeField] GameObject npcPrefab;
    [SerializeField] Transform spawnPosition;
    

    private void Awake()
    {
        
    }

    private void Start()
    {
        instance = this;
        SpawnNpc();
        
    }

    private void SpawnNpc() { if (npcCount == maxNpcCount) return;
        //choose variant
        Debug.Log("Start Spawn");
        npcCount++;
        npcID.Add(npcCount ,Instantiate(npcPrefab, spawnPosition));
        StartCoroutine(TimeToWait(1f));
    }

    private void Update()
    {
        if (npcCount == maxNpcCount)
        {
            StartCoroutine(TimeToWait(5f));
        }
    }

    IEnumerator TimeToWait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SpawnNpc();
    }
}
