using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorController : MonoBehaviour {

    [Header("Camera Rotation Settings")]
    public float sensitivity = 2f;
    public float maxXRotation = 45f;
    public float minXRotation = -28f;

    private Vector2 turn;

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        turn.y = -10;
        turn.x = 90;
        transform.localRotation = Quaternion.Euler(-turn.y, turn.x, 0);
    }


    void Update() {
        turn.y += Input.GetAxis("Mouse Y") * sensitivity;
        turn.y = Mathf.Clamp(turn.y, minXRotation, maxXRotation);
        turn.x += Input.GetAxis("Mouse X") * sensitivity;    
        transform.localRotation = Quaternion.Euler(-turn.y, turn.x, 0); 
    }

    public void SetMouseSensitivity(float _sensitivity) {
        sensitivity = _sensitivity;
    }
}
