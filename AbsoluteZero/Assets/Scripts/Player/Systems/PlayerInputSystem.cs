using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

public enum MoveDirection
{
    None,
    Forward,
    Backward,
    Left,
    Right,
}

[UpdateBefore(typeof(MovementSystem))]
public class PlayerInputSystem : SystemBase
{

    private Gamepad m_gamepad;
    private bool m_gamepadActive = false;

    private Keyboard m_keyboard;
    private bool m_keyboardActive = false;

    protected override void OnUpdate()
    {
        if (m_gamepad != Gamepad.current)
        {
            // gamepad has changed, update it
            m_gamepad = Gamepad.current;
            m_gamepadActive = m_gamepad != null ? m_gamepad.enabled : false;
        }

        if (m_keyboard != Keyboard.current)
        {
            // keyboard has changed, update it
            m_keyboard = Keyboard.current;
            m_keyboardActive = m_keyboard != null ? m_keyboard.enabled : false;
        }

        var gamepadEnabled = m_gamepad.enabled;
        var keyboardEnabled = m_keyboard.enabled;

        MoveDirection direction = MoveDirection.None;

        if (m_gamepad.dpad.up.wasPressedThisFrame)
        {
            direction = MoveDirection.Forward;
        }
        else if (m_gamepad.dpad.down.wasPressedThisFrame)
        {
            direction = MoveDirection.Backward;
        }
        else if (m_gamepad.dpad.right.wasPressedThisFrame)
        {
            direction = MoveDirection.Right;
        }
        else if (m_gamepad.dpad.left.wasPressedThisFrame)
        {
            direction = MoveDirection.Left;
        }

        if (m_keyboard.upArrowKey.wasPressedThisFrame)
        {
            direction = MoveDirection.Forward;
        }
        else if (m_keyboard.downArrowKey.wasPressedThisFrame)
        {
            direction = MoveDirection.Backward;
        }
        else if (m_keyboard.rightArrowKey.wasPressedThisFrame)
        {
            direction = MoveDirection.Right;
        }
        else if (m_keyboard.leftArrowKey.wasPressedThisFrame)
        {
            direction = MoveDirection.Left;
        }

        Entities.ForEach((ref Entity entity, ref MovementComponent movementComponent) =>
        {
            if (!movementComponent.moveRequested)
            {
                switch(direction)
                {
                    case MoveDirection.None:
                        break;
                    case MoveDirection.Forward:
                        movementComponent.moveRequested = true;
                        movementComponent.moveDirection = new Unity.Mathematics.float3(0, 0, 1);
                        break;
                    case MoveDirection.Backward:
                        movementComponent.moveRequested = true;
                        movementComponent.moveDirection = new Unity.Mathematics.float3(0, 0, -1);
                        break;
                    case MoveDirection.Left:
                        movementComponent.moveRequested = true;
                        movementComponent.moveDirection = new Unity.Mathematics.float3(-1, 0, 0);
                        break;
                    case MoveDirection.Right:
                        movementComponent.moveRequested = true;
                        movementComponent.moveDirection = new Unity.Mathematics.float3(1, 0, 0);
                        break;
                }
            }
            
        }).Run();
    }
}
