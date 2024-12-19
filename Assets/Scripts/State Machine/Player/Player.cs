using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : StateMachine
{
   
   [field: SerializeField] public InputReader InputReader { get; private set; }
   [field: SerializeField] public CharacterController Controller { get; private set; }
   [field: SerializeField] public Animator Animator { get; private set; }
   [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }

   [field: Header("Character Movement Configuration")]
   [field: SerializeField] public float RotationDamping { get; private set; }
   [field: SerializeField] public float FreeLookMovementSpeed { get; set; }
   
   [field: SerializeField] public float TargetingMovementSpeed { get; private set; }

   [field: Header("Camera")]
   
   [field: SerializeField] public GameObject CameraFocus { get; private set; }
   public Transform MainCamera { get; private set; }

   public Action OnTriggerEndPoint;
   
   public Action OnPlayerDeath;

   public Action<GameObject> OnCatRescue;
   private void Start()
   {
      if (Camera.main != null) MainCamera = Camera.main.transform;
      PlayerFreeLookState defaultState = new PlayerFreeLookState(this);
      SwitchState(defaultState);
   }
   private void OnControllerColliderHit(ControllerColliderHit hit)
   {
      if (hit.gameObject.CompareTag("EndPoint"))
      {
         Debug.Log("Player Reached End Point");
         OnTriggerEndPoint?.Invoke();
      }
      if (hit.gameObject.CompareTag("Sea"))
      {
         Debug.Log("Player Touch Sea Platform");
         OnPlayerDeath?.Invoke();
      }
      if (hit.gameObject.CompareTag("Cat"))
      {
         Debug.Log("Player Rescued Cat");
         OnCatRescue?.Invoke(hit.gameObject);
      }
   }
}
