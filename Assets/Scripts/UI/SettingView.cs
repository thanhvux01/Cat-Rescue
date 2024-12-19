using JetBrains.Annotations;
using RainbowArt.CleanFlatUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingView : View
{
    [field:SerializeField] public Button CloseButton { get; private set; }
    [field:SerializeField] public Button ClearDataButton { get; private set; }
    [field:SerializeField] public SwitchSimple RandomSeedSwitch { get; private set; }
    [field:SerializeField] public TMP_InputField InputField { get; private set; }
    
    [field:SerializeField] [NotNull] public Button CheatButton { get; private set; }
    [field:SerializeField] public TMP_Dropdown LevelDropDown { get; private set; }
    
    
}