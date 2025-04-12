using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHoverRightState : StateBase {

    [Header("PlayerHoverRightState")]
    [Header("Animations")]
    public string animationName;
    [Header("Sound")]
    public AudioClip glideSound;
    [Header("Related States")]
    public string fallState;
    public string glideState;
    public string hoverLeftState;
	
    public override void StateEnter() {
        SoundManager.instance.PlayClip(glideSound);
        controller.animator.Play(animationName);
    }

    public override void StateExit() {
        //AudioManager.instance.StopSound(glideSound);
        SoundManager.instance.StopClip();
    }

    public override void StateInput() {

        if (Input.GetButtonUp("Float") || !controller.movement._canGlide)
        {
            //controller.movement._canGlide = true;
            stateMachine.SetState(fallState);
        }

        controller.movement.GlideTimer();

        float xInput = Input.GetAxisRaw("Horizontal");
        if(xInput == 0)
        {
            stateMachine.SetState(glideState);
        }else if(xInput < 0)
        {
            stateMachine.SetState(hoverLeftState);
        }
        if (controller.movement._canGlide)
        {
            controller.movement.Fly(xInput);
        }
    }

    public override void StateLateStep() {
        if (controller.movement.grounded)
        {
            stateMachine.SetState(fallState);
        }
    }

    public override void StatePhysicsStep() {
        
    }

    public override void StateStep() {
        
    }
}