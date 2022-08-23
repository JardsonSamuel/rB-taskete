using System;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [SerializeField] private GameObject playerAndCameraPrefab;

    [SerializeField] private string locationToLoad;

    [SerializeField] private string guiScene;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Initialization")
        {
            StartGameFromInitialization();
        }
        else
        {
            StartGameFromLevel();
        }
    }


    private void StartGameFromLevel()
    {
        SceneManager.LoadScene(guiScene, LoadSceneMode.Additive);

        Vector3 starPosition = GameObject.Find("PlayerStart").transform.position;

        Instantiate(playerAndCameraPrefab, starPosition, Quaternion.identity);

    }

    void StartGameFromInitialization()
    {
        SceneManager.LoadScene(guiScene);
        SceneManager.LoadSceneAsync(locationToLoad, LoadSceneMode.Additive).completed += operation =>
        {
            Scene locationScene = default;
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                locationScene = SceneManager.GetSceneAt(i);
                break;
            }

            if (locationScene != default) SceneManager.SetActiveScene(locationScene);

            Vector3 starPosition = GameObject.Find("PlayerStart").transform.position;

            Instantiate(playerAndCameraPrefab, starPosition, Quaternion.identity);
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
