using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UsePotion : MonoBehaviour
{
    public Button usePotionButton;
    public TextMeshProUGUI potionCounterText;
    private string potionName = "Potition";

    private void Start()
    {
        // Delay potion button update if needed
        usePotionButton.onClick.AddListener(UsePotionItem);

        // Check inventory contents at start
        Debug.Log("Checking inventory on UsePotion Start:");
        ItemPickup.LogInventoryContents();

        // Update button based on current inventory
        UpdatePotionButton();
    }

    public void UpdatePotionButton()
    {
        // Fetch the potion count from the shared inventory
        if (ItemPickup.itemInventory.TryGetValue(potionName, out int potionCount))
        {
            potionCounterText.text = potionCount.ToString();
            usePotionButton.interactable = potionCount > 0;
            Debug.Log($"Potion count updated: {potionCount}");
        }
        else
        {
            potionCounterText.text = "0";
            usePotionButton.interactable = false;
            Debug.Log("Potion not found in inventory.");
        }

        // Log inventory after updating the button
        ItemPickup.LogInventoryContents();
    }

    public void UsePotionItem()
    {
        if (ItemPickup.itemInventory.TryGetValue(potionName, out int potionCount) && potionCount > 0)
        {
            Player player = FindObjectOfType<Player>();
            player.RestoreHealth(0.3f);  // Assuming 30% health restoration
            ItemPickup.itemInventory[potionName]--;
            Debug.Log($"Potion used, remaining: {ItemPickup.itemInventory[potionName]}");
            UpdatePotionButton();
        }
        else
        {
            Debug.Log("Cannot use potion, none available.");
        }

        // Log inventory after using a potion
        ItemPickup.LogInventoryContents();
    }
}
