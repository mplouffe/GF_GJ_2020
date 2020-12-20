using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[UpdateInGroup(typeof(GameObjectConversionGroup))]
public class PlayerConversion : GameObjectConversionSystem
{
    protected override void OnUpdate()
    {
        Entities
            .ForEach((PlayerAuthoring playerAuthoring) =>
            {
                var entity = GetPrimaryEntity(playerAuthoring);

                DstEntityManager.AddComponent(entity, typeof(MovementComponent));
            });
    }

}
