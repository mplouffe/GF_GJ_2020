using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct MovementComponent : IComponentData
{
    public bool moveRequested;

    public bool isMoving;
    public bool isRotating;

    public float cooldown;
    public float stepDuration;
    public float turnDuration;

    public float moveDistance;

    public int startMovementTick;
    public int startTurningTick;

    public float3 moveDirection;
}
