using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenuMenuManager : MonoBehaviour
{

    private void FixedUpdate()
    {
        var gamepad = Gamepad.current;
        if (gamepad != null)
        {
            if (gamepad.buttonSouth.wasPressedThisFrame)
            {
                SceneManager.LoadScene(1);
            }
        }

        var keyboard = Keyboard.current;
        if (keyboard != null)
        {
            if (keyboard.aKey.wasPressedThisFrame)
            {
                SceneManager.LoadScene(1);
            }
            else if (keyboard.escapeKey.wasPressedThisFrame)
            {
                Application.Quit();
            }
        }
    }
}
