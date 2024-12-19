using System;
using System.Collections;
using UnityEngine;

public class MenuController
{
    private MenuView _menuView;
    private PlayerSelectView _playerSelectView;
    private PlayerModel _playerModel;
    private PlayerData _playerData;
    private int _currentCharacterIndex = 0;

    public Action OnStartGame;
    public Action<int> OnCharacterChange;

    public MenuController(MenuView menuView, PlayerSelectView playerSelectView, PlayerModel playerModel)
    {
        _menuView = menuView;
        _playerSelectView = playerSelectView;
        _playerModel = playerModel;
        _playerData = _playerModel.GetPlayerData;
    }

    public void Init()
    {
        BindUI();
        StartMenu();
    }

    public void Dispose()
    {
        UnbindUI();
        _menuView.StopAllCoroutines();
    }

    private void BindUI()
    {
        _menuView.Show();

        _menuView.UpgradeSpeedBtn.AddListener(UpgradeSpeed);
        _menuView.UpgradeStaminaBtn.AddListener(UpgradeStamina);
        _menuView.UpgradeIncomeBtn.AddListener(UpgradeIncome);
        _menuView.MoneyTouch.onClick.AddListener(TryAddMoney);
        _menuView.SelectCharacterBtn.onClick.AddListener(SwitchCharacterMode);
        _menuView.StartGameBtn.onClick.AddListener(() => OnStartGame?.Invoke());

        _playerSelectView.PreviousButton.onClick.AddListener(PreviousCharacter);
        _playerSelectView.NextButton.onClick.AddListener(NextCharacter);
        _playerSelectView.ConfirmButton.onClick.AddListener(SwitchMenuMode);
    }

    private void UnbindUI()
    {
        _menuView.Hide();

        _menuView.UpgradeSpeedBtn.RemoveListener(UpgradeSpeed);
        _menuView.UpgradeStaminaBtn.RemoveListener(UpgradeStamina);
        _menuView.UpgradeIncomeBtn.RemoveListener(UpgradeIncome);
        _menuView.MoneyTouch.onClick.RemoveListener(TryAddMoney);
        _menuView.SelectCharacterBtn.onClick.RemoveListener(SwitchCharacterMode);
        _menuView.StartGameBtn.onClick.RemoveAllListeners();

        _playerSelectView.PreviousButton.onClick.RemoveListener(PreviousCharacter);
        _playerSelectView.NextButton.onClick.RemoveListener(NextCharacter);
        _playerSelectView.ConfirmButton.onClick.RemoveListener(SwitchMenuMode);
    }

    private void StartMenu()
    {
        UpdateView();
        _menuView.StartCoroutine(IncreaseStaminaEverySeconds());
    }

    private void TryAddMoney()
    {
        bool success = _playerModel.TryAddMoney();
        if(!success) return;
        
        _menuView.AudioSource.Play();
        UpdateView();
    }

    private void UpgradeSpeed()
    {
        _playerModel.UpgradeSpeed();
        UpdateView();
    }

    private void UpgradeStamina()
    {
        _playerModel.UpgradeStamina();
        UpdateView();
    }

    private void UpgradeIncome()
    {
        _playerModel.UpgradeIncome();
        UpdateView();
    }

    private void SwitchCharacterMode()
    {
        _playerSelectView.Show();
        _menuView.StopCoroutine(IncreaseStaminaEverySeconds());
        _menuView.Hide();
    }

    private void SwitchMenuMode()
    {
        _playerSelectView.Hide();
        _menuView.Show();
        _menuView.StartCoroutine(IncreaseStaminaEverySeconds());
    }

    private void PreviousCharacter() => ChangeCharacter(-1);

    private void NextCharacter() => ChangeCharacter(1);

    private void ChangeCharacter(int index)
    {
        _currentCharacterIndex += index;
        _currentCharacterIndex = Mathf.Clamp(_currentCharacterIndex, 0, GameUtils.MaxCharacters);
        OnCharacterChange?.Invoke(_currentCharacterIndex);
    }

    private IEnumerator IncreaseStaminaEverySeconds()
    {
        while (true)
        {
            //10s recovery whole enegery; 
            yield return new WaitForSeconds(0.1f);
            _playerModel.AddStamina(_playerData.Stamina/100);
            UpdateView();
        }
    }

    private void UpdateView()
    {
        _menuView.UpgradeSpeedBtn.SetValueAndCost(_playerData.Speed, _playerModel.GetUpgradeCost("Speed"));
        _menuView.UpgradeStaminaBtn.SetValueAndCost(_playerData.Stamina, _playerModel.GetUpgradeCost("Stamina"));
        _menuView.UpgradeIncomeBtn.SetValueAndCost(_playerData.Income, _playerModel.GetUpgradeCost("Income"));
        _menuView.StaminaText.SetText($"{Math.Round(_playerData.CurrentStamina, 2)} / {_playerData.Stamina}");
        _menuView.MoneyText.SetText(_playerData.Money.ToString());
        _menuView.StaminaSlider.value = _playerData.CurrentStamina / _playerData.Stamina;
    }
}