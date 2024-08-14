using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public EnemyData enemyData; // Reference to the EnemyData scriptable object
    public Image healthBar;     // Reference to the UI Image representing the health bar
    public TextMeshProUGUI hpText;
    private int currentHealth;
    private int maxHealth;


    // Reference to the player object
    public Player player;
    public delegate void DeathAction(int exp);
    public static event DeathAction OnEnemyDeath;

    public delegate void LootAction();
    public static event LootAction OnEnemyDestroy;
    void Start()
    {
        // Initialize the enemy's properties from the scriptable object
        maxHealth = enemyData.health;
        currentHealth = maxHealth;

        // Initialize the health display
        UpdateHealthDisplay();
    }

    // Method to take damage
    public void TakeDamage(int damage)
    {
        // Calculate the actual damage after defense reduction
        float defenseReduction = (100f - enemyData.defense) / 100f;
        int actualDamage = Mathf.RoundToInt(damage * defenseReduction);

        // Reduce current health by the actual damage
        currentHealth -= actualDamage;

        // Clamp the current health to ensure it doesn't go below zero
        currentHealth = Mathf.Max(currentHealth, 0);

        // Update the health bar UI and health text
        UpdateHealthDisplay();

        // Check if the enemy is dead
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Method to update the health bar UI and health text
    private void UpdateHealthDisplay()
    {
        // Update the fill amount of the health bar based on current health
        healthBar.fillAmount = (float)currentHealth / maxHealth;

        // Update the health text
        hpText.text = $"{currentHealth} / {maxHealth}";
    }

    // Method to handle the enemy's death
    private void Die()
    {
        int exp = enemyData.exp;
        // Implement death logic here (e.g., play animation, destroy the enemy)
        Debug.Log(enemyData.enemyName + " has been defeated!");
        OnEnemyDeath?.Invoke(exp);
        OnEnemyDestroy?.Invoke();
        // Example: Destroy the enemy game object
        Destroy(gameObject);
    }

    // Method for the enemy to attack the player
    public void AttackPlayer()
    {
        player = FindObjectOfType<Player>();

        if (player != null)
        {
            
            // Calculate damage based on enemy's attack power
            int damage = enemyData.attack;

            // Log attack action
            Debug.Log($"{enemyData.enemyName} attacks for {damage} damage!");

            // Apply damage to the player
            player.TakeDamage(damage);
        }
        else
        {
            Debug.Log("»√–Œ  Õ≈ Õ¿…ƒ≈Õ œŒ◊»Õ» ◊“Œ-“Œ");
        }
    }
    /*
    public EnemyData enemyData; // Reference to the EnemyData scriptable object
    public Image healthBar;     // Reference to the UI Image representing the health bar
    public TextMeshProUGUI hpText;
    private int currentHealth;
    private int maxHealth;

    void Start()
    {
        // Initialize the enemy's properties from the scriptable object
        maxHealth = enemyData.health;
        currentHealth = maxHealth;

        // Initialize the health display
        UpdateHealthDisplay();
    }

    // Method to take damage
    public void TakeDamage(int damage)
    {
        // Calculate the actual damage after defense reduction
        float defenseReduction = (100f - enemyData.defense) / 100f;
        int actualDamage = Mathf.RoundToInt(damage * defenseReduction);

        // Reduce current health by the actual damage
        currentHealth -= actualDamage;

        // Clamp the current health to ensure it doesn't go below zero
        currentHealth = Mathf.Max(currentHealth, 0);

        // Update the health bar UI and health text
        UpdateHealthDisplay();

        // Check if the enemy is dead
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Method to update the health bar UI and health text
    private void UpdateHealthDisplay()
    {
        // Update the fill amount of the health bar based on current health
        healthBar.fillAmount = (float)currentHealth / maxHealth;

        // Update the health text
        hpText.text = $"{currentHealth} / {maxHealth}";
    }

    // Method to handle the enemy's death
    private void Die()
    {
        // Implement death logic here (e.g., play animation, destroy the enemy)
        Debug.Log(enemyData.enemyName + " has been defeated!");

        // Example: Destroy the enemy game object
        Destroy(gameObject);
    }*/
}
