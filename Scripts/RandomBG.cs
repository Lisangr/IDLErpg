using UnityEngine;
using UnityEngine.UI;

public class RandomBG : MonoBehaviour
{
    public Sprite[] sprites; 
    private Image imageComponent;

    private void Awake()
    {
        imageComponent = GetComponent<Image>();
    }
    void Start()
    {
        if (sprites.Length > 0)
        {
            int randomIndex = Random.Range(0, sprites.Length);
            imageComponent.sprite = sprites[randomIndex];
        }
        else
        {
            Debug.LogWarning("Массив спрайтов пустой!");
        }
    }
}
