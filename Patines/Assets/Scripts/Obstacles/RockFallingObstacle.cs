using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockFallingObstacle : MonoBehaviour
{
    [Range(0,100)] public int probability = 100;
    public float rockFallingForce = 2f;
    public float activationDelay = 2;
    public Vector3 rockFallingAcivationFieldSize = new Vector3 (0.9f, 0.1f, 0.9f);
    public Vector3 rockFallingCollisionOffSet;

    private BoxCollider _bCollider;
    private Rigidbody _rb;
    private Animator _animator;
    private bool _isActivated;
    private float _currentActivationDelay;
    private Vector3 initialPos;
    private Quaternion initialRotation;

    private void OnValidate() {
        _bCollider = GetComponent<BoxCollider>();
        _rb = GetComponent<Rigidbody>();
        _bCollider.size = rockFallingAcivationFieldSize;
        _bCollider.center = rockFallingCollisionOffSet;
    }

    void Start() {
        _bCollider = GetComponent<BoxCollider>();
        _animator = GetComponentInChildren<Animator>();
        _bCollider.center = rockFallingCollisionOffSet;
        _currentActivationDelay = activationDelay;
        initialPos = transform.position;
        initialRotation = transform.rotation;
    }

    private void Update() {
        if (_isActivated) {
            _currentActivationDelay -= Time.deltaTime;
        }
        if (_currentActivationDelay <= 0) {
            _rb.velocity = rockFallingForce * -transform.up;
            _currentActivationDelay = activationDelay;
            _isActivated = false;
        }
        if (_isActivated) {
            _animator.Play("ActiveAnimation");
        } else {
            _animator.Play("Empty");
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            int random = Random.Range(0, 100);
            if(random < probability) {
                _rb.isKinematic = false;
                _isActivated = true;
            }
        }
    }

    public void RestartObstacle() {
        //_rb.velocity = Vector3.zero;
        _isActivated = false;
        _rb.isKinematic = true;
        transform.position = initialPos;
        transform.rotation = initialRotation;
    }
}
