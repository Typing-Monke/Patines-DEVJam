using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Obstáculo que rebota el player hacia detrás. El control del rebote
/// está en PlayerMovement y se comunica con este script a partir
/// del evento OnBounce.
/// 
/// Author: Gonzalo Blanch Domínguez
/// </summary>

public class BounceObstacleRagdoll : ObstacleBase
{
    public UnityEvent<Vector3, float> OnBounce;

    public float bounceForce = 5f;

    public override void ObstacleAction(){
        // Invocamos el evento
        Debug.Log("Hola");
        playerMachine.SetState("Move Back");
        OnBounce.Invoke(-transform.forward, bounceForce); 
    }
    
}