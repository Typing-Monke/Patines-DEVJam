using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : StateBase {

    [Header("JumpState")]
    [Header("Animations")]
    public string jumpAnimation;
    [Header("Sounds")] 
    public AudioClip jumpSound;
    [Header("Related States")]
    //public string groundMovementState;
    public string fallState;
    public string glideState;
	
    public override void StateEnter() {
        controller.particleController.TriggerParticleSystem(1);
        controller.animator.Play(jumpAnimation);
        controller.movement.SetFloatFactor(1);
        //float forwardMagnitud = Vector3.Dot(controller.movement._velocity, controller.transform.forward);

        //Vector3 forwardVelocity = controller.transform.forward * forwardMagnitud; ;
        //Vector3 velocidadFinal = forwardVelocity;
        //velocidadFinal.y = controller.movement._velocity.y;
        //Debug.Log(velocidadFinal);
        //controller.movement._velocity = velocidadFinal;
        SoundManager.instance.PlayClip(jumpSound);
        controller.movement.Jump();
    }

    public override void StateExit() {
        
    }

    public override void StateInput() {
        if (controller.movement._canGlide && Input.GetButtonDown("Float"))
        {
            stateMachine.SetState(glideState);
        }
    }

    public override void StateLateStep() {
        //if (controller.movement.grounded)
        //{
        //    controller.movement._canGlide = true;
        //    stateMachine.SetState(groundMovementState);
        //}

        if(controller.movement._rb.velocity.y <= 0)
        {
            stateMachine.SetState(fallState);
        }
    }

    public override void StatePhysicsStep() {
        
    }

    public override void StateStep() {
        
    }
}