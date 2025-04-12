using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class IdleState : StateBase {

    [Header("IdleState")]
    [Header("Animations")]
    public string animationName;
    public string[] longTimeIdles;
    [Header("Config")]
    public float longTimeIdleTimer;
    [Range(0f, 1f)]
    public float longTimeIdleProbability;
    [Header("Related States")]
    public string movementState;
    public string moveBackState;
    public string jumpState;

    private float _currentTimer;
	
    public override void StateEnter() {
        _currentTimer = longTimeIdleTimer;
        controller.animator.Play(animationName);       
    }

    public override void StateExit() {
        
    }

    public override void StateInput() {
        // Vuelvo a llamar al movimiento para que cuando se pulse para
        // cambiar de estado mueva y cambie

        if (Input.GetButtonDown("RightImpulse"))
        {
            controller.movement.Move(transform.forward, controller.movement.force);
            controller.movement.Rotate(controller.movement.stepRotation);
            stateMachine.SetState(movementState);
        }
        else if (Input.GetButtonDown("LeftImpulse"))
        {
            controller.movement.Move(transform.forward, controller.movement.force);
            controller.movement.Rotate(-controller.movement.stepRotation);
            stateMachine.SetState(movementState);
        }

        if (Input.GetButtonDown("Jump") || !controller.movement.grounded)
        {
            stateMachine.SetState(jumpState);
        }

        if (Input.GetButtonDown("ForceBack"))
        {
            controller.movement.MoveBack();
            stateMachine.SetState(moveBackState);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            controller.animator.Play("Quack");
        }
    }

    public override void StateLateStep() {
        
    }

    public override void StatePhysicsStep() {
        
    }

    public override void StateStep() {
        _currentTimer -= Time.deltaTime;
        if(_currentTimer <= 0)
        {
            _currentTimer = longTimeIdleTimer;
            int probability = Random.Range(0, 2);
            if(probability < longTimeIdleProbability)
            {
                int index = Random.Range(0, longTimeIdles.Length);
                controller.animator.Play(longTimeIdles[index]);
            }
        }
    }
}