

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayView : View
{
    [field:SerializeField] public TrackingBar CatTrackingBar { get; private set; }
    
    [field:SerializeField] public TMP_Text CatCountText { get; private set; }
    
    [field:SerializeField] public TMP_Text CoinCountText { get; private set; }
    [field:SerializeField] public TMP_Text CountDownText { get; private set; }
    
    [field:SerializeField] public TMP_Text LevelInfoText { get; private set; }
    [field:SerializeField] public Button PauseButton { get; private set; }
    [field:SerializeField] public Button ScreenTouch { get; private set; }
    [field:SerializeField] public Popup PausePopup { get; private set; }
    [field:SerializeField] public Popup LosePopup { get; private set; }
    [field:SerializeField] public Popup WinPopup { get; private set; }
    
    
    public void AddCatTrackPoint(Vector2 position)
    {
           CatTrackingBar.AddNewTrackPoint(position);
    }
    
    public void ClearCatTrackPoints()
    {
        CatTrackingBar.ClearTrackPoints();
    }

 
}