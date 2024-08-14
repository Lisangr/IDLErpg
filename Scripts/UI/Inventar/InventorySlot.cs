using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventorySlot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Image icon;
    public string itemName;
    public int itemQuantity;
    public Text quantityText;
    public int index;
    public string category;

    private Inventory inventory;
    private LootData item;
    private Vector3 originalPosition;
    private Transform originalParent;
    private CanvasGroup canvasGroup;
    private InventoryUI inventoryUI;
    private bool isOutsideInventory;
    private Vector3 currentMousePosition;
    private GameObject draggingIcon;
    private DefenseCounter defenseCounter;
    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        inventoryUI = FindObjectOfType<InventoryUI>();       
        inventory = FindObjectOfType<Inventory>();
        defenseCounter = FindObjectOfType<DefenseCounter>();
        category = gameObject.name;
    }

    private void Update()
    {
        Vector3 screenPosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
        {
            currentMousePosition = hit.point;
        }
    }

    public void AddItem(string newItemName, int quantity)
    {
        itemName = newItemName;
        itemQuantity = quantity;

        item = Resources.Load<LootData>($"Items/{itemName}");
        if (item != null)
        {
            icon.sprite = item.icon;
            if (icon.sprite != null)
            {
                icon.enabled = true;
                quantityText.text = itemQuantity.ToString();
                Debug.Log($"Item '{itemName}' added to inventory slot with quantity {itemQuantity}.");
            }
            else
            {
                Debug.LogWarning($"Icon for item '{itemName}' not found.");
                icon.enabled = false;
                quantityText.text = "";
            }
        }
        else
        {
            Debug.LogWarning($"Item '{itemName}' not found in Resources/Items.");
            icon.enabled = false;
            quantityText.text = "";
        }
    }

    public void ClearSlot()
    {
        itemName = null;
        itemQuantity = 0;
        icon.sprite = null;
        icon.enabled = false;
        quantityText.text = "";
        item = null;
    }
    public void OnRemoveButton()
    {
        if (ItemPickup.itemInventory.TryGetValue(itemName, out int quantity))
        {
            if (quantity > 1)
            {
                quantity -= 1;
                ItemPickup.itemInventory[itemName] = quantity;
            }
            else if (quantity == 1)
            {
                ItemPickup.itemInventory.Remove(itemName);
            }

            inventoryUI.UpdateUI();
        }
        else
        {
            Debug.Log("Item not found in inventory: " + itemName);
        }
    }/*
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            AmmoSlot[] ammoSlots = FindObjectsOfType<AmmoSlot>();

            foreach (AmmoSlot ammoSlot in ammoSlots)
            {
                if (ammoSlot.category == item.category)
                {
                    if (ammoSlot.SetItem(item))
                    {
                        inventoryUI.inventory.AddToAmmoInventory(itemName, itemQuantity);
                        ammoSlot.AddItem(itemName, itemQuantity);
                        ItemPickup.itemInventory.Remove(itemName);
                        ClearSlot();

                        // Добавляем защиту при экипировке
                        defenseCounter.AddDefense(item.defense);

                        Debug.Log("Item successfully moved to AmmoSlot: " + ammoSlot.gameObject.name);
                        break;
                    }
                    else
                    {
                        Debug.LogWarning("Failed to set item in AmmoSlot: " + ammoSlot.gameObject.name);
                    }
                    break;
                }
            }
        }
    }*/
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            AmmoSlot[] ammoSlots = FindObjectsOfType<AmmoSlot>();

            foreach (AmmoSlot ammoSlot in ammoSlots)
            {
                if (ammoSlot.category == item.category)
                {
                    if (ammoSlot.SetItem(item))
                    {
                        inventoryUI.inventory.AddToAmmoInventory(itemName, itemQuantity);
                        ammoSlot.AddItem(itemName, itemQuantity);

                        // Удаление предмета из инвентаря после успешного перемещения
                        ItemPickup.itemInventory.Remove(itemName);
                        ClearSlot();

                        // Добавление защиты при перемещении в AmmoSlot
                        defenseCounter.AddDefense(item.defense);

                        Debug.Log("Item successfully moved to AmmoSlot: " + ammoSlot.gameObject.name);
                        break;
                    }
                    else
                    {
                        Debug.LogWarning("Failed to set item in AmmoSlot: " + ammoSlot.gameObject.name);
                    }
                    break;
                }
            }
        }
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

        if (!RectTransformUtility.RectangleContainsScreenPoint(inventoryUI.GetComponent<RectTransform>(), Input.mousePosition))
        {
            isOutsideInventory = true;
        }
        else
        {
            isOutsideInventory = false;
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        if (draggingIcon != null)
        {
            Destroy(draggingIcon);
        }       

        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, results);

        bool itemMoved = false;
        foreach (RaycastResult result in results)
        {
            if (result.gameObject.GetComponent<InventorySlot>() != null)
            {
                InventorySlot targetSlot = result.gameObject.GetComponent<InventorySlot>();

                if (targetSlot != null)
                {
                    string tempItemName = targetSlot.itemName;
                    int tempItemQuantity = targetSlot.itemQuantity;
                    targetSlot.AddItem(itemName, itemQuantity);
                    AddItem(tempItemName, tempItemQuantity);
                    ItemPickup.itemInventory[itemName] = itemQuantity;
                    itemMoved = true;
                    break;
                }
            }
            else if (result.gameObject.CompareTag("AmmoSlot"))
            {                
                AmmoSlot ammoSlot = result.gameObject.GetComponent<AmmoSlot>();
                if (ammoSlot != null && ammoSlot.category == item.category)
                {
                    inventoryUI.inventory.AddToAmmoInventory(itemName, itemQuantity);
                    ammoSlot.AddItem(itemName, itemQuantity);
                    defenseCounter.AddDefense(item.defense);
                    ItemPickup.itemInventory.Remove(itemName);
                    ClearSlot();
                    //inventoryUI.UpdateAmmoUI();
                    inventoryUI.UpdateUI();
                    Debug.Log("Item moved to ammo slot.");
                    itemMoved = true;
                    break;
                }
            }            
        }

        if (!itemMoved)
        {
            transform.position = originalPosition;
            transform.SetParent(originalParent);
        }
    }

    private void CreateDroppedItem()
    {
        if (!string.IsNullOrEmpty(itemName))
        {
            GameObject prefab = Resources.Load<GameObject>($"Items/{itemName}");
            if (prefab != null)
            {
                GameObject droppedItem = Instantiate(prefab);
                droppedItem.transform.position = currentMousePosition;
            }
            else
            {
                Debug.LogWarning($"Prefab for item '{itemName}' not found in Resources/Items.");
            }

            if (ItemPickup.itemInventory.TryGetValue(itemName, out int quantity))
            {
                if (quantity > 1)
                {
                    quantity -= 1;
                    ItemPickup.itemInventory[itemName] = quantity;
                    
                }
                else if (quantity == 1)
                {
                    ItemPickup.itemInventory.Remove(itemName);
                }

                inventoryUI.UpdateUI();
            }
            else
            {
                Debug.Log("Item not found in inventory: " + itemName);
            }
        }
    }


    private void TradingItems()
    {
        if (!string.IsNullOrEmpty(itemName))
        {
            if (ItemPickup.itemInventory.TryGetValue(itemName, out int quantity))
            {
                if (quantity > 1)
                {
                    quantity -= 1;
                    ItemPickup.itemInventory[itemName] = quantity;
                }
                else if (quantity == 1)
                {
                    ItemPickup.itemInventory.Remove(itemName);
                }

                inventoryUI.UpdateUI();
            }
            else
            {
                Debug.Log("Item not found in inventory: " + itemName);
            }
        }
    }
}
