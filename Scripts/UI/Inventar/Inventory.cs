using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class Inventory : MonoBehaviour
{
    public int space = 10;
    public List<LootData> allItems;

    private InventoryUI inventoryUIManager;
    public static Dictionary<string, int> itemInventory = new Dictionary<string, int>();
    public static Dictionary<string, int> ammoInventory = new Dictionary<string, int>();
    private string saveFilePath;

    private void Awake()
    {
        inventoryUIManager = FindObjectOfType<InventoryUI>();
        saveFilePath = Path.Combine(Application.persistentDataPath, "inventory.json");
        LoadInventory();
        InitializeInventory();
    }

    private void InitializeInventory()
    {
        Debug.Log("Initializing Inventory");
        foreach (LootData item in allItems)
        {
            if (itemInventory.ContainsKey(item.itemName))
            {
                itemInventory[item.itemName]++;
                Debug.Log("Item already in inventory: " + item.itemName);
            }
            else if (itemInventory.Count < space)
            {
                itemInventory.Add(item.itemName, 1);
                Debug.Log("Item added to inventory: " + item.itemName);
            }
            else
            {
                Debug.Log("Not enough space to add item: " + item.itemName);
            }
        }
    }

    public bool Add(LootData item)
    {
        Debug.Log("Adding item: " + item.itemName);
        if (itemInventory.ContainsKey(item.itemName))
        {
            itemInventory[item.itemName]++;
            inventoryUIManager.UpdateUI();
            SaveInventory();
            Debug.Log("Item count increased: " + item.itemName);
            return true;
        }
        else if (itemInventory.Count < space)
        {
            itemInventory.Add(item.itemName, 1);
            inventoryUIManager.UpdateUI();
            SaveInventory();
            Debug.Log("New item added: " + item.itemName);
            return true;
        }
        else
        {
            Debug.Log("Not enough room to add item: " + item.itemName);
            return false;
        }
    }

    public void Remove(string itemName)
    {
        Debug.Log("Removing item: " + itemName);
        if (itemInventory.ContainsKey(itemName))
        {
            itemInventory[itemName]--;
            if (itemInventory[itemName] <= 0)
            {
                itemInventory.Remove(itemName);
                Debug.Log("Item removed from inventory: " + itemName);
            }
            SaveInventory();
            inventoryUIManager.UpdateUI();
        }
        else
        {
            Debug.Log("Item not found in inventory: " + itemName);
        }
    }

    public void AddToAmmoInventory(string itemName, int quantity)
    {
        Debug.Log("Adding to ammo inventory: " + itemName + ", quantity: " + quantity);
        if (ammoInventory.ContainsKey(itemName))
        {
            ammoInventory[itemName] += quantity;
        }
        else
        {
            ammoInventory.Add(itemName, quantity);
        }
    }

    public LootData GetItemByName(string itemName)
    {
        Debug.Log("Getting item by name: " + itemName);
        foreach (LootData item in allItems)
        {
            if (item.itemName == itemName)
            {
                Debug.Log("Item found: " + itemName);
                return item;
            }
        }
        Debug.Log("Item not found: " + itemName);
        return null;
    }

    private void SaveInventory()
    {
        Debug.Log("Saving inventory to file");
        InventorySaveData saveData = new InventorySaveData
        {
            itemInventory = itemInventory,
            ammoInventory = ammoInventory
        };

        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(saveFilePath, json);
    }

    private void LoadInventory()
    {
        Debug.Log("Loading inventory from file");
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            InventorySaveData saveData = JsonUtility.FromJson<InventorySaveData>(json);

            itemInventory = saveData.itemInventory ?? new Dictionary<string, int>();
            ammoInventory = saveData.ammoInventory ?? new Dictionary<string, int>();

            Debug.Log("Inventory loaded successfully");
        }
        else
        {
            Debug.Log("No inventory file found. Starting with empty inventory.");
            itemInventory = new Dictionary<string, int>();
            ammoInventory = new Dictionary<string, int>();
        }
    }
}

[System.Serializable]
public class InventorySaveData
{
    public Dictionary<string, int> itemInventory;
    public Dictionary<string, int> ammoInventory;
}