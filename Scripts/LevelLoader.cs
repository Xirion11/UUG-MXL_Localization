using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

    // Static singleton property
    public static LevelLoader Instance { get; private set; }

    // Use this for initialization
    void Start() {
        // Singleton initialization. If the instance already exists it should self-destruct to avoid duplicates
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            Debug.Log("An instance of LevelLoader already exists");
        }
        else
        {
            // Initialize singleton instance
            Instance = this;

            //Allow GameObject to exist through all execution
            DontDestroyOnLoad(gameObject);
        }
    }

    public void reloadCurrentScene()
    {
    	int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    	SceneManager.LoadScene(currentSceneIndex);
    }
}
