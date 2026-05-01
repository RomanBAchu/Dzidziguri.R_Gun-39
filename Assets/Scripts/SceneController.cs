using Zenject;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void OpenMainScene()
    {
        SceneManager.LoadScene(0); // Загружает MainScene по индексу 0
    }

    public void OpenGameScene()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Additive); // Аддитивно загружает GameScene по индексу 1
    }
}
