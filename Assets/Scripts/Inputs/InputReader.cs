using System;
using System.Collections;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "Inputs/InputReader", order = 0)]
public class InputReader : ScriptableObject, Controls.IPlayerActions, Controls.IUIActions
{
    public Vector2 MovementValue { get; private set; }
    public event Action JumpEvent;
    public event Action RunEvent;
    public event Action DodgeEvent;
    public event Action TargetEvent;
    public event Action InteractEvent;
    public event Action ExitEvent;
    public event Action ConfirmEvent;
    public event Action OpenInventoryEvent;
    public event Action CloseEvent;

    private Controls controls;
    
    public bool IsAttacking;
    
    public bool IsBlocking;
    private void OnEnable()
    {
        controls = new Controls();
        controls.Player.SetCallbacks(this);
        controls.UI.SetCallbacks(this);
        controls.Player.Enable();
    }
    private void OnDisable()
    {
        controls.Player.Disable();
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }
        JumpEvent?.Invoke();

    }
    public void OnDodge(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }
        DodgeEvent?.Invoke();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MovementValue = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {

    }

    public void OnTarget(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }
        TargetEvent?.Invoke();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed) { IsAttacking = true; }
        else if (context.canceled) { IsAttacking = false; }
    }

    public void OnBlock(InputAction.CallbackContext context)
    {
        if (context.performed) { IsBlocking = true; }
        else if (context.canceled) { IsBlocking = false; }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }
        InteractEvent?.Invoke();
    }

    public void OnExit(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }
        ExitEvent?.Invoke();
    }

    public void OnConfirm(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }
        Debug.Log("Here");
        ConfirmEvent?.Invoke();
    }

    public void EnableGameplayInput()
    {
        controls.Player.Enable();
        controls.UI.Disable();
    }
    public void EnableMenuInput()
    {
        controls.Player.Disable();
        controls.UI.Enable();
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }
        OpenInventoryEvent?.Invoke();
    }

    public void OnClose(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }
        Debug.Log("Here");
        CloseEvent?.Invoke();
    }
}
