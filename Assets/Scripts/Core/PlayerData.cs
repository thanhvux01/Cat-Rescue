using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public float Speed { get; private set; }
    public float Stamina { get; private set; }
    public float Income { get; private set; }
    public float Money { get; private set; }
    public float CurrentStamina { get; private set; }
    public PlayerData()
    {
        Load();
    }

    private void Load()
    {
        Speed          = PlayerPrefs.GetFloat("Speed", 0.2f);
        Stamina        = PlayerPrefs.GetFloat("Stamina", 7);
        Money          = PlayerPrefs.GetFloat("Money", 0);
        Income         = PlayerPrefs.GetFloat("Income", 1);
        CurrentStamina = PlayerPrefs.GetFloat("CurrentStamina", 0);
    }
    public void UpdateSpeed(float value)
    {
        if (value >= 0)
        {
            Speed = value;
            Save(); 
        }
    }

    public void UpdateStamina(float value)
    {
        if (value >= 0)
        {
            Stamina = value;
            Save(); 
        }
    }
    
    public void UpdateMoney(float value)
    {
        if (value >= 0)
        {
            Money = value;
            Save(); 
        }
    }
    
    public void UpdateIncome(float value)
    {
        if (value >= 0)
        {
            Income = value;
            Save(); 
        }
    }
    
    public void UpdateCurrentStamina(float value)
    {
        if (value >= 0)
        {
            CurrentStamina = value;
            Save(); 
        }
    }
    
    public void Save()
    {
        PlayerPrefs.SetFloat("Speed", Speed);
        PlayerPrefs.SetFloat("Stamina", Stamina);
        PlayerPrefs.SetFloat("Money", Money);
        PlayerPrefs.SetFloat("Income", Income);
        PlayerPrefs.SetFloat("CurrentStamina", CurrentStamina);
        PlayerPrefs.Save();
    }
    
    public void Clear()
    {
        PlayerPrefs.DeleteKey("Speed");
        PlayerPrefs.DeleteKey("Stamina");
        PlayerPrefs.DeleteKey("Money");
        PlayerPrefs.DeleteKey("Income");
        PlayerPrefs.DeleteKey("CurrentStamina");
        Load(); 
        PlayerPrefs.Save();
    }
    
   
}