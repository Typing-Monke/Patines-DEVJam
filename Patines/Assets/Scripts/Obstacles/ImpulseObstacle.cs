using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ImpulseObstacle : ObstacleBase
{
    // Evento que ejecuta el rebote. Viene del PlayerMovement
    public UnityEvent<float> OnBounce;
    [Header("References")]
    [Header("Config")]
    public float bounceForce = 5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerMachine = other.gameObject.GetComponentInChildren<StateMachineController>();
            ObstacleAction();
        }
    }

    public override void ObstacleAction()
    {
        playerMachine.SetState("Jump");
        // Invocamos el evento de rebote
        OnBounce.Invoke(bounceForce);
    }

}