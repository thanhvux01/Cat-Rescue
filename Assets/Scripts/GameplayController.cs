using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameplayController
{
    private GameplayView _gameplayView;
    private LevelManagement _levelManagement;
    private Player _player;
    private PlayerModel _playerModel;
    private bool _isReady;
    private float _boostSpeed;

    public Action OnMainMenu;
    public Action OnPauseGame;
    public Action OnResumeGame;
    public Func<UniTask> OnRestartGame;
    
    private List<GameObject> _catRescued = new List<GameObject>();
    public GameplayController(GameplayView gameplayView, PlayerModel playerModel, LevelManagement levelManagement)
    {
        _gameplayView = gameplayView;
        _levelManagement = levelManagement;
        _playerModel = playerModel;
    }

    public void Init()
    {
        BindUI();
        StartGameplay();
        SubscribePlayerEvents();
        InitializeTracking();
    }

    public void Dispose()
    {
        UnbindUI();
        UnsubscribePlayerEvents();
        _gameplayView.StopAllCoroutines();
        _catRescued.Clear();
    }

    private void BindUI()
    {
        _gameplayView.Show();

        _gameplayView.PauseButton.onClick.AddListener(PauseGame);
        _gameplayView.PausePopup.CancelButton.onClick.AddListener(ResumeGame);
        _gameplayView.PausePopup.ConfirmButton.onClick.AddListener(BackToMenu);
        _gameplayView.WinPopup.CancelButton.onClick.AddListener(BackToMenu);
        _gameplayView.WinPopup.ConfirmButton.onClick.AddListener(RestartGame);
        _gameplayView.LosePopup.ConfirmButton.onClick.AddListener(RestartGame);
        _gameplayView.LosePopup.CancelButton.onClick.AddListener(BackToMenu);
        _gameplayView.ScreenTouch.onClick.AddListener(BoostPlayerSpeed);
    }

    private void UnbindUI()
    {
        _gameplayView.Hide();

        _gameplayView.PauseButton.onClick.RemoveListener(PauseGame);
        _gameplayView.PausePopup.CancelButton.onClick.RemoveListener(ResumeGame);
        _gameplayView.PausePopup.ConfirmButton.onClick.RemoveListener(BackToMenu);
        _gameplayView.WinPopup.CancelButton.onClick.RemoveListener(BackToMenu);
        _gameplayView.WinPopup.ConfirmButton.onClick.RemoveListener(RestartGame);
        _gameplayView.LosePopup.ConfirmButton.onClick.RemoveListener(RestartGame);
        _gameplayView.LosePopup.CancelButton.onClick.RemoveListener(BackToMenu);
        _gameplayView.ScreenTouch.onClick.RemoveListener(BoostPlayerSpeed);
    }

    private void StartGameplay()
    {
        _gameplayView.ClearCatTrackPoints();
        _gameplayView.LevelInfoText.text = $"Seed:{_levelManagement.Seed }\nLevel:{_levelManagement.DifficultyLevel}";
        _gameplayView.CatCountText.text = _catRescued.Count.ToString();
        _gameplayView.CoinCountText.text = (_catRescued.Count * 100).ToString();
        _gameplayView.StartCoroutine(UpdateFillBar());
        _gameplayView.StartCoroutine(CountDown());
    }

    private void SubscribePlayerEvents()
    {
        _player.OnTriggerEndPoint += OnTriggerEndPoint;
        _player.OnPlayerDeath += OnPlayerDeath;
        _player.OnCatRescue += OnCatRescue;
    }

    private void UnsubscribePlayerEvents()
    {
        _player.OnTriggerEndPoint -= OnTriggerEndPoint;
        _player.OnPlayerDeath -= OnPlayerDeath;
        _player.OnCatRescue -= OnCatRescue;
    }

    private void InitializeTracking()
    {
        SetTrackingPoint();
        SetSpeedForCats();
    }

    private void PauseGame()
    {
        OnPauseGame?.Invoke();
        _gameplayView.PausePopup.gameObject.SetActive(true);
    }

    private void ResumeGame()
    {
        _gameplayView.PausePopup.gameObject.SetActive(false);
        OnResumeGame?.Invoke();
    }

    private void BackToMenu()
    {
        DisableAllPopup();
        OnResumeGame?.Invoke();
        OnMainMenu?.Invoke();
    }
    
    private void RestartGame()
    {
        DisableAllPopup();
        _isReady = false;
        Dispose();
        OnRestartGame?.Invoke();
    }

    private void OnCatRescue(GameObject cat)
    {
        if (!_catRescued.Contains(cat))
        {
            _catRescued.Add(cat);
            _gameplayView.CatCountText.text = _catRescued.Count.ToString();
            _gameplayView.CoinCountText.text = (_catRescued.Count * 100).ToString();
        }
    }

    private void OnPlayerDeath()
    {
        OnPauseGame?.Invoke();
        _gameplayView.LosePopup.gameObject.SetActive(true);
    }

    private void OnTriggerEndPoint()
    {
        OnPauseGame?.Invoke();
        _gameplayView.WinPopup.gameObject.SetActive(true);
    }

    private void SetTrackingPoint()
    {
        float endPosition = _levelManagement.EndZPosition + _levelManagement.MaxZCatSpawn;
        float trackingBarWidth = _gameplayView.CatTrackingBar.FillBarWidth;

        _levelManagement.SpawnedCats.ForEach(cat =>
        {
            float mappedPosition = cat.transform.position.z * (trackingBarWidth / endPosition);
            _gameplayView.AddCatTrackPoint(new Vector2(mappedPosition, 0));
        });
    }

    private void SetSpeedForCats()
    {
        _levelManagement.SpawnedCats.ForEach(cat =>
        {
            cat.followSpeed = _player.FreeLookMovementSpeed * 0.8f;
        });
    }

    private void BoostPlayerSpeed()
    {
        if (_isReady) return;

        _boostSpeed += _playerModel.GetPlayerData.Speed * GameUtils.BoostSpeedMultiplier;
        _boostSpeed = Mathf.Min(_boostSpeed, _playerModel.GetPlayerData.Speed * GameUtils.MaxBoostSpeedMultiplier);
        Debug.Log($"Boost Speed: {_boostSpeed}");
    }

    private IEnumerator UpdateFillBar()
    {
        while (_player.transform.position.z < _levelManagement.endPoint.transform.position.z)
        {
            yield return null;
            _gameplayView.CatTrackingBar.FillBar.value =
                _player.transform.position.z / _levelManagement.endPoint.transform.position.z;
        }
    }

    private IEnumerator CountDown()
    {
        for (int second = 0; second <= 3; second++)
        {
            yield return new WaitForSeconds(1);
            _gameplayView.CountDownText.text = $"<size=300>{3 - second}\n<size=50>TAP TO BOOST YOUR SPEED</size>";
        }

        _isReady = true;
        _gameplayView.StartCoroutine(ReduceBoostSpeed());
        _gameplayView.CountDownText.text = string.Empty;
        _levelManagement.tsunami.isMoving = true;
    }

    private IEnumerator ReduceBoostSpeed()
    {
        while (_boostSpeed > 0 && _isReady)
        {
            yield return new WaitForSeconds(0.2f);
            _player.FreeLookMovementSpeed = _playerModel.GetPlayerData.Speed + _boostSpeed;
            Debug.Log(_player.FreeLookMovementSpeed);
            _boostSpeed -= GameUtils.BoostSpeedMultiplier;
        }
    }
    private void DisableAllPopup()
    {
        _gameplayView.WinPopup.gameObject.SetActive(false);
        _gameplayView.LosePopup.gameObject.SetActive(false);
        _gameplayView.PausePopup.gameObject.SetActive(false);
    }

    public void SetNewPlayer(Player player)
    {
        _player = player;
        _player.FreeLookMovementSpeed = _playerModel.GetPlayerData.Speed;
    }
}
