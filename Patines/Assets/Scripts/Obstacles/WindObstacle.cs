using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindObstacle : MonoBehaviour
{
    public float windForce;
    public Vector3 windFieldSize;
     
    private PlayerMovement pMovement;
    private Vector3 windForceVector;
    private BoxCollider bCollider;

    private void Start()
    {
        windForceVector = new Vector3(windForce, 0, 0);
        bCollider = GetComponent<BoxCollider>();
    }

    private void OnValidate()
    {
        bCollider = GetComponent<BoxCollider>();
        bCollider.size = windFieldSize;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            pMovement = other.gameObject.GetComponent<PlayerMovement>();
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //windForceVector = new Vector3(0, windForce, 0);
            //pMovement._velocity += windForceVector;

            pMovement._velocity += windForce * transform.up;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 rayGizmoOffset = new Vector3(0, bCollider.size.y/2, 0);
        Gizmos.DrawLine(transform.position, transform.position + rayGizmoOffset);
    }
}
