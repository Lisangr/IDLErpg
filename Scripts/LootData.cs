using UnityEngine;

[CreateAssetMenu(fileName = "NewLoot", menuName = "ScriptableObjects/LootData", order = 1)]

public class LootData : ScriptableObject
{
    public string itemName;                 // название лута    
    public string itemNameForUI = null;
    public float spawnChance;           // Шанс появления лута   
    public Sprite icon = null;
    public string category = null;
    public int defense = 0;
    public string description;
}
