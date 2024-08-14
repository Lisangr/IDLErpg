using UnityEngine;

[CreateAssetMenu(fileName = "NewLoot", menuName = "ScriptableObjects/LootData", order = 1)]

public class LootData : ScriptableObject
{
    public string itemName;                 // �������� ����    
    public string itemNameForUI = null;
    public float spawnChance;           // ���� ��������� ����   
    public Sprite icon = null;
    public string category = null;
    public int defense = 0;
    public string description;
}
