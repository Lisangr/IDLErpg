using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExpBar : MonoBehaviour
{
    public Image expImage;
    public TextMeshProUGUI expText;
    public TextMeshProUGUI lvlText; // Поле для отображения уровня
    private int expForLevelUp = 100;
    private int currentExp;
    private int currentLevel = 1; // Текущий уровень

    private void Start()
    {
        LoadExperience();
        UpdateUI();
    }

    private void OnEnable()
    {
        Enemy.OnEnemyDeath += AddExperience;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyDeath -= AddExperience;
    }

    private void AddExperience(int exp)
    {
        currentExp += exp;

        // Повышение уровня, если достигнут требуемый опыт
        while (currentExp >= expForLevelUp)
        {
            LevelUp();
        }

        SaveExperience();
        UpdateUI();
    }

    private void LevelUp()
    {
        currentExp -= expForLevelUp; // Перенос остатка опыта на следующий уровень
        currentLevel++; // Увеличение уровня
        expForLevelUp = Mathf.CeilToInt(expForLevelUp * 1.2f); // Увеличение опыта для следующего уровня на 10%
    }

    private void UpdateUI()
    {
        expImage.fillAmount = (float)currentExp / expForLevelUp;
        expText.text = "EXP: " + currentExp + "/" + expForLevelUp;
        lvlText.text = "Level: " + currentLevel; // Обновление текста уровня
    }

    private void SaveExperience()
    {
        PlayerPrefs.SetInt("CurrentExp", currentExp);
        PlayerPrefs.SetInt("CurrentLevel", currentLevel); // Сохранение уровня
        PlayerPrefs.SetInt("ExpForLevelUp", expForLevelUp); // Сохранение опыта для следующего уровня
        PlayerPrefs.Save(); // Сохраняем изменения
    }

    private void LoadExperience()
    {
        currentExp = PlayerPrefs.GetInt("CurrentExp", 0); // Загружаем опыт, если его нет, то 0
        currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1); // Загружаем уровень, если его нет, то 1
        expForLevelUp = PlayerPrefs.GetInt("ExpForLevelUp", 100); // Загружаем требуемый опыт для уровня
    }
}
