using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class LevelTrigger : MonoBehaviour
{
    public string scene;
    public int triggerIndex;
    public GameObject padLock;
    private bool _canInteract = false;
    public string text;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(triggerIndex > 2) {
                if (PlayerPrefs.GetInt("levelPassed" + (triggerIndex - 1)) == 1) {
                    GameManager.instance.player.eInteractuable.SetActive(true);
                    _canInteract = true;

                }
            } else {// Para primer nivel
                GameManager.instance.player.eInteractuable.SetActive(true);
                _canInteract = true;
            }

            if (_canInteract && Input.GetButton("Interact")) {
                SceneController.instance.ChangeScene(scene);
                Debug.Log(scene);
                Debug.Log(text);
            }
        }
    }
    private void OnTriggerExit(Collider other) {
        if (CompareTag("Interactuable")) {
            GameManager.instance.player.eInteractuable.SetActive(false);
            //_canInteract = false;
        }
    }

    private void Update() {
        if(triggerIndex > 2 && PlayerPrefs.GetInt("levelPassed" + (triggerIndex - 1)) == 1) {
            _canInteract = true;
        }

        

        if (_canInteract) {
            padLock.SetActive(false);
        }
    }
}
