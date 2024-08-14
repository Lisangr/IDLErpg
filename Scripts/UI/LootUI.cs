using UnityEngine;
using UnityEngine.UI;

public class LootUI : MonoBehaviour
{
    public Image itemImage; // ������ �� UI Image ���������

    public void UpdateLootDisplay(LootData loot)
    {
        if (itemImage == null)
        {
            Debug.LogError("Item image reference is null!");
            return;
        }

        itemImage.sprite = loot.icon; // ���������� ������ ��������
    }
}
