using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FlyingObstacle : MonoBehaviour
{
    public UnityEvent<Vector3, float> OnHit;
    public float hitForce = 3;

    private StateMachineController playerMachine;
    private Transform player;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            playerMachine = other.gameObject.GetComponentInChildren<StateMachineController>();
            player = other.gameObject.GetComponent<Transform>();
            ObstacleAction();
        }
    }

    private void ObstacleAction() {
        playerMachine.SetState("Ragdoll");
        Vector3 direction = player.transform.position - transform.position;
        OnHit.Invoke(direction, hitForce);
    }
}
