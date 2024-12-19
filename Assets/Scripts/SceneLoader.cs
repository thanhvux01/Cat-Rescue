using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : Singleton<SceneLoader>
{
    public enum SceneType
    {
        InitializeScene,
        CoreGameLoop,
        MainMenu
    }

    [Header("Scene Mappings")]
    
    [SerializeField] private string initializeScene;
    
    [SerializeField] private string coreGameLoop;
    
    [SerializeField] private string mainMenu;

    private Dictionary<SceneType, string> _sceneMappings;
    
    private List<AsyncOperation> _loadOperations = new List<AsyncOperation>();

    public override void Awake()
    {
        base.Awake();
        InitializeSceneMappings();
    }

    private void InitializeSceneMappings()
    {
        _sceneMappings = new Dictionary<SceneType, string>
        {
            { SceneType.InitializeScene, initializeScene },
            { SceneType.CoreGameLoop, coreGameLoop },
            { SceneType.MainMenu, mainMenu }
        };
    }

    public async UniTask<bool> LoadScene(SceneType sceneType, Action onComplete = null,bool forceLoad = true)
    {
        if (!_sceneMappings.TryGetValue(sceneType, out var sceneName) || string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError($"Scene name for {sceneType} is not set or invalid.");
            return false;
        }
        
        if(SceneManager.GetActiveScene().name == sceneName)
        {
            if (!forceLoad)
            {
                Debug.LogWarning($"Scene {sceneName} is already loaded. if you want to reload it set forceLoad to true.");
                return false;
            }
        }

        Debug.Log($"Loading scene: {sceneName}");
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        if (asyncOperation == null)
        {
            Debug.LogError($"Failed to load scene: {sceneName}");
            
        }

        _loadOperations.Add(asyncOperation);
        await asyncOperation.ToUniTask();
        Debug.Log($"Scene {sceneName} loaded successfully.");
        onComplete?.Invoke();
        return true;
    }
}

