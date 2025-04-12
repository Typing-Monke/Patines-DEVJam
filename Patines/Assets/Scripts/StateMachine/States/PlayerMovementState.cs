using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PlayerMovementState : StateBase {

    [Header("PlayerMovementState")]
    [Header("Animations")]
    public float movementAnimationThreshold = .2f;
    public string animationName;
    [Header("Config")]
    public float ragdollThreshold = .5f;
    [Header("Sound")]
    public AudioClip movementFXSound;

    [Header("Related States")]
    public string jumpState;
    public string breakState;
    public string idleState;

    public bool fixForwardVelocity = true;
	
    public override void StateEnter() {
        controller.animator.Play(animationName);
        SoundManager.instance.PlayClip(movementFXSound);

        if (controller.movement._velocity.magnitude > movementAnimationThreshold)
        {
            controller.animator.Play(animationName);
        }

        controller.movement._canDrag = true;
    }

    public override void StateExit() {
        //AudioManager.instance.StopSound(movementFXSound);
        SoundManager.instance.StopClip();
    }

    public override void StateInput() {

        

        //controller.movement.CheckGroundInputs();

        if (fixForwardVelocity && controller.movement._velocity.y == 0)
        {
            controller.movement._velocity = transform.forward * controller.movement._velocity.magnitude;
        }

        if (Input.GetButtonDown("RightImpulse"))
        {
            fixForwardVelocity = true;
            controller.movement.Move(transform.forward, controller.movement.force);
            controller.movement.Rotate(controller.movement.stepRotation);
        }
        else if (Input.GetButtonDown("LeftImpulse"))
        {
            fixForwardVelocity = true;
            controller.movement.Move(transform.forward, controller.movement.force);
            controller.movement.Rotate(-controller.movement.stepRotation);
        }
        if (Input.GetButton("ForceBack"))
        {
            stateMachine.SetState(breakState);
        }

        if (Input.GetButtonDown("Jump") || !controller.movement.grounded)
        {
            stateMachine.SetState(jumpState);
        }
    }

    public override void StateLateStep() {
        
    }

    public override void StatePhysicsStep() {
        
    }

    public override void StateStep() {
        //if(controller.movement._velocity.magnitude < 0.2)
        //{
        //    controller.animator.Play("Idle5");
        //}

        //if (controller.movement._velocity.magnitude > movementAnimationThreshold)
        //{
        //    controller.animator.Play(animationName);
        //}
        //else
        //{
        //    controller.animator.Play("Idle5");
        //}

        //if (controller.movement.isAgainstWall) {
        //    controller.movement._velocity.x = 0;
        //    controller.movement._velocity.y = 0;
        //    Debug.Log("RAGDOLL");
        //    stateMachine.SetState("Ragdoll");
        //}

        if (controller.movement._velocity.magnitude <= 0.01)
        {
            stateMachine.SetState(idleState);
        }
    }
}