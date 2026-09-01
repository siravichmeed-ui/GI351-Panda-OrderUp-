using UnityEngine;
using UnityEngine.SceneManagement;
public class Scene : MonoBehaviour
{
    public void GoToGame()
    {
        SceneManager.LoadScene(1);
    }
    public void RetryGame()
    {
        SceneManager.LoadScene(1);

    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
