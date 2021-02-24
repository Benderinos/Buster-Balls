﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopUpgrades : MonoBehaviour
{
    [Header("List of items to sold")]
    [SerializeField] public UpgradeItem[] items;

    [Header("References")]
    [SerializeField] private Transform upgradesContainer;
    [SerializeField] private GameObject upgradesItemPrefab;

    public GameController gameController;
    public TextMeshProUGUI coinsText;
    private UpgradeItem itemSelected;
    public float[] currentItems;
    public ShopManager shopManager;

    private int coins;

    private void Awake()
    {
        PlayerData playerData = SaveSystem.LoadPlayerData();
        if (playerData != null)
        {
            coins = playerData.coins;
        }
        else
        {
            coins = 0;
        }
        coins = 100000;
        coinsText.text = ""+ coins;
        UpgradesData upgrades = SaveSystem.LoadUpgrades();
        if(upgrades != null)
        {
            currentItems = upgrades.items;
        }
    }

    private void Start()
    {
        PopulateShop();
    }

    private void PopulateShop()
    {
        for (int i = 0; i < items.Length; i++)
        {
            UpgradeItem item = items[i];
            GameObject itemObject = Instantiate(upgradesItemPrefab, upgradesContainer);

            itemObject.GetComponent<Button>().onClick.AddListener(() => OnButtonClick(item, itemObject));

            itemObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "" + item.cost;
            itemObject.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "" + item.itemName;
            if(currentItems != null && currentItems.Length > 0)
            {
                for (int j = 0; j < currentItems.Length; j++)
                {
                    if (currentItems[i] == item.id)
                    {
                        itemObject.gameObject.GetComponent<Image>().color = new Color32(255, 0, 0, 0);
                    }
                }
            }
        }     
    }

    private void OnButtonClick(UpgradeItem item, GameObject itemObject)
    {
        itemSelected = item;
        foreach (Transform child in upgradesContainer)
        {
            child.gameObject.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
        }
        itemObject.GetComponent<Image>().color = item.backgroundColor;
    }

    public UpgradeItem GetItemSelected()
    {
        return itemSelected;
    }
    public void Buy()
    {
        bool canBuy = true;
        if (currentItems != null)
        {
            for (int i = 0; i < currentItems.Length; i++)
            {
                if (currentItems[i] == GetItemSelected().id)
                {
                    canBuy = false;
                }
            }
        }
        if(GetItemSelected() != null)
        {
            if (GetItemSelected().cost <= coins && canBuy)
            {
                //buy item
                //save upgrades
                Debug.Log("Buy it");
                SaveSystem.SaveUpgrades(this);
                gameController.ApplyUpgrades();
                //apply upgrades
            }
            else
            {
                Debug.Log("No no no give me money");
                //show not enogh coins dialog
            }
        }
    }
 
}
