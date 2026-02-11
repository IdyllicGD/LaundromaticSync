using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class NpcManager : MonoBehaviour
{
    public static NpcManager instance;
    public Transform playerTransform;
    public List<Transform> basketPlacePosition = new();
    [SerializeField] Transform spawnPosition;
    [SerializeField] List<GameObject> npcPrefabs = new();
    [SerializeField] List<GameObject> originalNpcPrefabs = new();
    [SerializeField] int maxNpcAmount;
    public List<GameObject> currentNpcs;
    public List<GameObject> currentBaskets;
    public Transform startLineUpPosition;
    public bool canSpawnNpc = true;
    public List<Transform> waitingPositions = new();

    [Title("Special Npc Stuff", null, TitleAlignments.Centered)]
    public List<GameObject> detergentsInWorld = new();
    public List<GameObject> softenerInWorld = new();

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }

        SpawnNpc();
    }

    public void SpawnNpc()
    {
        int indexToSpawnAt;
        indexToSpawnAt = Random.Range(0, npcPrefabs.Count);
        if (!canSpawnNpc) return;
        if (currentNpcs.Count >= maxNpcAmount) return;
        currentNpcs.Add(Instantiate(npcPrefabs[indexToSpawnAt], spawnPosition));
        npcPrefabs.RemoveAt(indexToSpawnAt);
    }
    public Transform GetNextWaypoint(Transform agentTransform)
    {
        float _nearestDistance = float.MaxValue;
        Transform _nearestWaypointTransform = null;
        foreach (Transform _transform in waitingPositions)
        {
            float _distanceSqr = (agentTransform.position - _transform.position).sqrMagnitude;
            if (_distanceSqr < _nearestDistance)
            {
                _nearestDistance = _distanceSqr;
                _nearestWaypointTransform = _transform;
            }
        }

        int _index = _nearestWaypointTransform ? waitingPositions.IndexOf(_nearestWaypointTransform) : 0;
        _index++;
        
        if (_index >= waitingPositions.Count)
        {
            _index = 0;
        }

        return waitingPositions[_index];
    }

    public void KillNpc(GameObject npcToKill)
    {
        Destroy(npcToKill);
    }

    public void AddNpcBack(NpcID npcToReAdd)
    {
        Debug.Log("Ran");
        for (int i = 0; i < originalNpcPrefabs.Count; i++)
        {
            if (originalNpcPrefabs[i].GetComponent<NpcOrderData>().npcID == npcToReAdd)
            {
                Debug.Log($"[NpcManager] Prefab added");
                npcPrefabs.Add(originalNpcPrefabs[i]);
                return;
            }
            else
            {
                Debug.Log("Loser Idiot");
            }
        }
    }

    public void ResetNpcList()
    {
        npcPrefabs.Clear();
        for (int i = 0; i < originalNpcPrefabs.Count; i++)
        {
            npcPrefabs.Add(originalNpcPrefabs[i]);

        }
    }


}



public enum NpcOrderTypes
{
    OrderDirty, OrderClean, OrderRandom
}

public enum NpcFinalOrderTypes
{
    Clean, Dirty, Random
}
