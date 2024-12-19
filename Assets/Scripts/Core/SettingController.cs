using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingController
{
    private SettingView _settingView;

    private LevelManagement _levelManagement;
    
    private PlayerModel _playerModel;
    public SettingController(SettingView settingView,LevelManagement levelManagement,PlayerModel playerModel,MenuView menuView)
    {
        this._settingView  = settingView;
        this._levelManagement = levelManagement;
        this._playerModel  = playerModel;
        menuView.SettingButton.onClick.AddListener(Init);
    }

    public void Init()
    {
        BindUI();
    }
    private void Dispose()
    {
        UnbindUI();
    }

    private void BindUI()
    {
        _settingView.Show();
        _settingView.CloseButton.onClick.AddListener(Dispose);
        _settingView.ClearDataButton.onClick.AddListener(ClearPlayerData);
        _settingView.RandomSeedSwitch.OnValueChanged.AddListener(ChangeRandomSeed);
        _settingView.InputField.onValueChanged.AddListener(SetWorldSeed);
        _settingView.LevelDropDown.onValueChanged.AddListener(ChangeLevel);
        _settingView.CheatButton.onClick.AddListener(OnCheatButton);
    }
    private void UnbindUI()
    {
        _settingView.Hide();
        _settingView.CloseButton.onClick.RemoveListener(Dispose);
        _settingView.ClearDataButton.onClick.RemoveListener(ClearPlayerData);
        _settingView.RandomSeedSwitch.OnValueChanged.RemoveListener(ChangeRandomSeed);
        _settingView.InputField.onValueChanged.RemoveListener(SetWorldSeed);
        _settingView.LevelDropDown.onValueChanged.RemoveListener(ChangeLevel);
        _settingView.CheatButton.onClick.RemoveListener(OnCheatButton);

    }
   
    public void ClearPlayerData()
    {
        _playerModel.ClearPlayerData();
    }
    private void SetWorldSeed(string value)
    {
        Debug.Log("SetWorldSeed: " + value);
    }
    
    private void ChangeRandomSeed(bool value)
    {
        _levelManagement.SetRandomWorldSeed(value);
    }
    
    private void ChangeLevel(int value)
    {
        _levelManagement.SetDifficultyLevel(value+1);
    }
    
    private void OnCheatButton()
    {
        _playerModel.Add1000Coin();
    }
        
        
    
}
