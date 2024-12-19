using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFreeLookState : PlayerBaseState
{
    private readonly int FREELOOKBLENDTREE = Animator.StringToHash("FreeLookBlendTree");
    private readonly int FREELOOKPARAM = Animator.StringToHash("FreeLookSpeed");

    private bool shouldFade;

    private const float animatorDampTime = 0.1f;

    public PlayerFreeLookState(Player stateMachine, bool shouldFade = true) : base(stateMachine)
    {
        this.shouldFade = shouldFade;
    }

    public override void Enter()
    {
        if (shouldFade)
        {
            stateMachine.Animator.CrossFadeInFixedTime(FREELOOKBLENDTREE, 0.1f);
        }
        else
        {
            stateMachine.Animator.Play(FREELOOKBLENDTREE);
        }

        stateMachine.Animator.SetFloat(FREELOOKPARAM, 0);

    }

    public override void Exit()
    {

    }

    public override void Tick(float deltaTime)
    {
     
        Vector3 direction = CalculateMovement();
        Move(direction * stateMachine.FreeLookMovementSpeed, deltaTime);

        if (direction == Vector3.zero)
        {
            stateMachine.Animator.SetFloat(FREELOOKPARAM, 0, animatorDampTime, deltaTime);
            return;
        }
        stateMachine.Animator.SetFloat(FREELOOKPARAM, 1, animatorDampTime, deltaTime);
        FaceToDirection(direction, deltaTime);

    }

    private void FaceToDirection(Vector3 direction, float deltaTime)
    {
        Quaternion destination = Quaternion.LookRotation(direction);
        stateMachine.transform.rotation = Quaternion.Lerp(stateMachine.transform.rotation, destination, deltaTime * stateMachine.RotationDamping);
    }


    private Vector3 CalculateMovement()
    {
        Vector2 input = stateMachine.InputReader.MovementValue; // Vector2 từ InputSystem (có thể là WASD hoặc Analog Stick)

        // Lấy hướng của camera (không tính đến độ nghiêng trục Y)
        Vector3 forward = stateMachine.MainCamera.forward;
        forward.y = 0f;
        forward.Normalize();

        Vector3 right = stateMachine.MainCamera.right;
        right.y = 0f;
        right.Normalize();

        // Tính toán hướng di chuyển dựa trên input
        Vector3 direction = forward * input.y + right * input.x;
        direction.Normalize();

        return direction;
    }
}
