using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private UIManager uiManager;

    [SerializeField] private SceneLoader sceneLoader;
    
    [SerializeField] private LevelManagement levelManagement;

    [SerializeField] private Player playerPrefab;
    
    private PlayerData _playerData;
    
    private PlayerModel _playerModel;
    
    private MenuController _menuController;
    
    private SettingController _settingController;
    
    private GameplayController _gameplayController;
    
    private static bool _hasLoadedInitializeScene = false;
    
    private Player _player;
    
    private GameState _currentGameState;

    public override void Awake()
    {
        base.Awake();
        InitializeControllers();
    }
    
    private void InitializeControllers()
    {
        _menuController                   = new MenuController(uiManager.MenuView, uiManager.PlayerSelectView, _playerModel);
        _gameplayController               = new GameplayController(uiManager.GameplayView, _playerModel, levelManagement);
        _settingController                = new SettingController(uiManager.SettingView,levelManagement,_playerModel,uiManager.MenuView);
        _menuController.OnStartGame       = OnStartGame;
        _menuController.OnCharacterChange = OnCharacterChange;
        _gameplayController.OnMainMenu    = OnMainMenu;
        _gameplayController.OnPauseGame   = OnPauseGame;
        _gameplayController.OnResumeGame  = OnResumeGame;
        _gameplayController.OnRestartGame = OnRestartGame;
        
    }

    private void ChangeState(GameState newState)
    {
        _currentGameState = newState;

        switch (_currentGameState)
        {
            case GameState.MainMenu:
                HandleMainMenuState().Forget();
                break;

            case GameState.Gameplay:
                HandleGameplayState().Forget();
                break;

            case GameState.Pause:
                HandlePauseState();
                break;
                
        }
    }

    private void Start()
    {
        LoadPersistentGameObject().Forget();
        Application.targetFrameRate = 60;
    }

    private async UniTask HandleMainMenuState()
    {
        bool isLoadingSuccess = await sceneLoader.LoadScene(SceneLoader.SceneType.MainMenu);
        if(!isLoadingSuccess) return;
        
        levelManagement.ClearMap();
        _gameplayController.Dispose();
        _menuController.Init();
    }

    private async UniTask HandleGameplayState()
    {
        bool isLoadingSuccess = await sceneLoader.LoadScene(SceneLoader.SceneType.CoreGameLoop);
        if(!isLoadingSuccess) return;
        
        levelManagement.Generate();
        if (levelManagement.spawnPoint != null)
        {
            _player = Instantiate(playerPrefab, levelManagement.spawnPoint.position, Quaternion.identity);
            _menuController.Dispose();
            _gameplayController.SetNewPlayer(_player);
            _gameplayController.Init();
        }
    }
    
    private void HandlePauseState()
    {
        Time.timeScale = 0;
    }
    
    private async UniTask LoadPersistentGameObject()
    {
        if (!_hasLoadedInitializeScene)
        {
            Debug.Log("Loading Initialize Scene First Time!!!!");
            
            await sceneLoader.LoadScene(SceneLoader.SceneType.InitializeScene, () =>
            {
                _hasLoadedInitializeScene = true;
            });
            
            await sceneLoader.LoadScene(SceneLoader.SceneType.MainMenu, () =>
            {
                _menuController.Init();
            });
        }
    }
    
    private async UniTask OnRestartGame()
    {
        bool isLoadingSuccess = await sceneLoader.LoadScene(SceneLoader.SceneType.CoreGameLoop);
        if(!isLoadingSuccess) return;
        Time.timeScale = 1;
        levelManagement.Generate();
        if (levelManagement.spawnPoint != null)
        {
            _player = Instantiate(playerPrefab, levelManagement.spawnPoint.position, Quaternion.identity);
            _gameplayController.SetNewPlayer(_player);
            _gameplayController.Init();
        }
    }
    private void OnStartGame()
    {
        ChangeState(GameState.Gameplay);
    }

    private void OnPauseGame()
    {
        ChangeState(GameState.Pause);
    }

    private void OnResumeGame()
    {
        Time.timeScale = 1;
        _currentGameState = GameState.Gameplay;
    }

    private void OnMainMenu()
    {
        ChangeState(GameState.MainMenu);
    }

    private void OnCharacterChange(int index)
    {
        GameUtils.SelectedSkin = index;
        EventManager.TriggerEvent("SelectSkin", new Dictionary<string, object> { { "index", index } });
    }
}

public enum GameState
{
    MainMenu,
    Gameplay,
    Pause,
}
