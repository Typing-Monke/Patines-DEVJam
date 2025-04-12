using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Obstáculo que hace que el jugador rebote en una determinada
/// dirección calculada a través de bounceDirectionControl
/// 
/// Author: Gonzalo Blanch Domínguez
/// </summary>

public class ControlledBounceObstacle : ObstacleBase
{
    // Evento que ejecuta el rebote. Viene del PlayerMovement
    public UnityEvent<Vector3> OnBounce;
    [Header("References")]
    // Control de dirección del rebote
    public Transform bounceDirectionControl;
    [Header("Config")]
    public float bounceForce = 5f;
    
    public override void ObstacleAction(){
        // Calculamos la dirección hacia donde queremos que rebote
        Vector3 direction = bounceDirectionControl.position - transform.position;
        // Calculamos la fuerza a partir de la dirección
        Vector3 force = direction.normalized * bounceForce;

        playerMachine.SetState("Move Back");
        // Invocamos el evento de rebote
        OnBounce.Invoke(force);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if(bounceDirectionControl != null)
        {
            Gizmos.DrawRay(transform.position, bounceDirectionControl.position - transform.position);
        }
    }
}