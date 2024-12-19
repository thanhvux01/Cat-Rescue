using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class SkinSelector : MonoBehaviour

{  [SerializeField] private GameObject[] skins;

    private int currentIndex = 0;
    
    private void Start()
    {
        SelectSkin(GameUtils.SelectedSkin);
        EventManager.StartListening("SelectSkin",SelectSkin);
    }

    private void OnDestroy()
    {
       EventManager.StopListening("SelectSkin",SelectSkin);
    }

    private void SelectSkin(Dictionary<string,object> message)
    {
        int index = (int)message["index"];
       SelectSkin(index);
    }

    private void SelectSkin(int index)
    {
        if (index < 0 || index >= skins.Length) return;
        
        skins.ToList().ForEach(skin => skin.SetActive(false));
        skins[index].SetActive(true);
        currentIndex = index;
    }
}
