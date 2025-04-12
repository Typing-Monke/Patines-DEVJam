using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script que controla la máquina de estado.
/// 
/// Author: Gonzalo Blanch Domínguez
/// </summary>

public class StateMachineController : MonoBehaviour
{
    public StateBase[] states;

    [SerializeField]
    private StateBase _currentState;

    /// <summary>
    /// Inicializa la máquina de estados al estado que hay en la primera posición de la lista
    /// </summary>
    public void Initialize() {
        // Comprobamos si hay estados en la lista
        if(states != null && states.Length > 0) {
            // Ponemos como estado activo por defecto el primero de la lista
            SetState(states[0].stateName);
        }
    }

    /// <summary>
    /// Ejecuta las funciones de input y step. Equivale al Update()
    /// </summary>
    public void Step() {
        // Si hay estado activo
        if(_currentState != null) {
            // Ejecutamos input y step
            _currentState.StateInput();
            _currentState.StateStep();
        }
    }

    /// <summary>
    /// Ejecuta la función de físicas. Equivale al FixedUpdate()
    /// </summary>
    public void PhysicsStep() {
        // Si hay estado activo
        if (_currentState != null){
            // Ejecutamos el step de físicas
            _currentState.StatePhysicsStep();
        }
    }

    /// <summary>
    /// Ejecuta la función de lateStep. Equivale al LateUpdate()
    /// </summary>
    public void LateStep() {
        if(_currentState != null) {
            _currentState.StateLateStep();
        }
    }

    /// <summary>
    /// Pone como estado activo, el estado cuyo nombre coincide con el parámetro stateName.
    /// Si no lo encuentra, no hace nada.
    /// </summary>
    /// <param name="stateName"></param>
    public void SetState(string stateName) {
        if(GameManager.instance.pauseMenu._isInGame) {
            // Obtenemos una referencia al proximo estado
            StateBase nextState = GetStateWithName(stateName);
            // Si no existe cortamos la ejecución
            if(nextState == null) {
                return;
            }
            // Si hay esrtado activo, ejecutamos la función de salida.
            if(_currentState != null) {
                _currentState.StateExit();
            }
            // Actualizamos el estado
            _currentState = nextState;
            // Ejecutamos la función de entrada
            _currentState.StateEnter();
        }
        
    }

    /// <summary>
    /// Busca un estado en la lista con nombre stateName. Si no 
    /// lo encuentra retorna nulo.
    /// </summary>
    /// <param name="stateName"></param>
    /// <returns></returns>
    private StateBase GetStateWithName(string stateName) {
        // Recorremos la lista
        for(int i = 0; i < states.Length; i++) {
            // Si existe un estado con nombre = stateName
            if (states[i].stateName == stateName) {
                // Lo devolvemos
                return states[i];
            }
        }
        // Sino devolvemos null
        return null;
    }
}
