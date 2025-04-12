using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGlideState : StateBase {

    [Header("PlayerGlideState")]
    [Header("Animations")]
    public string glideAnimation;
    [Header("Sound")]
    public AudioClip glideSound;
    [Header("Config")]
    public float glideTime = 2f;
    [Header("Related States")]
    public string fallState;
    public string hoverRightState;
    public string hoverLeftState;
    public string breakState;

    private float _currentTimer;
    private bool _canGlide = true;
	
    public override void StateEnter() {
        controller.animator.Play(glideAnimation);
        SoundManager.instance.PlayClip(glideSound);
        _currentTimer = glideTime;
    }

    public override void StateExit() {
        //AudioManager.instance.StopSound(glideSound);
        SoundManager.instance.StopClip();
    }

    public override void StateInput() {

        if (controller.movement._canGlide && Input.GetButton("Float")){
            // Aquí está flotando
            controller.movement.GlideTimer();
            controller.movement.SetFloatFactor(controller.movement.jumpFloatFactor);
        }else{
            // Aquí está cayendo
            controller.movement.SetFloatFactor(1);
        }

        if (!Input.GetButton("Float") || !controller.movement._canGlide)
        {
            stateMachine.SetState(fallState);
        }

        if (Input.GetAxisRaw("Horizontal") > 0) {
            stateMachine.SetState(hoverRightState);
        }
        else if(Input.GetAxisRaw("Horizontal") < 0)
        {
            stateMachine.SetState(hoverLeftState);
        }

        if (Input.GetButton("ForceBack"))
        {
            stateMachine.SetState(breakState);
        }
    }

    public override void StateLateStep() {
        if (controller.movement.grounded)
        {
            stateMachine.SetState("Movement");
        }

    }

    public override void StatePhysicsStep() {
        
    }

    public override void StateStep() {
        //_currentTimer -= Time.deltaTime;
        //if(_currentTimer <= 0)
        //{
        //    _canGlide = false;
        //}

        //if (controller.movement.grounded)
        //{
        //    _canGlide = true;
        //}
    }
}