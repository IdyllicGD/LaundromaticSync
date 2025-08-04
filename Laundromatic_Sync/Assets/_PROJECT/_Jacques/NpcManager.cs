using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class NpcManager : MonoBehaviour
{
    public static NpcManager instance;
    public Transform playerTransform;
    [SerializeField] Transform spawnPosition;
    [SerializeField] List<GameObject> npcPrefabs = new();
    public Transform startLineUpPosition;
    public List<Transform> waitingPositions = new();
    public UnityEvent whenToSpawnNpc = new();

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }

        Debug.Log("[NpcMan] Trying to spawn npc");
        SpawnNpc();
    }

    public void SpawnNpc()
    {
        Instantiate(npcPrefabs[Random.Range(0, npcPrefabs.Count)], spawnPosition);
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
}



public enum NpcOrderTypes
{
    OrderDirty, OrderClean, OrderRandom
}
