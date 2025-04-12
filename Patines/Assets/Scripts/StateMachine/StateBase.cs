using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clase abstracta con la plantilla de un estado
/// básico para la máquina de estados.
/// </summary>

public abstract class StateBase : MonoBehaviour
{
    [Header("References")]
    public StateMachineController stateMachine;
    public PlayerController controller;
    [Header("State Name")]
    public string stateName;

    public abstract void StateEnter();
    public abstract void StateExit();
    public abstract void StateInput();
    public abstract void StateLateStep();
    public abstract void StatePhysicsStep();
    public abstract void StateStep();
}
