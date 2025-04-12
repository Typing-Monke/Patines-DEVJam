using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingObstacleMovement : MonoBehaviour
{
    // PUBLICS
    [Header("Config")]
    public Vector3 amplitude = new Vector3 (1f, 1f, 1f);
    public float frequency = 1.0f;
    public enum AxisRotation {
        YAxis,
        XAxis,
        ZAxis,
    }

    public AxisRotation axisRotation;
    //booleana para el sentido horario o antihorario
    public bool clockWise;

    [HideInInspector]
    public bool rotatingLeft = false;

    // PRIVATES
    private float _timerCounter = 0;
    private Vector3 _initialPosition;
    private Vector3 _currentposition;

    public float X {
        get {return _currentposition.x; }
    }
    public float Y {
        get { return _currentposition.y; }
    }
    public float Z {
        get { return _currentposition.z; }
    }

    private void Start()
    {
        _initialPosition = transform.position;
        _currentposition = transform.position;
    }

    private void Update()
    {
        _timerCounter += Time.deltaTime;
        if(axisRotation == AxisRotation.XAxis) {
            if(clockWise) {
                _currentposition.y = _initialPosition.y + (Mathf.Sin(frequency * _timerCounter) * amplitude.y);
                _currentposition.z = _initialPosition.z + (Mathf.Cos(frequency * _timerCounter) * amplitude.z);
            } else {
                _currentposition.y = _initialPosition.y + (Mathf.Cos(frequency * _timerCounter) * amplitude.y);
                _currentposition.z = _initialPosition.z + (Mathf.Sin(frequency * _timerCounter) * amplitude.z);
            }
        } else if(axisRotation == AxisRotation.YAxis) {
            if(clockWise) {
                _currentposition.x = _initialPosition.x + (Mathf.Sin(frequency * _timerCounter) * amplitude.x);
                _currentposition.z = _initialPosition.z + (Mathf.Cos(frequency * _timerCounter) * amplitude.z);
            } else {
                _currentposition.x = _initialPosition.x + (Mathf.Cos(frequency * _timerCounter) * amplitude.x);
                _currentposition.z = _initialPosition.z + (Mathf.Sin(frequency * _timerCounter) * amplitude.z);
            }
        } else if(axisRotation == AxisRotation.ZAxis) {
            if(clockWise) {
                _currentposition.x = _initialPosition.x + (Mathf.Sin(frequency * _timerCounter) * amplitude.x);
                _currentposition.y = _initialPosition.y + (Mathf.Cos(frequency * _timerCounter) * amplitude.y);
            } else {
                _currentposition.x = _initialPosition.x + (Mathf.Cos(frequency * _timerCounter) * amplitude.x);
                _currentposition.y = _initialPosition.y + (Mathf.Sin(frequency * _timerCounter) * amplitude.y);
            }
        }

        rotatingLeft = clockWise;
        transform.position = _currentposition;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        
        Gizmos.DrawRay(transform.position, transform.right * amplitude.x);
        Gizmos.DrawRay(transform.position, -transform.right * amplitude.x);
        Gizmos.DrawRay(transform.position, transform.up * amplitude.y);
        Gizmos.DrawRay(transform.position, -transform.up * amplitude.y);
        Gizmos.DrawRay(transform.position, transform.forward * amplitude.z);
        Gizmos.DrawRay(transform.position, -transform.forward * amplitude.z);
    }
}
