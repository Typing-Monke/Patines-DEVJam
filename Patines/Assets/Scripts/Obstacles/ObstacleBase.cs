using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Plantilla base para el comportamiento de los obstáculos
/// 
/// Author: Gonzalo Blanch Domínguez
/// </summary>

public abstract class ObstacleBase : MonoBehaviour
{
    [SerializeField]
    protected StateMachineController playerMachine;

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            playerMachine = collision.gameObject.GetComponentInChildren<StateMachineController>();
            ObstacleAction();
        }
    }

    /// <summary>
    /// Función que se va a ejecutar cuando haya interacción con el jugador
    /// </summary>
    public abstract void ObstacleAction();
}
