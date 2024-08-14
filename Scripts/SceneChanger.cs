using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string sceneToEscape = "Map";

    public void EscapeOnClick()
    {
        SceneManager.LoadScene(sceneToEscape);
        Time.timeScale = 1.0f;
    }

    public void RestartCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
}
