using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameMenuManager : MonoBehaviour
{

    private void FixedUpdate()
    {
        var gamepad = Gamepad.current;
        if (gamepad != null)
        {
            if (gamepad.buttonSouth.wasPressedThisFrame)
            {
                SceneManager.LoadScene(2);
            }

            if (gamepad.buttonNorth.wasPressedThisFrame)
            {
                SceneManager.LoadScene(3);
            }
        }

        var keyboard = Keyboard.current;
        if (keyboard != null)
        {
            if (keyboard.aKey.wasPressedThisFrame)
            {
                SceneManager.LoadScene(2);
            }
            if (keyboard.kKey.wasPressedThisFrame)
            {
                SceneManager.LoadScene(3);
            }
            else if (keyboard.escapeKey.wasPressedThisFrame)
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}
