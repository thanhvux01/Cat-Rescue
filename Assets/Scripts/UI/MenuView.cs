using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class MenuView : View
{
   //todo should make a list if there are more button in future

   [field:SerializeField] public UpgradeButton UpgradeStaminaBtn { get; private set; }
   [field:SerializeField] public UpgradeButton UpgradeIncomeBtn { get; private set; }
   [field:SerializeField] public UpgradeButton UpgradeSpeedBtn { get; private set; }
   [field:SerializeField] public TMP_Text MoneyText { get; private set; }
   [field:SerializeField] public TMP_Text StaminaText { get; private set; }
   [field:SerializeField] public Button MoneyTouch { get; private set; }
   [field:SerializeField] public Button StartGameBtn { get; private set; }
   [field:SerializeField] public Button SelectCharacterBtn { get; private set; }
   [field:SerializeField] public Button SettingButton { get; private set; }
   
   [field:SerializeField] public AudioSource AudioSource { get; private set; }
   [field:SerializeField] public Slider StaminaSlider { get; private set; }
   
   
}
