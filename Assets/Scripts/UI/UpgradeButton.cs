using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] private Button button;
    
    [SerializeField] private TMP_Text costText;
    
    [SerializeField] private TMP_Text valueText;

    public void SetValueAndCost(float value , float cost)
    {
        costText.text  = Math.Round(cost, 2).ToString(CultureInfo.InvariantCulture);
        valueText.text = Math.Round(value, 2).ToString(CultureInfo.InvariantCulture);
    }

    public void AddListener(Action action)
    {
        button.onClick.AddListener(()=>
        {
            action?.Invoke();
        });
    }
    
    public void RemoveListener(Action action)
    {
        button.onClick.RemoveListener(()=>
        {
            action?.Invoke();
        });
    }
    
}
