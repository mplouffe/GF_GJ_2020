using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameOverMenuManager : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(LevelChanger());
    }

    private void FixedUpdate()
    {
        var keyboard = Keyboard.current;
        if (keyboard != null)
        {
            if (keyboard.escapeKey.wasPressedThisFrame)
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    IEnumerator LevelChanger()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(0);
    }
}
