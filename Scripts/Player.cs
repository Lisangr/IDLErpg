using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    public PlayerData playerData; // Reference to the PlayerData component
    public Image healthBar;       // Reference to the UI Image representing the health bar
    public TextMeshProUGUI hpText; // Reference to the TextMeshProUGUI for displaying health
    public GameObject defeatPanel;

    private int currentHealth;
    private int maxHealth;
    private const string DefenseKey = "PlayerDefense"; // Ключ для сохранения данных в PlayerPrefs

    void Start()
    {
        maxHealth = playerData.health;
        currentHealth = maxHealth;
        UpdateHealthDisplay();
    }
    
    public void TakeDamage(int damage)
    {
        int currentDefense = PlayerPrefs.GetInt(DefenseKey, playerData.defense);
        float defenseReduction = currentDefense / 420f; //210 - максимальный показатель защиты, т.е. при 210 защиты дамаг
        //будет 0 чего быть не должно, увеличим в 2 раза чтобы максимальный дамаг при максимальной защите был 0,5
        int actualDamage = Mathf.RoundToInt(damage - damage * defenseReduction);
        currentHealth -= actualDamage;
        currentHealth = Mathf.Max(currentHealth, 0);
        UpdateHealthDisplay();

        if (currentHealth <= 0)
        {
            Die();
        }
    }
   
    // Метод для восстановления здоровья
    public void RestoreHealth(float percentage)
    {
        int healthRestored = Mathf.RoundToInt(maxHealth * percentage);
        currentHealth += healthRestored;
        currentHealth = Mathf.Min(currentHealth, maxHealth); // Ensure health doesn't exceed max
        UpdateHealthDisplay();
        Debug.Log($"Restored {healthRestored} health. Current health: {currentHealth}");
    }

    private void UpdateHealthDisplay()
    {
        healthBar.fillAmount = (float)currentHealth / maxHealth;
        hpText.text = $"{currentHealth} / {maxHealth}";
    }

    private void Die()
    {
        if (currentHealth <= 0)
        {
            Destroy(this.gameObject);
            defeatPanel.SetActive(true);
        }
    }
}
