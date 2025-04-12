using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RotatingObstacle : MonoBehaviour
{
    public UnityEvent<Vector3, float> OnHit;
    public RotatingObstacleMovement movement;

    private StateMachineController playerMachine;
    private PlayerMovementState playerMovement;
    
    public float bounceForce = 5f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerMachine = collision.gameObject.GetComponentInChildren<StateMachineController>();
            playerMovement = collision.gameObject.GetComponentInChildren<PlayerMovementState>();
            ObstacleAction();
        }
    }

    public void ObstacleAction(){
        playerMachine.SetState("Movement");
        playerMovement.fixForwardVelocity = false;
        //TODO: Comprobar dirección de giro y aplicar fuerzas en función de ello. Lo de abajo solo funciona
        // si va hacia la izquierda
        if(movement.rotatingLeft){
            OnHit.Invoke(new Vector3 (-1, 0, 0), bounceForce);
        }

        if(!movement.rotatingLeft)
        {
            OnHit.Invoke(new Vector3(1, 0, 0), bounceForce);
        }
    }
    
}