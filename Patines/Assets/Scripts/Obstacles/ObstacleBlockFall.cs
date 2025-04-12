using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleBlockFall : MonoBehaviour
{
    public float blockFallForce = 2f;
    public float activationDelay = 2;
    public Vector3 blockFallAcivationFieldSize = new Vector3 (0.9f, 0.1f, 0.9f);
    public float blockFallAcivationOffset = 0.1f;

    private BoxCollider _bCollider;
    private Rigidbody _rb;
    private Animator _animator;
    private bool _isActivated;
    private float _currentActivationDelay;
    private Vector3 initialPos;

    private void OnValidate() {
        _bCollider = GetComponent<BoxCollider>();
        _rb = GetComponent<Rigidbody>();
        _bCollider.size = blockFallAcivationFieldSize;
        _bCollider.center = transform.up * blockFallAcivationOffset;
    }

    void Start() {
        _bCollider = GetComponent<BoxCollider>();
        _animator = GetComponentInChildren<Animator>();
        _currentActivationDelay = activationDelay;
        initialPos = transform.position;
    }

    private void Update() {
        if (_isActivated) {
            _currentActivationDelay -= Time.deltaTime;
        }
        if (_currentActivationDelay <= 0) {
            _rb.velocity = blockFallForce * -transform.up;
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
            _rb.isKinematic = false;
            _isActivated = true;
        }
    }

    public void RestartObstacle() {
        //_rb.velocity = Vector3.zero;
        _isActivated = false;
        _rb.isKinematic = true;
        transform.position = initialPos;
    }
}