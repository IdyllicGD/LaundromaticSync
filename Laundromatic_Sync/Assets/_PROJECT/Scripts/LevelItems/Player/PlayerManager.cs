using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager Instance;
        public Dictionary<string, GameObject> playerInventory = new Dictionary<string, GameObject>();
        public float playerMoney;

        private void Awake()
        {
            Instance = this;
        }
        
        
    }
