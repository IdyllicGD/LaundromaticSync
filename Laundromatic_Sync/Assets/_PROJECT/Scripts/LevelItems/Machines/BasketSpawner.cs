using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BasketSpawner : MonoBehaviour
{
    [SerializeField] GameObject basketPrefab;
    
    public static BasketSpawner instance;

    public bool canSpawnBaskets = false;

    private void Start()
    {
        Instantiate(basketPrefab ,transform.position, Quaternion.identity, transform);
    }

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (transform.childCount != 0 || !canSpawnBaskets) return;
        Instantiate(basketPrefab, transform.position, Quaternion.identity, transform);
    }
}
