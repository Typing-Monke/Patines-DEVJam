using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Script que gestiona el movimiento del jugador
/// Author: Carlos Carnero
/// </summary>

public class Carlos_PlayerController : MonoBehaviour {
    [Header("References")]
    public Rigidbody rb;
    public Animator animator;


    [Header("State")]
    public patueloState currentState = patueloState.Idle; // Controla el estado del jugador
    public enum patueloState {Idle, Walking, Ragdoll}  // Estados

    [Header("Movement")]
    public float speed;
    private Vector3 _velocity;

    private void FixedUpdate() {
        ApplyVelocity();
        //if (currentState != patueloState.Ragdoll) { // Si no está en estado ragdoll...
        //    MovementState(); // Aplica el estado según su movimiento
        //}      
        //AnimationControll();       
    }
    private void Update() {
        MoveInput();        
    }
    /// <summary>
    /// Méotodo que gestiona los inputs
    /// </summary>
    public void MoveInput() {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputZ = Input.GetAxisRaw("Vertical");

        Move(new Vector3(inputX, 0, inputZ));
    }
    /// <summary>
    /// Método que gestiona el movimiento del jugador según lo devuelto por MoveInput()
    /// </summary>
    /// <param name="direction"></param>
    public void Move(Vector3 direction) {
        _velocity = direction.normalized * speed;
    }
    /// <summary>
    /// Método que aplica velocidad al rigidbody según lo obtenido en el método de Move()
    /// </summary>
    public void ApplyVelocity() {
        rb.velocity = _velocity;
    }
    ///// <summary>
    ///// Controla los estados asociados al movimiento
    ///// </summary>
    //public void MovementState() {
    //    if (_velocity != Vector3.zero) {
    //        currentState = patueloState.Walking;
    //    } else {
    //        currentState = patueloState.Idle;
    //    }
    //}
    ///// <summary>
    ///// Controla las animaciones según el eestado del jugador
    ///// </summary>
    //public void AnimationControll() {
    //    switch (currentState) {
    //        case patueloState.Idle:
    //            animator.Play("Idle");
    //            break;
    //        case patueloState.Walking:
    //            animator.Play("Walk");
    //            break;
    //        case patueloState.Ragdoll:
    //            animator.enabled = false;
    //            break;
    //        default:
    //            break;
    //    }
    //}
}
