using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[UpdateInGroup(typeof(GameObjectConversionGroup))]
public class LostBoxConversion : GameObjectConversionSystem
{
    protected override void OnUpdate()
    {
        Entities
            .ForEach((LostBoxAuthoring lostBox) =>
            {
                var entity = GetPrimaryEntity(lostBox);
                DstEntityManager.AddComponent(entity, typeof(LostBoxComponent));
            });
    }
}
