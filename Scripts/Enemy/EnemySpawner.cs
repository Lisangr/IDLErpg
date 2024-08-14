using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public EnemyData[] enemiesData;  // Массив данных о врагах
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
                Debug.LogWarning("Префаб врага не найден!");
            }
        }
        else
        {
            Debug.LogWarning("Массив данных о врагах пустой!");
        }
    }

    private EnemyData GetRandomEnemy()
    {
        float totalChance = 0f;

        // Рассчитываем общую вероятность появления
        foreach (var enemy in enemiesData)
        {
            totalChance += enemy.spawnChance;
        }

        // Генерируем случайное число от 0 до общей вероятности
        float randomValue = Random.Range(0f, totalChance);
        float cumulativeChance = 0f;

        // Выбираем врага на основе его шанса появления
        foreach (var enemy in enemiesData)
        {
            cumulativeChance += enemy.spawnChance;
            if (randomValue <= cumulativeChance)
            {
                return enemy;
            }
        }

        return null;  // На случай, если ничего не выбрано (что не должно случиться)
    }
}
