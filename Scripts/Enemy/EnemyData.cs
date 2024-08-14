using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "ScriptableObjects/EnemyData", order = 1)]
public class EnemyData : ScriptableObject
{
    public string enemyName;          // ��� �����
    public int health;                // �������� �����
    public int attack;                // ����� �����
    public int defense;               // ������ �����
    public float spawnChance;         // ���� ��������� �����
    public GameObject enemyPrefab;    // ������ �����
    public int exp;
}
