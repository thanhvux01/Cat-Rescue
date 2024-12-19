using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeDetecter : MonoBehaviour
{
    public event Action<Vector3,Vector3> OnLedgeDetect;
    private void OnTriggerEnter(Collider other)
    {  
        Debug.Log("here");
        OnLedgeDetect?.Invoke(other.transform.forward,other.ClosestPointOnBounds(transform.position));
    }
}
