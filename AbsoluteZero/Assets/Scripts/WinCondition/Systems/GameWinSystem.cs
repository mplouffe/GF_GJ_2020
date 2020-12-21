using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public class GameWinSystem : SystemBase
{
    protected override unsafe void OnUpdate()
    {
        int totalBoxesReturned = 0;
        Entities
            .ForEach((FoundBox boxFound) =>
            {
                totalBoxesReturned++;
            })
        .Run();

        if (totalBoxesReturned == 1 && !ZeroGameManager.Instance.IsGameOver)
        {
            ZeroGameManager.Instance.IsGameOver = true;
            ZeroGameManager.Instance?.GameOver();
        }
    }
}
