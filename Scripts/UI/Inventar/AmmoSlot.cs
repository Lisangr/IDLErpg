using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using Newtonsoft.Json;

public class AmmoSlot : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Image icon;
    public string itemName;
    public int itemQuantity;
    public string category;
    public Image itemIcon;

    private LootData item;
    private Vector3 originalPosition;
    private Transform originalParent;
    private CanvasGroup canvasGroup;
    private InventoryUI inventoryUI;
    private GameObject draggingIcon;
    private PlayerData playerData; // Use PlayerData instead of Player
    private DefenseCounter defenseCounter;
    [System.Serializable]
    public class SlotData
    {
        public string itemName;
        public int itemQuantity;
    }

    private void Awake()
    {
        category = gameObject.name;
        playerData = FindObjectOfType<PlayerData>(); // Find PlayerData to update defense
    }

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        inventoryUI = FindObjectOfType<InventoryUI>();
        defenseCounter = FindObjectOfType<DefenseCounter>();
        LoadSlots();
    }
    /*
    public bool SetItem(LootData newItem)
    {
        if (itemIcon == null)
        {
            Debug.LogError("itemIcon не установлен в инспекторе для " + gameObject.name);
            return false;
        }

        if (newItem != null)
        {
            itemIcon.sprite = newItem.icon;
            itemIcon.enabled = true;
            item = newItem;

            // Добавляем защиту при добавлении предмета
            defenseCounter.AddDefense(item.defense);

            SaveSlots();
        }
        else
        {
            itemIcon.sprite = null;
            itemIcon.enabled = false;

            // Убираем защиту при снятии предмета
            defenseCounter.RemoveDefense(item.defense);

            item = null;
        }

        return true;
    }*/
    public bool SetItem(LootData newItem)
    {
        if (itemIcon == null)
        {
            Debug.LogError("itemIcon не установлен в инспекторе для " + gameObject.name);
            return false;
        }

        if (item != null)
        {
            // Удаляем защиту текущего предмета перед заменой
            defenseCounter.RemoveDefense(item.defense);
        }

        if (newItem != null)
        {
            itemIcon.sprite = newItem.icon;
            itemIcon.enabled = true;
            item = newItem;

            // Добавляем защиту при добавлении нового предмета
            defenseCounter.AddDefense(item.defense);

            SaveSlots();
        }
        else
        {
            itemIcon.sprite = null;
            itemIcon.enabled = false;

            // Обнуление защиты, если новый предмет отсутствует
            item = null;
        }

        return true;
    }
    public void AddItem(string newItemName, int quantity)
    {
        itemName = newItemName;
        itemQuantity = quantity;

        item = Resources.Load<LootData>($"Items/{itemName}");
        if (item != null)
        {
            icon.sprite = item.icon;
            icon.enabled = true;
            SaveSlots();
            Debug.Log($"Item '{itemName}' added to ammo slot {category} with quantity {itemQuantity} and defense {item.defense}.");
        }
        else
        {
            Debug.LogWarning($"Item '{itemName}' not found in Resources/Items.");
            icon.enabled = false;
        }
    }
    public void ClearSlot()
    {
        if (item != null)
        {
            // Удаляем защиту перед очисткой слота
            defenseCounter.RemoveDefense(item.defense);
        }

        itemName = null;
        itemQuantity = 0;
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        SaveSlots();
        Debug.Log("Ammo slot cleared.");
    }
    /*
    public void ClearSlot()
    {        
        itemName = null;
        itemQuantity = 0;
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        SaveSlots();
        Debug.Log("Ammo slot cleared.");
    }
    */
    public bool IsSlotEmpty()
    {
        return item == null;
    }

    public LootData GetItem()
    {
        return item;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        originalPosition = transform.position;
        canvasGroup.blocksRaycasts = false;

        draggingIcon = new GameObject("Dragging Icon");
        draggingIcon.transform.SetParent(inventoryUI.transform, false);
        draggingIcon.transform.SetAsLastSibling();

        Image draggingIconImage = draggingIcon.AddComponent<Image>();
        draggingIconImage.sprite = icon.sprite;
        draggingIconImage.raycastTarget = false;

        RectTransform draggingIconRect = draggingIcon.GetComponent<RectTransform>();
        draggingIconRect.sizeDelta = new Vector2(50, 50);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggingIcon != null)
        {
            draggingIcon.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        if (draggingIcon != null)
        {
            Destroy(draggingIcon);
        }
        bool itemMoved = false;
        MoveToInventory();

        if (!itemMoved)
        {
            transform.position = originalPosition;
            transform.SetParent(originalParent);
        }
    }
    /*
    private void MoveToInventory()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.GetComponent<InventorySlot>() != null)
            {
                defenseCounter.RemoveDefense(item.defense);
                InventorySlot targetSlot = result.gameObject.GetComponent<InventorySlot>();

                if (targetSlot != null)
                {
                    targetSlot.AddItem(itemName, itemQuantity);
                    if (Inventory.ammoInventory.ContainsKey(itemName))
                    {
                        Inventory.ammoInventory[itemName] -= itemQuantity;
                        if (Inventory.ammoInventory[itemName] <= 0)
                        {
                            Inventory.ammoInventory.Remove(itemName);
                        }
                    }
                    if (ItemPickup.itemInventory.ContainsKey(itemName))
                    {
                        ItemPickup.itemInventory[itemName] += itemQuantity;
                    }
                    else
                    {
                        ItemPickup.itemInventory.Add(itemName, itemQuantity);
                    }
                    ClearSlot();
                    
                    inventoryUI.UpdateUI();
                    SaveSlots();
                    Debug.Log("Item moved back to inventory slot.");
                    break;
                }
            }
        }
    }
    */
    private void MoveToInventory()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.GetComponent<InventorySlot>() != null)
            {
                InventorySlot targetSlot = result.gameObject.GetComponent<InventorySlot>();

                if (targetSlot != null)
                {
                    // Удаляем защиту из AmmoSlot перед перемещением
                    defenseCounter.RemoveDefense(item.defense);

                    // Добавляем предмет в целевую ячейку
                    targetSlot.AddItem(itemName, itemQuantity);

                    // Добавляем защиту обратно, если перемещаем в инвентарь
                    defenseCounter.AddDefense(item.defense);

                    if (Inventory.ammoInventory.ContainsKey(itemName))
                    {
                        Inventory.ammoInventory[itemName] -= itemQuantity;
                        if (Inventory.ammoInventory[itemName] <= 0)
                        {
                            Inventory.ammoInventory.Remove(itemName);
                        }
                    }
                    if (ItemPickup.itemInventory.ContainsKey(itemName))
                    {
                        ItemPickup.itemInventory[itemName] += itemQuantity;
                    }
                    else
                    {
                        ItemPickup.itemInventory.Add(itemName, itemQuantity);
                    }
                    ClearSlot();

                    inventoryUI.UpdateUI();
                    SaveSlots();
                    Debug.Log("Item moved back to inventory slot.");
                    break;
                }
            }
        }
    }
    private void SaveSlots()
    {
        SlotData slotData = new SlotData
        {
            itemName = itemName,
            itemQuantity = itemQuantity
        };

        string jsonData = JsonConvert.SerializeObject(slotData);
        PlayerPrefs.SetString("AmmoSlot_" + category, jsonData);
        PlayerPrefs.Save();
    }

    private void LoadSlots()
    {
        string jsonData = PlayerPrefs.GetString("AmmoSlot_" + category, string.Empty);

        if (!string.IsNullOrEmpty(jsonData))
        {
            SlotData slotData = JsonConvert.DeserializeObject<SlotData>(jsonData);
            if (slotData != null && !string.IsNullOrEmpty(slotData.itemName))
            {
                AddItem(slotData.itemName, slotData.itemQuantity);
            }
        }
    }
}
