using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tsunami : MonoBehaviour
{
    public float moveSpeed;
    
    public bool isMoving = false;
    private void OnTriggerEnter(Collider other)
    { 
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent(out Player player))
            {
                Debug.Log("Tsunami hit player");
                player.OnPlayerDeath?.Invoke();
            }
        }
    }
    private void Update()
    {
        if (!isMoving) return;
           transform.Translate(Vector3.forward * (moveSpeed * Time.deltaTime));
        
    }
}
