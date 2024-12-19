using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
   [field:SerializeField]  public MenuView MenuView { get; private set; }
   [field:SerializeField]  public GameplayView GameplayView { get; private set; }
   [field:SerializeField]  public PlayerSelectView PlayerSelectView { get; private set; }
   [field:SerializeField]  public SettingView SettingView { get; private set; }
}
