using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public PlayerMovement movement;
    public StateMachineController stateMachine;
    public Animator animator;
    public RagdollController_2 ragdoll;
    public ParticleSystemController particleController;
    public GameObject eInteractuable;
    [Header("Sounds")]
    public AudioClip quackSound;
    //public string musicName;

    private void Awake() {
        //AudioManager.instance.StopAllSound();
        //AudioManager.instance.PlaySound(musicName);
    }

    private void Start() {
        stateMachine.Initialize();
        // Cortamos todos los sonidos para que no se queden sonidos anteriores
        eInteractuable.SetActive(false);
    }

    private void Update() {
        stateMachine.Step();
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //AudioManager.instance.PlaySound("Quack");
            SoundManager.instance.PlayClip(quackSound);
        }

    }

    private void FixedUpdate() {
        stateMachine.PhysicsStep();
    }

    private void LateUpdate() {
        stateMachine.LateStep();
    }
}
