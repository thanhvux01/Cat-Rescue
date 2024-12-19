using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class Cat : MonoBehaviour
{
    [SerializeField] private float keepingDistance = 1f;
    private Transform _player;
    private bool _isFollowing = false;
    public float followSpeed = 2f;  
   
    public void FollowPlayer(Transform player)
    {
        _player = player;
        _isFollowing = true;
    }
    
    private void Update()
    {
        if (_isFollowing && _player != null)
        {
            float distance = Vector3.Distance(_player.transform.position, transform.position);
            if (distance < keepingDistance) { return; }
            Vector3 targetPosition = _player.position;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, followSpeed * Time.deltaTime);
            
            Vector3 direction = targetPosition - transform.position;
           
            if (direction.magnitude > 0.1f ) 
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, followSpeed * Time.deltaTime);
            }
        }
    }
}