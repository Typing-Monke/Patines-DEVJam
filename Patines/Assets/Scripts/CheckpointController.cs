using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    public int index;
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            GameManager.instance.UpdateCurrentCheckPoint(index);
        }
    }
}
