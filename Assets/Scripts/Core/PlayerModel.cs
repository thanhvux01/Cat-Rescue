using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel
{
       private readonly PlayerData _playerData;

       private const float SpeedMultiplier = 0.2f;

       private const float IncomeMultiplier = 1f;
       
       private const float StaminaMultiplier = 7f;
       
       public readonly float MoneyToUpgrade = 10f;
       public PlayerData GetPlayerData => _playerData;
       public PlayerModel()
       {
           _playerData = new PlayerData();
       }
       
       public void UpgradeSpeed()
       {

           float moneyNeedToLevel = _playerData.Speed / SpeedMultiplier * MoneyToUpgrade;
           
            if (_playerData.Money >= moneyNeedToLevel)
            {
                _playerData.UpdateMoney(_playerData.Money - moneyNeedToLevel);
                _playerData.UpdateSpeed(_playerData.Speed + SpeedMultiplier);
            }
       }
       
       public void UpgradeStamina()
       {
          
          float moneyNeedToLevel = _playerData.Stamina / StaminaMultiplier * MoneyToUpgrade;
          
            if (_playerData.Money >= moneyNeedToLevel)
            {
                _playerData.UpdateMoney(_playerData.Money - moneyNeedToLevel);
                _playerData.UpdateStamina(_playerData.Stamina + StaminaMultiplier);
            }
       }
       
       public void UpgradeIncome()
       {
          float moneyNeedToLevel = _playerData.Income / IncomeMultiplier * MoneyToUpgrade;
          
            if (_playerData.Money >= moneyNeedToLevel)
            {
                _playerData.UpdateMoney(_playerData.Money - moneyNeedToLevel);
                _playerData.UpdateIncome(_playerData.Income + IncomeMultiplier);
            }
       }

       public bool TryAddMoney()
       {
           if (_playerData.CurrentStamina >= _playerData.Speed)
           {
               _playerData.UpdateCurrentStamina(_playerData.CurrentStamina - _playerData.Speed);
               _playerData.UpdateMoney(_playerData.Money + _playerData.Income);
               return true;
           }

           return false;
       }
       
       public float GetUpgradeCost(string upgradeType)
       {
           return upgradeType switch
           {
               "Speed" =>  _playerData.Speed / SpeedMultiplier * MoneyToUpgrade,
               "Stamina" => _playerData.Stamina / StaminaMultiplier * MoneyToUpgrade,
               "Income" => _playerData.Income / IncomeMultiplier * MoneyToUpgrade,
               _ => 0
           };
       }

       public void AddStamina(float value)
       {
           _playerData.UpdateCurrentStamina(Mathf.Min(_playerData.CurrentStamina + value, _playerData.Stamina));
       }

       public void Add1000Coin()
       {
           _playerData.UpdateMoney(100000);
       }

       public void ClearPlayerData()
       {
           _playerData.Clear();
       }


}
