using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallState : StateBase {

    [Header("FallState")]
    [Header("Animations")]
    public string animationName;
    [Header("Related States")]
    public string movementStateName;
	
    public override void StateEnter() {
        controller.movement.SetFloatFactor(1);
        controller.animator.Play(animationName);
    }

    public override void StateExit() {
    }

    public override void StateInput() {
        
    }

    public override void StateLateStep() {
        
    }

    public override void StatePhysicsStep() {
        
    }

    public override void StateStep() {
        if (controller.movement.grounded)
        {
            stateMachine.SetState(movementStateName);
        }

        if(controller.movement._canGlide && Input.GetButton("Float"))
        {
            controller.stateMachine.SetState("Glide");
        }
    }
}