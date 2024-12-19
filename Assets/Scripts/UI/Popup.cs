using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Popup : MonoBehaviour
{
    [field:SerializeField] public Button CloseButton { get; private set; }
    
    [field:SerializeField] public Button ConfirmButton { get; private set; }
    
    [field:SerializeField] public Button CancelButton { get; private set; }

    private void Awake()
    {
        CloseButton.onClick.AddListener(()=> gameObject.SetActive(false));
    }
}
