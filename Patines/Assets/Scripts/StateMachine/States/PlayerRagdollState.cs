using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerRagdollState : StateBase {

    [Header("PlayerRagdollState")]
    [Header("Animations")]
    public string animationName;
    [Header("Related States")]
    public string idleState;

    private bool _ragdollEnabled = false;
	
    public override void StateEnter() {
        _ragdollEnabled = true;
        controller.ragdoll.currentState = RagdollController_2.PatueloState.Ragdoll;
        controller.particleController.TriggerParticleSystem(0);

        
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
        if (controller.ragdoll.currentState == RagdollController_2.PatueloState.Idle || !_ragdollEnabled)
        {
            // Sale del estado
            controller.stateMachine.SetState(idleState);
        }
    }
}