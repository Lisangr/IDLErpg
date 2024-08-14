using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public EnemyData[] enemiesData;  // ������ ������ � ������
    private Transform spawnPoint;

    private void Awake()
    {
        spawnPoint = GetComponent<Transform>();
    }

    private void Start()
    {
        if (enemiesData.Length > 0)
        {
            EnemyData selectedEnemy = GetRandomEnemy();
            if (selectedEnemy != null && selectedEnemy.enemyPrefab != null)
            {                
                Instantiate(selectedEnemy.enemyPrefab, spawnPoint.position, Quaternion.Euler(0, 180, 0));
            }
            else
            {
                Debug.LogWarning("������ ����� �� ������!");
            }
        }
        else
        {
            Debug.LogWarning("������ ������ � ������ ������!");
        }
    }

    private EnemyData GetRandomEnemy()
    {
        float totalChance = 0f;

        // ������������ ����� ����������� ���������
        foreach (var enemy in enemiesData)
        {
            totalChance += enemy.spawnChance;
        }

        // ���������� ��������� ����� �� 0 �� ����� �����������
        float randomValue = Random.Range(0f, totalChance);
        float cumulativeChance = 0f;

        // �������� ����� �� ������ ��� ����� ���������
        foreach (var enemy in enemiesData)
        {
            cumulativeChance += enemy.spawnChance;
            if (randomValue <= cumulativeChance)
            {
                return enemy;
            }
        }

        return null;  // �� ������, ���� ������ �� ������� (��� �� ������ ���������)
    }
}
