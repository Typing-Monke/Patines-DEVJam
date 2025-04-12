using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// Script que gestiona el Ragdoll del jugador
/// Author: Carlos Carnero
/// </summary>

public class PlayerRagdollController : MonoBehaviour {
    [Header("References")]
    public Rigidbody playerRb;  
    public Animator playerAnimator; 
    public BoxCollider playerCollider;
    public Carlos_PlayerController playerController;
    //public PlayerMovement playerMovement;

    public Collider[] childrenColliders; // Array de los colliders asociados a los huesos del jugador
    public Rigidbody[] childrenRigidbodys; // Array de los rigidbody asociados a los huesos del jugador
    //public Transform[] childrenTransform; // Array de los rigidbody asociados a los huesos del jugador

    public List<Vector3> childrenPosition; // Lista que guarda la posición de los huesos
    public List<Quaternion> childrenRotation; // Lista que guarda la rotación de los huesos

    private bool isActive = false; // Booleana que gestiona si el Ragdoll está activo o no

    private void Awake() {
        childrenColliders = GetComponentsInChildren<Collider>(); // Busca los colliders de los huesos del jugador para aplicarlos al array
        childrenRigidbodys = GetComponentsInChildren<Rigidbody>(); // Busca los rigidbody de los huesos del jugador para aplicarlos al array     
    }
    private void Start() {
        //  DESACTIVA EL RAGDOLL AL INICIO DEL JUEGO
        RaggdolEnabled(isActive = false);
    }
    public void Update() {
        // BOTÓN PARA ACTIVAR Y DESACTIVAR EL RAGDOLL
        if (Input.GetButtonDown("Jump")) {
            RaggdolEnabled(isActive = !isActive);
        }
    }
    /// <summary>
    /// Gestiona todo lo que hay que activar y desactivar para que el ragdoll funcione dependiendo de la booleana que le mandemos
    /// </summary>
    /// <param name="active"></param>
    public void RaggdolEnabled(bool active) {
        // GUARDAMOS TRANSFORM DE LOS HUESOS CUANDO SE ACTIVA EL RAGDOLL
        if (active) {
            foreach (var rigidbody in childrenRigidbodys) // Guarda la posición de los huesos con collider
                childrenPosition.Add(rigidbody.transform.position);
            foreach (var rigidbody in childrenRigidbodys) // Guarda la rotación de los huesos con collider
                childrenRotation.Add(rigidbody.transform.rotation);
        } else {
            if (childrenPosition.Any() && childrenRotation.Any()) { // Para evitar conflictos con el inicio del juego
                for (int i = 0; i < childrenRigidbodys.Length; i++) { // Aplicamos el transforma todos los huesos
                    childrenRigidbodys[i].transform.position = childrenPosition[i]; // Recueramos la posición
                    childrenRigidbodys[i].transform.rotation = childrenRotation[i]; // Recuperamos la rotación
                }
            }            
            // Limpiamos la lista
            childrenPosition.Clear();
            childrenRotation.Clear();
        }

        foreach (var collider in childrenColliders) // Activa todos los collider de los huesos
            collider.enabled = active;
        foreach (var rigidbody in childrenRigidbodys) { // Activa todos los rigidbody de los huesos
            rigidbody.detectCollisions = active; 
            rigidbody.isKinematic = !active;
        }     
        // DESACTIVAMOS LOS PARÁMETROS QUE PUEDAN HACER CONFLICTO CON EL RAGDOLL
        playerAnimator.enabled = !active;
        playerCollider.enabled = !active;
        playerRb.detectCollisions = !active;
        playerRb.isKinematic = active;
        playerController.enabled = !active;
        //playerMovement.enabled = !active;
    }
    /// <summary>
    /// Activa el ragdoll cuando el player se golpea
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Obstacle")) {
            RaggdolEnabled(isActive = true);

        }
    }
}
