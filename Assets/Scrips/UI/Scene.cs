using UnityEngine;
using UnityEngine.SceneManagement;
public class Scene : MonoBehaviour
{
    public void GoToGame()
    {
        SceneManager.LoadScene(1);
    }
}
