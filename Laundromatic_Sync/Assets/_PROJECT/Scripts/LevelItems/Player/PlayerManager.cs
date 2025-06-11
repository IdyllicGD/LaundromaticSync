using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager Instance;
        public Dictionary<string, GameObject> playerInventory = new Dictionary<string, GameObject>();
        public float playerMoney;
        public Character_Controller_Player character_Player;
        [SerializeField] TextMeshProUGUI playerMoneyText;
        [SerializeField] private TextMeshProUGUI playerMoneyShopUi;

        private void Awake()
        {
            Instance = this;
        }

        public void PlayerActionsEnableOrDisable(bool enable)
        {
            switch (enable)
            {
                case true:
                    character_Player.playerActions.Enable();
                    break;
                case false:
                    character_Player.playerActions.Disable();
                    break;
            }
        }

        private void Update()
        {
            playerMoneyText.text = $"${playerMoney}";
            playerMoneyShopUi.text = $"${playerMoney}";
        }
    }
