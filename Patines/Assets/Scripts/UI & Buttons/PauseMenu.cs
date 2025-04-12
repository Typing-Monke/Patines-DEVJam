using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    private Animator animator;
    public RotatorController rotationCamera;
    private Coroutine corrutinePause;

    [Header("HUD")]
    //public Sprite broadCrumSpriteReference;
    //texto que muestra la cantidad de migas de pan
    public Image[] breadcrumbsImages = new Image[5];


    public bool _isInGame = true;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SoundManager.instance.StopClip();
            PauseMenuEnabled(!pauseMenu.activeInHierarchy); // Llamamos a la pausa según si está o no activado el menú
        }
    }
    /// <summary>
    /// actualiza los datos de la migas de pan
    /// </summary>
    public void UpdateBreadcrumbsScoreHUD() {
        for (int i = 0; i < breadcrumbsImages.Length; i++) {
            breadcrumbsImages[i].enabled = DataManager.instance.breadcrumbsPicked[i] == 1;
        }
    }
    public void AddBreadcrumb(int index) {
        DataManager.instance.breadcrumbsPicked[index] = 1;
        DataManager.instance.SaveValues();
        UpdateBreadcrumbsScoreHUD();
    }
    /// <summary>
    /// Métood que gestiona la pausa del juego según la boleana que se le devuelva
    /// </summary>
    /// <param name="active"></param>
    public void PauseMenuEnabled(bool active) {
        _isInGame = !active;
        rotationCamera.enabled = !active; // Desactiva rotación al entrar en pausa
        
        StartCoroutine(PlayAnimation(active));
        if (corrutinePause != null) { // Para evitar que se solapen corrutinas
            return;
        }    
    }
    IEnumerator PlayAnimation(bool active) {        
        if (active) {
            animator.SetBool("options", false); // Nos aseguramos de que opciones esté inactivo
            pauseMenu.SetActive(active); // Activamos la pausa            
            Time.timeScale = 0; // Pausa el juego
            Cursor.lockState = CursorLockMode.None;
            animator.Play("PauseIn");
        } else {
            animator.Play("PauseOut");
            yield return new WaitForSecondsRealtime(1f);
            animator.SetBool("options", false); // Nos aseguramos de que opciones se desactive
            Time.timeScale = 1; // Reanudamos
            Cursor.lockState = CursorLockMode.Locked;
            pauseMenu.SetActive(active); // Desactivamos la pausa
            
        }
    }
    /// <summary>
    /// Reintentamos el nivel actual
    /// </summary>
    public void Retry() {
        SceneController.instance.ChangeScene(SceneManager.GetActiveScene().name);       
    }
    /// <summary>
    /// Volvemos al lobby
    /// </summary>
    public void BackToLobby() {
        PauseMenuEnabled(false);
        SceneController.instance.ChangeScene("1_TutorialLevel");
    }

    public void BackToMainMenu() {
        Time.timeScale = 1;
        SceneController.instance.ChangeScene("0_MainMenu");
    }
}
