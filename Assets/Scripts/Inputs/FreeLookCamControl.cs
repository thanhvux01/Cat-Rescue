using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;


//This script is using for fix free look bug when disable action but camera still working
public class FreeLookCamControl : MonoBehaviour
{
    [SerializeField] CinemachineInputProvider cinemachineInputProvider;

    private void Update()
    {
        // if (GameMa.CurrentGameState != GameState.Gameplay)
        // {
        //     cinemachineInputProvider.enabled = false;
        // }
        // else
        // {
        //     cinemachineInputProvider.enabled = true;
        // }
    }
}
