using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/// <summary>
/// Script que gestiona las opciones y botones del menú principañ
/// Author: Carlos
/// </summary>
public class StartMenuController : MonoBehaviour
{
    [Header("Default Settings Values")]
    [Range(0f, 1f)] public float defaultSfxVolume;
    [Range(0f, 1f)] public float defaultMusicVolume;
    [Range(0.1f, 4f)] public float defaultSensibilityCam;

    private Coroutine corrutinaKemekaigo;
    private Coroutine corrutinaPulseButton;
    [Header("References")]
    public Rigidbody patueloRB;
    public GameObject hud;
    public GameObject startBlock;
    [Header("Animators")]
    public Animator patueloAnimation;
    public Animator menuAnimator;
    private GameObject pulsedButon; // Guarda el botón que hemos pulsado

    private void Awake() {
        
        

    }
    private void Start() {
        if (PlayerPrefs.GetInt("tutorialMade") == 0) {
            DataManager.instance.sfxVolume = defaultSfxVolume;
            DataManager.instance.musicVolume = defaultMusicVolume;
            DataManager.instance.sensibilityCam = defaultSensibilityCam;
            DataManager.instance.SaveSettings();
        }
        startBlock.SetActive(false); // Nos aseguramos que el bloque esté oculto
        StartCoroutine(StartMainMenu()); // Ejecutamos la animación de entrada
        Cursor.lockState = CursorLockMode.None;
    }
    /// <summary>
    /// Borra los datos de la anterior partida y cargamos en el tutorial
    /// </summary>
    public void NewGame() {
        DataManager.instance.ResetValues();
        PlayCorrutineOneTime(corrutinaKemekaigo, Kemekaigo("1_TutorialLevel")); // Iniciamos corrutina
        menuAnimator.Play("LabelsOut");
        menuAnimator.SetBool("allOut", true);
    }
    /// <summary>
    /// Cárga el lobby con los niveles que hayamos completado en la anterior partida
    /// </summary>
    public void LoadLastPoint() {
        PlayCorrutineOneTime(corrutinaKemekaigo, Kemekaigo("1_TutorialLevel")); // Iniciamos corrutina
        menuAnimator.Play("LabelsOut");
        menuAnimator.SetBool("allOut", true);
    }
    /// <summary>
    /// Salimos del juego
    /// </summary>
    public void Exit() {
        Debug.Log("Exit");
    }/// <summary>
    /// Método que gestiona lo que sucede al pulsar un botón
    /// </summary>
    /// <param name="labelBlockIn"></param>
    public void ShowBlock(GameObject labelBlockIn) {
        corrutinaPulseButton = StartCoroutine(PulseButtonTransition(labelBlockIn));
        if (corrutinaPulseButton != null) { // Para evitar que se solapen corrutinas
            return;
        }
    }
    /// <summary>
    /// Método que gestiona que bloque de labels del menú vamos a ocultar
    /// </summary>
    /// <param name="labelBlockOut"></param>
    public void HideBlock(GameObject labelBlockOut) {
        pulsedButon = labelBlockOut;
    }
    /// <summary>
    /// Método que gestiona que una corrutina no se ejecute mientras una está activa
    /// </summary>
    /// <param name="corrutine"></param>
   public void PlayCorrutineOneTime(Coroutine corrutine, IEnumerator enumerator) {
        corrutine = StartCoroutine(enumerator); // Ejecutamos corrutina
        if (corrutine != null) { // Para evitar que se solapen corrutinas
            return;
        }
    }
    /// <summary>
    /// Corrutina que hace que el pato se caiga, espera 5 segundos y transiciona al nivel elegido
    /// </summary>
    /// <param name="scene">Nivel elegido</param>
    /// <returns></returns>
    IEnumerator Kemekaigo(string scene) {
        patueloAnimation.Play("Kemecaigo"); // Ejecutamos animación de caida
        // hud.SetActive(false); 
        yield return new WaitForSeconds(2);
        patueloRB.useGravity = true;
        SceneController.instance.ChangeScene(scene);
    }
    /// <summary>
    /// Corrutina que gestiona la entada y salida de los labels
    /// </summary>
    /// <param name="labelBlockIn"></param>
    /// <returns></returns>
    IEnumerator PulseButtonTransition(GameObject labelBlockIn) {
        menuAnimator.Play("LabelsOut");
        yield return new WaitForSecondsRealtime(0.60f);        
        pulsedButon.SetActive(false);
        labelBlockIn.SetActive(true);
       // menuAnimator.Play("LabelsIn");        
    }
    IEnumerator StartMainMenu() {        
        menuAnimator.Play("TitleIn");
        yield return new WaitForSeconds(1.8f);
        startBlock.SetActive(true); // Mostramos el primer bloque de botones
    }
}
