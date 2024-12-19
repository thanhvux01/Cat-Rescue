using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class FixCamera : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook _freeLookCamera;

    private void Update()
    {
        _freeLookCamera.m_XAxis.Value = 0;
    }
}
