using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    // PUBLICS
    [Header("Reference")]
    public Transform cameraTarget;
    [Header("Camera lerp settings")]
    [Tooltip("If the value is increased, the follow movement is faster")]
    public float positionLerpTime = 0.02f;
    [Tooltip("If the value is increased, the rotation movement is faster")]
    public float rotationLerpTime = .01f;

    // PRIVATES
    private float _positionLerp;
    private float _rotationLerp;

    private void Start()
    {
        
    }

    void FixedUpdate() {
        _positionLerp = Time.deltaTime / positionLerpTime;
        _rotationLerp = Time.deltaTime / rotationLerpTime;

        transform.position = Vector3.Lerp(transform.position, cameraTarget.position, _positionLerp);
        transform.rotation = Quaternion.Lerp(transform.rotation, cameraTarget.rotation, _rotationLerp); 
    }
}
