using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    [SerializeField] private Player player;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cat"))
        {
            if(other.TryGetComponent(out Cat cat))
            {
                player.OnCatRescue.Invoke(other.gameObject);
                cat.FollowPlayer(this.transform.parent);
            }
        }
    }
}
