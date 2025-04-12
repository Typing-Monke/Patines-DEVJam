using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBackwardState : StateBase {

    [Header("BreakBackwardState")]
    [Header("Animations")]
    public string animationName;
    [Header("Related States")]
    public string movementState;
    public string idleState;
    public string jumpState;
	
    public override void StateEnter() {
        
    }

    public override void StateExit() {
        
    }

    public override void StateInput() {
        if (Input.GetButton("ForceFront"))
        {
            controller.movement.Break();
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