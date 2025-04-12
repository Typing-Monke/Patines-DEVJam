using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakForwardState : StateBase {

    [Header("BreakState")]
    [Header("Animations")]
    public float velocityTriggerThreshold = .1f;
    public string animationName;
    [Header("Sound")]
    public AudioClip breakSound;
    [Header("Related States")]
    public string movementState;
    public string fallState;
    public string idleState;
    public string glideState;
	
    public override void StateEnter() {
        
        if(controller.movement._velocity.magnitude >= velocityTriggerThreshold)
        {
            controller.movement.SetFloatFactor(1);
            if (controller.movement.grounded)
            {
                SoundManager.instance.PlayClip(breakSound);
            }
            controller.animator.Play(animationName);
        }
    }

    public override void StateExit() {
        //AudioManager.instance.StopSound(breakSound);
        SoundManager.instance.StopClip();
    }

    public override void StateInput() {
        if (Input.GetButton("ForceBack"))
        {
            controller.movement.Break();
        }
        else
        {
            stateMachine.SetState(movementState);
        }
        // Si no esta pulsando boton de frenar y el player no está en el suelo
        if(!Input.GetButton("ForceBack") && !controller.movement.grounded)
        {
            // Quiere decir que quiere dejar de frenar pero está en el aire
            stateMachine.SetState(glideState);
        }
    }

    public override void StateLateStep() {
        
    }

    public override void StatePhysicsStep() {
        
    }

    public override void StateStep() {
        if (controller.movement._velocity.magnitude <= 0.1 && controller.movement.grounded) 
        {
            stateMachine.SetState(idleState);
        }

        if (!controller.movement._canGlide)
        {
            stateMachine.SetState(fallState);
        }
    }
}