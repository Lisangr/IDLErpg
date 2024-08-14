using UnityEngine;

public class VoidsForPanels : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject winPanel;
    private bool onClick;

    private void OnEnable()
    {
        Enemy.OnEnemyDestroy += WinPanel;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyDestroy -= WinPanel;
    }

    private void WinPanel()
    {
        Time.timeScale = 0f;
        winPanel.SetActive(true);
    }
    private void Start()
    {
        onClick = false;
        inventoryPanel.SetActive(false);
        winPanel.SetActive(false);
    }

    public void OnInventoryButtnoClick()
    { 
        onClick = !onClick;

        if (onClick)
        {
            inventoryPanel.SetActive(true);
        }
        else
        {
            inventoryPanel.SetActive(false);
        }
    }
}
