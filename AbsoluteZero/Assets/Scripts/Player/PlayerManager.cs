using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public static string playerTagName = "Player";

    public static void EnablePlayer(bool state)
    {
        GameObject[] allPlayerObjects = GameObject.FindGameObjectsWithTag(playerTagName);
        foreach (GameObject go in allPlayerObjects)
        {
            go.SetActive(state);
        }
    }
}
