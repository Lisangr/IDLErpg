using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public static ItemPickup Instance { get; private set; }
    public static Dictionary<string, int> itemInventory = new Dictionary<string, int>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeInventory();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeInventory()
    {
        if (!itemInventory.ContainsKey("Potion"))
        {
            itemInventory["Potion"] = 0; // Initialize with default count
        }

        // Log initial state
        Debug.Log("Inventory Initialized.");
        LogInventoryContents();
    }

    public static void LogInventoryContents()
    {
        Debug.Log("Current Inventory:");
        foreach (var item in itemInventory)
        {
            Debug.Log($"Item: {item.Key}, Count: {item.Value}");
        }
    }
}


/*using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public LootData item;
    [HideInInspector] public string itemName;
    [HideInInspector] public int itemQuantity = 1;
    public static Dictionary<string, int> itemInventory = new Dictionary<string, int>(); // Словарь для хранения предметов

    private Inventory inventory;
    private InventoryUI inventoryUIManager;

    private void Awake()
    {
        inventory = FindObjectOfType<Inventory>();
        inventoryUIManager = FindObjectOfType<InventoryUI>();
    }

    public void InitializeItem()
    {
        itemName = item.itemName;
        Debug.Log($"Initialized item: {itemName}");
    }

    private void Start()
    {
        if (string.IsNullOrEmpty(itemName))
        {
            InitializeItem();
        }
    }

    public void AddItemToInventory()
    {
        inventory.Add(item);
    }
}*/