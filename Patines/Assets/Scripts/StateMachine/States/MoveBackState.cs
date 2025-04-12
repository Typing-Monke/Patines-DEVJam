using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackState : StateBase {

    [Header("MoveBackState")]
    [Header("Animations")]
    public string animationName;
    [Header("Related States")]
    public string movementState;
    public string idleState;
    public string breakBackwardsState;
	
    public override void StateEnter() {
        
    }

    public override void StateExit() {
        
    }

    public override void StateInput() {

        if (controller.movement._velocity.y == 0)
        {
            controller.movement._velocity = -transform.forward * controller.movement._velocity.magnitude;
        }

        if (Input.GetButtonDown("ForceBack"))
        {
            controller.movement.MoveBack();
        }

        //if (Input.GetButtonDown("RightImpulse"))
        //{
        //    controller.movement.Move(transform.forward, controller.movement.force);
        //    controller.movement.Rotate(controller.movement.stepRotation);
        //    stateMachine.SetState(movementState);
        //}
        //else if (Input.GetButtonDown("LeftImpulse"))
        //{
        //    controller.movement.Move(transform.forward, controller.movement.force);
        //    controller.movement.Rotate(-controller.movement.stepRotation);
        //    stateMachine.SetState(movementState);
        //}

        if (Input.GetButton("ForceFront"))
        {
            stateMachine.SetState(breakBackwardsState);
        }
    }

    public override void StateLateStep() {
        
    }

    public override void StatePhysicsStep() {
        
    }

    public override void StateStep() {
        if (controller.movement._velocity.magnitude <= 0.1)
        {
            stateMachine.SetState(idleState);
        }
    }
}