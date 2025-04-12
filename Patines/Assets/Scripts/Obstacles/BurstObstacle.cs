using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstObstacle : MonoBehaviour
{
    public float burstForce;
    public Vector3 burstFieldSize;
     
    private PlayerMovement pMovement;
    private BoxCollider bCollider;

    private void OnValidate() {
        bCollider = GetComponent<BoxCollider>();
        bCollider.size = burstFieldSize;
    }
    
    private void Start() {
        bCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            pMovement = other.gameObject.GetComponent<PlayerMovement>();

            pMovement._velocity = burstForce * transform.forward;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Vector3 rayGizmoOffset = new Vector3(0, 0, bCollider.size.z/2);
        Gizmos.DrawLine(transform.position, transform.position + rayGizmoOffset);
    }
}
