using TMPro;
using UnityEngine;

public class AcceptedData : MonoBehaviour
{    
    public TextMeshProUGUI nameText;
    public EnemyData enemyData;

    private void Start()
    {
        if (enemyData != null)
        {           
            nameText.text = enemyData.enemyName;
        }
        else
        {
            Debug.LogError("EnemyData не назначен в инспекторе!");
        }


    }
}
