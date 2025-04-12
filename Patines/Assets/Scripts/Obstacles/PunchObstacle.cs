using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PunchObstacle : MonoBehaviour
{
    public UnityEvent<Vector3, float> OnPunch;

    public float punchForce = 2;
    public float punchTime = 2;

    private BoxCollider bCollider;
    private PlayerMovement pMovement;
    private StateMachineController stateMachine;
    private PlayerMovementState movementState;
    private float _currentPunchTimer;
    [SerializeField]
    private Animator _animator;

    private void OnValidate() {
        bCollider = GetComponentInChildren<BoxCollider>();
        _animator = GetComponentInParent<Animator>();
    }

    private void Start() {
        //bCollider = GetComponent<BoxCollider>();
        _currentPunchTimer = punchTime;
    }
    private void Update() {
        Timer();
    }
    //private void OnCollisionEnter(Collision other) {
    //    if (other.gameObject.CompareTag("Player")) {
    //        Debug.Log("Mamawebo");
    //        stateMachine = other.gameObject.GetComponentInChildren<StateMachineController>();
    //        movementState = other.gameObject.GetComponentInChildren<PlayerMovementState>();
    //        pMovement = other.gameObject.GetComponent<PlayerMovement>();
    //        stateMachine.SetState("Movement");
    //        movementState.fixForwardVelocity = false;
    //        pMovement._velocity = punchForce * transform.forward;
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Mamawebo");
            stateMachine = other.gameObject.GetComponentInChildren<StateMachineController>();
            movementState = other.gameObject.GetComponentInChildren<PlayerMovementState>();
            pMovement = other.gameObject.GetComponent<PlayerMovement>();
            //stateMachine.SetState("Movement");
            //movementState.fixForwardVelocity = false;
            //pMovement._velocity = punchForce * transform.forward;
            stateMachine.SetState("Ragdoll");
            OnPunch.Invoke(transform.forward, punchForce);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Vector3 rayGizmoOffset = new Vector3(bCollider.size.z / 2, 0);
        Gizmos.DrawLine(transform.position, transform.position + rayGizmoOffset);
    }

    public void Timer() {
        _currentPunchTimer -= Time.deltaTime;
        if (_currentPunchTimer <= 0) {
            _currentPunchTimer = punchTime;
            _animator.Play("PunchAnimation");
        }
    }
}
