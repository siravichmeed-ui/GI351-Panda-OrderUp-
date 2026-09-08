using UnityEngine;
using UnityEngine.SceneManagement;

public class BGM : MonoBehaviour
{
    private static BGM instance;
    private AudioSource audioSource;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            audioSource.Stop();
        }
    }
}