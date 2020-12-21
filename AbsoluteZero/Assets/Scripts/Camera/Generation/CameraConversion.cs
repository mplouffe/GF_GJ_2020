using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;

[UpdateInGroup(typeof(GameObjectConversionGroup))]
public class CameraConversion : GameObjectConversionSystem
{
    protected override void OnUpdate()
    {
        Entities
            .ForEach((CameraProxyAuthoring cameraProxyAuthoring) =>
            {
                var entity = GetPrimaryEntity(cameraProxyAuthoring);
                var cameraComponent = new CameraComponent
                {
                    translation = new float3(cameraProxyAuthoring.MainCamera.transform.position),
                    rotation = cameraProxyAuthoring.MainCamera.transform.rotation
                };
                DstEntityManager.AddComponentData(entity, cameraComponent);
            });
    }
}
