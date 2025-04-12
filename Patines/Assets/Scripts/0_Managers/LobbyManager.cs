using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public GameObject tutorialObjects;
    public GameObject tutorialPicture;

    void Awake(){
        tutorialObjects.SetActive(PlayerPrefs.GetInt("tutorialMade") == 0);
        tutorialPicture.SetActive(true);
    }
    
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")) {
            tutorialPicture.SetActive(false);
        }
    }
    private void OnTriggerExit(Collider other) {
        if(other.CompareTag("Player")) {
            tutorialPicture.SetActive(true);
        }
    }
}
