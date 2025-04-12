using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartController : MonoBehaviour
{
    public bool isEnd;

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")) {
            if(isEnd) {
                GameManager.instance.EndLevel();
            } else {
                GameManager.instance.ResPawn();
            }
        }
    }
}
