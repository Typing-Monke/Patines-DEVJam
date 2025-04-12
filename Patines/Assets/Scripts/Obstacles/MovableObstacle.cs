using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Obstáculo que se mueve hacia delante y detrás
/// 
/// Author: Gonzalo Blanch Domínguez
/// </summary>

public class MovableObstacle : MonoBehaviour {

    public enum Direction {
        Right, Left, Front, Back
    }

    public Transform bodyTransform;
    public float movementDistance = 3f;
    public float speed = 2f;
    public Direction initialDirection;

    private float minX;
    private float minZ;
    private float max = 3f;
    private float range;

    void Start() {

        minX = transform.position.x;
        minZ = transform.position.z;
        max = transform.position.x + movementDistance;
        range = max - minX;
    }

    // Update is called once per frame
    private void Update() {
        max = transform.position.x + movementDistance;

        float offset = Mathf.PingPong(Time.time * speed, range);


        switch (initialDirection) {
            case Direction.Right:
                bodyTransform.localEulerAngles = Vector3.zero;
                // Cambiamos la escala
                if (offset >= range - 0.1) {
                    bodyTransform.localScale = new Vector3(-1, 1, 1);
                } else if (offset <= 0.1) {
                    bodyTransform.localScale = new Vector3(1, 1, 1);
                }
                transform.position = new Vector3(minX + offset, transform.position.y, transform.position.z);
                break;

            case Direction.Left:
                bodyTransform.localEulerAngles = Vector3.zero;
                // Cambiamos la escala
                if (offset >= range - 0.1) {
                    bodyTransform.localScale = new Vector3(1, 1, 1);
                } else if (offset <= 0.1) {
                    bodyTransform.localScale = new Vector3(-1, 1, 1);
                }
                transform.position = new Vector3(minX - offset, transform.position.y, transform.position.z);
                break;

            case Direction.Front:
                bodyTransform.localEulerAngles = new Vector3(0, -90, 0);
                // Cambiamos la escala
                if (offset >= range - 0.1) {
                    bodyTransform.localScale = new Vector3(-1, 1, 1);
                } else if (offset <= 0.1) {
                    bodyTransform.localScale = new Vector3(1, 1, 1);
                }
                transform.position = new Vector3(transform.position.x, transform.position.y, minZ + offset);
                break;

            case Direction.Back:
                bodyTransform.localEulerAngles = new Vector3(0, -90, 0);
                // Cambiamos la escala
                if (offset >= range - 0.1) {
                    bodyTransform.localScale = new Vector3(1, 1, 1);
                } else if (offset <= 0.1) {
                    bodyTransform.localScale = new Vector3(-1, 1, 1);
                }
                transform.position = new Vector3(transform.position.x, transform.position.y, minZ - offset);

                break;

            default:
                break;
        }

        
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;

        switch (initialDirection) {
            case Direction.Right:

                Gizmos.DrawRay(transform.position, transform.right * movementDistance);
                break;

            case Direction.Left:

                Gizmos.DrawRay(transform.position, -transform.right * movementDistance);
                break;

            case Direction.Front:

                Gizmos.DrawRay(transform.position, transform.forward * movementDistance);
                break;

            case Direction.Back:

                Gizmos.DrawRay(transform.position, -transform.forward * movementDistance);
                break;

            default:
                break;
        }
    }

}
