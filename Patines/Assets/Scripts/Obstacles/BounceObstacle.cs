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

public class BounceObstacle : MonoBehaviour
{
    public UnityEvent<Vector3, float> OnBounce;

    public float bounceForce = 5f;
    public float ragdollThreshold = .3f;

    private StateMachineController playerMachine;
    private PlayerMovement movement;

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Player")) {
            playerMachine = collision.gameObject.GetComponentInChildren<StateMachineController>();
            movement = collision.gameObject.GetComponent<PlayerMovement>();
            if(movement._velocity.magnitude >= ragdollThreshold) {
                ObstacleAction();
            }
            
        }
        
    }

    public void ObstacleAction(){
        // Invocamos el evento
        Debug.Log("Hola");
        playerMachine.SetState("Ragdoll");
        OnBounce.Invoke(-playerMachine.gameObject.transform.forward, bounceForce);
    }
    
}