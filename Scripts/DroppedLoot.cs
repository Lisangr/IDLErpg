using System.Collections.Generic;
using UnityEngine;

public class DroppedLoot : MonoBehaviour
{
    public List<LootData> items;
    public List<LootUI> lootUISlots;

    private void Awake()
    {
        if (lootUISlots == null || lootUISlots.Count == 0)
        {
            Debug.LogError("No LootUI slots assigned!");
        }
    }

    private void OnEnable()
    {
        Enemy.OnEnemyDestroy += SpawnLoot;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyDestroy -= SpawnLoot;
    }

    private void SpawnLoot()
    {
        if (items == null || items.Count == 0)
        {
            return;
        }

        List<LootData> selectedItems = SelectRandomLoots(lootUISlots.Count);
        for (int i = 0; i < selectedItems.Count; i++)
        {
            var selectedItem = selectedItems[i];
            if (selectedItem != null && i < lootUISlots.Count)
            {
                if (ItemPickup.itemInventory.ContainsKey(selectedItem.itemName))
                {
                    ItemPickup.itemInventory[selectedItem.itemName]++;
                }
                else
                {
                    ItemPickup.itemInventory.Add(selectedItem.itemName, 1);
                }

                Debug.Log($"Added {selectedItem.itemName} to inventory. Total now: {ItemPickup.itemInventory[selectedItem.itemName]}");

                // Log entire inventory
                ItemPickup.LogInventoryContents();

                var lootUI = lootUISlots[i];
                if (lootUI != null)
                {
                    if (!lootUI.gameObject.activeSelf)
                    {
                        lootUI.gameObject.SetActive(true);
                    }

                    if (selectedItem.icon != null)
                    {
                        lootUI.UpdateLootDisplay(selectedItem);
                    }
                }
            }
        }
    }


    private List<LootData> SelectRandomLoots(int count)
    {
        List<LootData> selectedItems = new List<LootData>();
        HashSet<int> selectedIndices = new HashSet<int>();

        int attempts = 0;
        while (selectedItems.Count < count && attempts < 100)
        {
            attempts++;
            int index = Random.Range(0, items.Count);
            if (!selectedIndices.Contains(index))
            {
                selectedItems.Add(items[index]);
                selectedIndices.Add(index);
            }
        }

        return selectedItems;
    }
}
