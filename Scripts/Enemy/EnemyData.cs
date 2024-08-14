using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "ScriptableObjects/EnemyData", order = 1)]
public class EnemyData : ScriptableObject
{
    public string enemyName;          // Имя врага
    public int health;                // Здоровье врага
    public int attack;                // Атака врага
    public int defense;               // Защита врага
    public float spawnChance;         // Шанс появления врага
    public GameObject enemyPrefab;    // Префаб врага
    public int exp;
}
