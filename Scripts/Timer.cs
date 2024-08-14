using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float startTime = 5f;  // Duration for each player's turn
    public float battleTime = 30f; // Total allowed battle time

    [SerializeField] private TextMeshProUGUI battleTimeText; // Text field for battle time
    [SerializeField] private TextMeshProUGUI turnTimeText;   // Text field for turn time
    [SerializeField] private GameObject panel;               // Panel to show/hide based on player input
    [SerializeField] private GameObject defeatPanel;
    [HideInInspector] public float currentTurnTime;
    private float totalElapsedTime;
    private bool playerInputBlocked;

    public Enemy enemy; // Reference to the enemy object
    public void Start()
    {
        defeatPanel.SetActive(false);
        Time.timeScale = 1.0f;

        currentTurnTime = startTime;
        totalElapsedTime = 0f;
        playerInputBlocked = false;

        // Initialize the text fields
        UpdateBattleTimeText();
        UpdateTurnTimeText();
    }

    public void Update()
    {
        if (totalElapsedTime >= battleTime)
        {
            HandleBattleLost();
            return;
        }

        if (currentTurnTime > 0)
        {
            currentTurnTime -= Time.deltaTime;
            totalElapsedTime += Time.deltaTime;

            UpdateBattleTimeText();
            UpdateTurnTimeText();
            MovePlayer();
        }
        else
        {
            playerInputBlocked = true;
            StartCoroutine(MoveEnemy());
        }
    }

    private void HandleBattleLost()
    {
        Time.timeScale = 0f;
        defeatPanel.SetActive(true);
        playerInputBlocked = true;
    }

    private void UpdateBattleTimeText()
    {
        float remainingBattleTime = battleTime - totalElapsedTime;
        battleTimeText.text = $"Battle Time Left: {Mathf.Ceil(remainingBattleTime)}s";
    }

    private void UpdateTurnTimeText()
    {
        turnTimeText.text = $"Turn Time: {Mathf.Ceil(currentTurnTime)}s";
    }

    public void MovePlayer()
    {
        panel.SetActive(!playerInputBlocked);
    }

    public IEnumerator MoveEnemy()
    {
        if (playerInputBlocked)
        {
            // Reset turn time and block player input
            currentTurnTime = startTime;
            UpdateTurnTimeText();
            panel.SetActive(false);

            // Simulate enemy thinking or preparing attack
            yield return new WaitForSeconds(1f);

            enemy = FindObjectOfType<Enemy>();
            // Enemy attacks player
            if (enemy != null)
            {
                enemy.AttackPlayer();
            }
            else
            {
                Debug.Log("À ÃÄÅ ÆÅ ÂÐÀÃ");
            }

            // Wait for a short duration after attack
            yield return new WaitForSeconds(2f);

            // Reset state for next player's turn
            playerInputBlocked = false;
            currentTurnTime = startTime;
            yield break;
        }
    }

    public void ResetTimerAndMoveEnemy()
    {
        currentTurnTime = startTime;
        UpdateTurnTimeText();
        panel.SetActive(false);
        playerInputBlocked = true;
        StartCoroutine(MoveEnemy());
    }
    /*
    public float startTime = 5f;  // Duration for each player's turn
    public float battleTime = 30f; // Total allowed battle time
    public Enemy enemy; // Reference to the enemy object
    [SerializeField] private TextMeshProUGUI battleTimeText; // Text field for battle time
    [SerializeField] private TextMeshProUGUI turnTimeText;   // Text field for turn time
    [SerializeField] private GameObject panel;               // Panel to show/hide based on player input
    [SerializeField] private GameObject defeatPanel;
    [HideInInspector] public float currentTurnTime;
    private float totalElapsedTime;
    private bool playerInputBlocked;

    public void Start()
    {
        defeatPanel.SetActive(false);
        Time.timeScale = 1.0f;

        currentTurnTime = startTime;
        totalElapsedTime = 0f;
        playerInputBlocked = false;

        // Initialize the text fields
        UpdateBattleTimeText();
        UpdateTurnTimeText();
    }

    public void Update()
    {
        if (totalElapsedTime >= battleTime)
        {
            HandleBattleLost();
            return;
        }

        if (currentTurnTime > 0)
        {
            currentTurnTime -= Time.deltaTime;
            totalElapsedTime += Time.deltaTime;

            UpdateBattleTimeText();
            UpdateTurnTimeText();
            MovePlayer();
        }
        else
        {
            playerInputBlocked = true;
            StartCoroutine(MoveEnemy());
        }
    }

    private void HandleBattleLost()
    {
        Time.timeScale = 0f;
        defeatPanel.SetActive(true);
        playerInputBlocked = true;        
    }

    private void UpdateBattleTimeText()
    {
        float remainingBattleTime = battleTime - totalElapsedTime;
        battleTimeText.text = $"Battle Time Left: {Mathf.Ceil(remainingBattleTime)}s";
    }

    private void UpdateTurnTimeText()
    {
        turnTimeText.text = $"Turn Time: {Mathf.Ceil(currentTurnTime)}s";
    }

    public void MovePlayer()
    {
        panel.SetActive(!playerInputBlocked);
    }

    public IEnumerator MoveEnemy()
    {
        if (playerInputBlocked)
        {
            currentTurnTime = startTime;
            UpdateTurnTimeText(); // Update turn time text upon reset
            panel.SetActive(false);

            yield return new WaitForSeconds(3f);
            // enemy.GenerateRandomAttack();

            playerInputBlocked = false;
            currentTurnTime = startTime;
            yield break;
        }
    }

    public void ResetTimerAndMoveEnemy()
    {
        currentTurnTime = startTime;
        UpdateTurnTimeText(); // Reset and update turn time
        panel.SetActive(false);
        playerInputBlocked = true;
        StartCoroutine(MoveEnemy());
    }*/
}