using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsController : MonoBehaviour
{    
    public Animator animator;
    public PauseMenu pauseController;
    // Start is called before the first frame update
    void Start()
    {
        animator.Play("Credits");                
    }

    private void Update() {
        pauseController.PauseMenuEnabled(false);
    }

    public void AnimationEvent() {
        SceneController.instance.ChangeScene("0_MainMenu");
    }
}
