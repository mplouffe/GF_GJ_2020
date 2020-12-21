using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct CameraFollowComponent : IComponentData
{
    public float3 CameraFollowOffset;
    public float CameraSmoothSpeed;
}
