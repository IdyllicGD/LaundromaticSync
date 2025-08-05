using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] int maxNpcAmount;
    public List<GameObject> currentNpcs;
    public Transform startLineUpPosition;
    public List<Transform> waitingPositions = new();

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
        if (currentNpcs.Count >= maxNpcAmount) return;
        currentNpcs.Add(Instantiate(npcPrefabs[Random.Range(0, npcPrefabs.Count)], spawnPosition));
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


}



public enum NpcOrderTypes
{
    OrderDirty, OrderClean, OrderRandom
}

public enum NpcFinalOrderTypes
{
    Clean, Dirty, Random
}
