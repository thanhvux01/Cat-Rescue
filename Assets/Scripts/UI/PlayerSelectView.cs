using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectView : View
{
    [field:SerializeField] public Button PreviousButton { get; private set; }
    
    [field:SerializeField] public Button NextButton { get; private set; }
    
    [field:SerializeField] public Button ConfirmButton { get; private set; }
}
