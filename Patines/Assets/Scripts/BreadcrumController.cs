using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadcrumController : MonoBehaviour
{
    public int index;

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")) {
            GameManager.instance.AddBreadcrumb(index);
            Destroy(gameObject);
        }
    }
}
