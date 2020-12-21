using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct CameraComponent : IComponentData
{
    public float3 translation;
    public quaternion rotation;
}
