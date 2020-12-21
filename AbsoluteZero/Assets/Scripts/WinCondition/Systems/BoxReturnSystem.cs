using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class BoxReturnSystem : SystemBase
{
    protected override unsafe void OnUpdate()
    {
        Entities
            .WithStructuralChanges()
            .ForEach((Entity entity, LostBoxComponent lostbox, Translation translation) =>
            {
                if (translation.Value.z > -3 && translation.Value.x > -1)
                {
                    EntityManager.AddComponent(entity, typeof(FoundBox));
                    EntityManager.RemoveComponent(entity, typeof(LostBoxComponent));
                }
            })
            .Run();

        Entities
            .WithStructuralChanges()
            .ForEach((Entity entity, FoundBox lostbox, Translation translation) =>
            {
                if (translation.Value.z < -3 || translation.Value.x < -1)
                {
                    EntityManager.AddComponent(entity, typeof(LostBoxComponent));
                    EntityManager.RemoveComponent(entity, typeof(FoundBox));
                }
            })
            .Run();
    }
}
