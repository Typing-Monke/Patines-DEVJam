using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class RagdollController_2 : MonoBehaviour
{
    /// <summary>
    /// Clase que gestiona la posición y rotación de los huesos para recuperar su transform al desacativar el ragdoll
    /// </summary>
    private class BoneTransform {
        public Vector3 Position { get; set;  }

        public Quaternion Rotation { get; set; }
    }

    [Header("References")]
    public Animator animator;
    public Transform _hipsBone; // Hueso del Armature
    public string standUpAnimationName;
    public bool ragdollEnabled; // Gestiona si el ragdoll está activo o no

    // MÁQUINA DE ESTADO BÁSICA (SE PUEDE CAMBIAR POR EL NUEVO SCRIPT)
    public enum PatueloState { Idle, Ragdoll, StandingUp, ResettingBones } 
    public PatueloState currentState = PatueloState.Idle;

    private Rigidbody[] _ragdollRigidbodies;    // Array de los ragdolls que vamos a gestionar
    // REFERENCES
    private PlayerMovement _playerMovement;
    private Rigidbody playerRb;

    public float ragdollTime = 5; // Tiempo que pasará hasta que el pato desactive el ragdoll
    private float _ragdollTimeBackup; // Tiempo que pasará hasta que el pato desactive el ragdoll
    [SerializeField] private float _timeToResetBones; // Tiempo que pasará hasta que los huesos se reseteen

    private BoneTransform[] _standUpBoneTransforms; // Guarda el transform del primer frame de los huesos de la animación de levantarse
    private BoneTransform[] _ragdollBoneTransforms; // Guarda el transform de los huesos en modo ragdoll antes de seactivarlo
    private Transform[] _bones; // Guardamos el transform de todos los huesos
    private float _elapsedResetBonesTime;

    private Quaternion _hipsRotation;

    private Vector3 _hipsVelocity;
    
 
    private void Awake() {
        // INICIALIZAMOS LAS REFERENCIAS
        _ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
       // _characterController = GetComponent<Carlos_PlayerController>();
        playerRb = GetComponent<Rigidbody>();
        _playerMovement = GetComponent<PlayerMovement>();
        _ragdollTimeBackup = ragdollTime;

        // INICIALIZAMOS LOS HUESOS
        _bones = _hipsBone.GetComponentsInChildren<Transform>(); // Obtenemos los transforms de todos los huesos
        _hipsRotation = _hipsBone.rotation;
        _standUpBoneTransforms = new BoneTransform[_bones.Length]; // Aplicamos al array el número de huesos que vamos a usar
        _ragdollBoneTransforms = new BoneTransform[_bones.Length]; // Aplicamos al array el número de huesos que vamos a usar

        for (int boneIndex = 0; boneIndex < _bones.Length; boneIndex++) {
            _standUpBoneTransforms[boneIndex] = new BoneTransform(); // Asignamos la clase BoneTransform a cada hueso
            _ragdollBoneTransforms[boneIndex] = new BoneTransform(); // Asignamos la clase BoneTransform a cada hueso
        }
        Debug.Log("stand up population");
        PopulateAnimationStartBoneTransforms(standUpAnimationName, _standUpBoneTransforms);   
        

        RagdollEnabled(false); // Nos aseguramos de que el ragdoll esté desactivado al inicio
    }
    void Update()
    {
        // ACTIVAMOS Y DESACTIVAMOS RAGDOLL CON EL ESPACIO
        //if (Input.GetKeyDown(KeyCode.Space)) {
        //    if (currentState != PatueloState.Ragdoll) { // Si no está en estado ragdoll, activamos el ragdoll
        //        currentState = PatueloState.Ragdoll;
        //        ragdollTime = _ragdollTimeBackup; // Restablecemos el tiempo para que se levante
        //    } else {
        //        ragdollTime = 0; // Si está en modo ragdoll, establecemos que se levante ya
        //    }
        //}

        if (currentState == PatueloState.Ragdoll) {
            _hipsBone.GetComponent<Rigidbody>().velocity = _hipsVelocity;
        }
        // GESTIONAMOS LA MÁQUINA DE ESTADO BÁSICA (QUE HACE EN CADA ESTADO)
        switch (currentState) {
            case PatueloState.Idle:                
                IdleBehaviour();
                break;
            case PatueloState.Ragdoll:
                RagdollEnabled(ragdollEnabled = true); // Activa el ragdoll
                RagdollBehaviour();                
                break;
            case PatueloState.StandingUp:
                StandingUpBehaviour();                
                break;
            case PatueloState.ResettingBones:                
                ResettingBonesBehaviour();               
                break;
        }
    }
    /// <summary>
    /// Gestiona el cambio al activar y desactivar el ragdoll
    /// </summary>
    /// <param name="active"></param>
    private void RagdollEnabled(bool active) {        
        foreach (var rigidbody in _ragdollRigidbodies) { // Activa todos los rigidbody de los huesos
            rigidbody.detectCollisions = active;
            rigidbody.isKinematic = !active;
        }
        // DESACTIVAMOS LOS PARÁMETROS QUE PUEDAN HACER CONFLICTO CON EL RAGDOLL
        animator.enabled = !active;
        playerRb.detectCollisions = !active;
        playerRb.isKinematic = active;
       // _characterController.enabled = !active;
    }
    /// <summary>
    /// Gestiona lo que hace cuando está en estado Idle
    /// </summary>
    private void IdleBehaviour() {       
        //AlignPositionToHips();
    }
    /// <summary>
    /// Gestiona lo que hace cuando está en estado ragdoll
    /// </summary>
    private void RagdollBehaviour() {
        ragdollTime -= Time.deltaTime; // Cuenta atrás
        // Cuando el tiempo llegue a 0...
        if (ragdollTime <= 0) {
            AlignPositionToHips(); // Alineamos el armature con el Pato
            Debug.Log("--- RAGDOLL BONES: -----");
            PopulateBoneTransforms(_ragdollBoneTransforms); // Guardamos el transform actual de los huesos 
            _elapsedResetBonesTime = 0;
            currentState = PatueloState.ResettingBones;
        }
    }
    private void StandingUpBehaviour() {
        // ESPERAMOS A QUE LA ANIMACIÓN TERMINE
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(standUpAnimationName) == false) {
            currentState = PatueloState.Idle; // Volvemos al estado de Idle
        }
    }
    private void ResettingBonesBehaviour() {        
        _elapsedResetBonesTime += Time.deltaTime;
        float elapsedPercentage = _elapsedResetBonesTime / _timeToResetBones; // Porcentaje de completado de la recuperación de los huesos

        // LERPEO DE LOS HUESOS HACIA LA POSICIÓN Y ROTACIÓN DE LA ANIMACIÓN DE LEVANTARSE
        for (int boneIndex = 0; boneIndex < _bones.Length; boneIndex++) { 
            _bones[boneIndex].localPosition = Vector3.Lerp(
                _ragdollBoneTransforms[boneIndex].Position,
                _standUpBoneTransforms[boneIndex].Position,
                elapsedPercentage);

            _bones[boneIndex].localRotation = Quaternion.Lerp(
                 _ragdollBoneTransforms[boneIndex].Rotation,
                 _standUpBoneTransforms[boneIndex].Rotation,
                 elapsedPercentage);
        }
        if (elapsedPercentage >= 1) { // Cuando este se haya completado...
            currentState = PatueloState.StandingUp; //  Haremos que se levante
            ragdollTime = _ragdollTimeBackup; // Restablecemos el tiempo para que se levante
            RagdollEnabled(ragdollEnabled = false); // Desactiva el ragdoll
            //animator.Play(standUpStateName); // Ejecutamos la animación de levantarse
        }
    }
    /// <summary>
    /// Método que gestiona el alineamiento del Pato con respecta a donde a acabado su armature cuando este vuelva a incorporarse
    /// </summary>
    private void AlignPositionToHips() {
        Vector3 originalHipsPosition = _hipsBone.position; // Guardamos la posición original del Armature
        Quaternion originalHipsRotation = _hipsBone.rotation; // Guardamos la rotación original del Armature

        transform.position = _hipsBone.position; // Alineamos al pato a su nueva posición
        transform.rotation = new Quaternion(transform.rotation.x, _hipsBone.rotation.y, transform.rotation.z, transform.rotation.w); // Alineamos al pato a su nueva rotación

        _hipsBone.position = originalHipsPosition; // Traemos el armature con el
        _hipsBone.rotation = originalHipsRotation; // Rotamos el armature con el
    }
    /// <summary>
    /// Método que guarda el estado actual del transform de los huesos
    /// </summary>
    /// <param name="boneTransforms"></param>
    private void PopulateBoneTransforms(BoneTransform[] boneTransforms) {
        for (int boneIndex = 0; boneIndex < _bones.Length; boneIndex++) {
            boneTransforms[boneIndex].Position = _bones[boneIndex].localPosition; // Guardamos la posición actual de los huesos

          // Debug.Log(boneIndex + ": " + boneTransforms[boneIndex].Position);

            boneTransforms[boneIndex].Rotation = _bones[boneIndex].localRotation; // Guardamos la rotación actual de los huesos
        }
    }
    /// <summary>
    /// Método que guarda el estado del transform de los huesos del primer frame de la animación de levantarse
    /// </summary>
    /// <param name="clipName"></param>
    /// <param name="boneTransforms"></param>
    private void PopulateAnimationStartBoneTransforms(string clipName, BoneTransform[] boneTransforms) {
        Vector3 positionBeforeSampling = transform.position; // Guardamos la posición actual antes del sampleo de huesos
        Quaternion rotationBeforeSampling = transform.rotation; // Guardamos la rotación actual antes del sampleo de huesos

        // Obtendremos la animación de levantarse entre las que hay en el animator según su nombre
        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips) {
            Debug.Log(clip);
            // Si coincide el nombre...
            if (clip.name == clipName) {                
                clip.SampleAnimation(gameObject, 0); // Hacemos un sample de su primer frame
                Debug.Log("--- STANDU UP BONES: -----");
                PopulateBoneTransforms(boneTransforms); // Asignaremos los transforms de ese frame a cada hueso
                break; // Dejamos de buscar
            }
        }
        transform.position = positionBeforeSampling; // Devolvemos a player a su posición inicial
        transform.rotation = rotationBeforeSampling; // Devolvemos a player a su rotación inicial
    }

    public void BounceBack(float bounceForce) {
        currentState = PatueloState.Ragdoll;
        Vector3 directionForce = -transform.forward * bounceForce;
        _hipsVelocity = directionForce;
    }

    public void BounceBackDirection(Vector3 direction, float bounceForce) {
        float speed = Mathf.Clamp(playerRb.velocity.magnitude * 10, 0.2f, 5f);
        Debug.Log(playerRb.velocity.magnitude);
        //currentState = PatueloState.Ragdoll;
        Vector3 directionForce = direction.normalized * bounceForce;    
        _hipsVelocity = directionForce;
    }
}
