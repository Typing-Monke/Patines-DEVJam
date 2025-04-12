using JetBrains.Annotations;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
//using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Events;


public class GameManager : MonoBehaviour
{
    public UnityEvent RestartPlataforms;

    [Header("Tutorial")]
    public bool isTutorialLevel = false;
    //public GameObject tutorialObjects;

    [Header("References")]
    //referencia al player
    public PlayerController player;
    //referencias a la camara
    public GameObject cam;
    private PlayerMovement playerMov;
    [SerializeField]
    private RagdollController_2 playerRagdoll;
    private RotatorController _camRotator;
    private CameraController _camController;
    // Pause menu
    public PauseMenu pauseMenu;

    [Header("BreadCrumbs")]
    public GameObject[] breadcrumbs;
    public GameObject breadcrumbReference;
    public GameObject breadcrumbPickedReference;

    [Header("Respawn")]
    public float playerRespawnTime;
    public float cameraRespawnTime;
    public float restartRespawnTime;

    [Header("EndLevelTime")]
    public float endLevelTime;

    [Header("CheckPoints")]
    public Transform[] checkPoints;
    public BoxCollider[] checkPointsCollider;

    [SerializeField]
    private int _currentCheckPoints;

    //nivel completado con la máxima puntuación
    [Header("Level Complete")]
    //puntuacion de migas de pan con la que el nivel se va a completar
    public int breadcrumbsScoreToLevelComplete = 5;

    [Header("Sounds")]
    //sonido de muerte
    public string gameOverSound;
    //sonido de finalizacion del nivel
    public string endLevelSound;
    //musica del nivel
    public string levelMusic;

    //contador interno del cronometro del nivel
    private float _currentTimerScore;
    //controlador del menu de pausa
    private bool _isPaused;

    // Instancia del Singleton
    public static GameManager instance;
    private void Awake() {
        //Inicializamos a la instancia del singleton
        if (instance == null) {
            instance = this;
        } else {
            // Si ya esta inicializada, destruimos ESTE objeto (componente)
            Destroy(this);
        }
    }
    private void Start() {
        //añadimos las referencias
        playerMov = player.gameObject.GetComponent<PlayerMovement>();
        playerRagdoll = player.gameObject.GetComponent<RagdollController_2>();
        _camRotator = cam.gameObject.GetComponent<RotatorController>();
        _camController = cam.gameObject.GetComponent<CameraController>();
        Application.targetFrameRate = 60;

        //reiniciamos el timer
        _currentTimerScore = 0;

        if(isTutorialLevel) {
            _currentCheckPoints = PlayerPrefs.GetInt("tutorialMade");
            if(PlayerPrefs.GetInt("tutorialMade") == 0) {
                PlayerPrefs.SetInt("tutorialMade", 1);
            }
        } else {
            _currentCheckPoints = 0;
            UpdateBreadcrumbs();
        }
        //Inicializamos las migas de pan
        //InitializeBreadcrumbsHUD();
        pauseMenu.UpdateBreadcrumbsScoreHUD();
     

        //comenzamos el juego
        StartCoroutine(IniciateSecuence());
    }
    private void Update() { 
        if (Input.GetButtonDown("Escape")) {
            _isPaused = !_isPaused;
        }
        if (Input.GetKeyDown(KeyCode.R)) {
            ResPawn();
        }
        //if (Input.GetKeyDown(KeyCode.T)) {
        //    AddBreadcrumb();
        //}
    }
    //public void InitializeBreadcrumbsHUD() {
    //    breadcrumbsImages = new Image[breadcrumbsScoreToLevelComplete];
    //    for (int i = 0; i < breadcrumbsImages.Length; i++) {
    //        breadcrumbsImages[i].sprite = broadCrumSpriteReference;
    //    }
    //    UpdateBreadcrumbsScoreHUD();
    //}
    public void AddBreadcrumb(int index) {
        DataManager.instance.breadcrumbsPicked[index] = 1;
        DataManager.instance.SaveValues();
        pauseMenu.UpdateBreadcrumbsScoreHUD();
    }

    public void UpdateBreadcrumbs() {
        for(int i = 0; i < breadcrumbs.Length; i++) {
            if(DataManager.instance.breadcrumbsPicked[i] == 0) {
                Instantiate(breadcrumbReference, breadcrumbs[i].transform.position, Quaternion.identity, breadcrumbs[i].transform);
            } else {
                Instantiate(breadcrumbPickedReference, breadcrumbs[i].transform.position, Quaternion.identity, breadcrumbs[i].transform);
            }
        }
    }
    /// <summary>
    /// Metodo que respawnea al jugador
    /// </summary>
    public void ResPawn() {
        StartCoroutine(RespawnSecuence());

        RestartPlataforms.Invoke();
    }
    public IEnumerator RespawnSecuence() {
        _camController.enabled = false;
        _camRotator.enabled = false;
        SceneController.instance.cameraFade.StartFade();

        yield return new WaitForSeconds(playerRespawnTime);
        
        playerRagdoll.currentState = RagdollController_2.PatueloState.ResettingBones;
        playerMov._playerCanMove = false;
        player.gameObject.transform.position = checkPoints[_currentCheckPoints].transform.position;
        
        yield return new WaitForSeconds(cameraRespawnTime);
        SceneController.instance.cameraFade.StartFade();
        cam.transform.position = checkPointsCollider[_currentCheckPoints].transform.position;
        playerMov._playerCanMove = true;

        yield return new WaitForSeconds(restartRespawnTime);
        
        _camController.enabled = true;
        _camRotator.enabled = true;

        //pausamos la musica
        //MusicManager.instance.PauseMusic();

        //reproducomos el sonido de muerte
        //SoundManager.instance.PlayClip(gameOverSound);
    }
    public IEnumerator IniciateSecuence() {
        _camController.enabled = false;
        _camRotator.enabled = false;
        cam.transform.position = checkPointsCollider[_currentCheckPoints].transform.position;
        playerRagdoll.currentState = RagdollController_2.PatueloState.ResettingBones;
        player.gameObject.transform.position = checkPoints[_currentCheckPoints].transform.position;

        yield return new WaitForSeconds(restartRespawnTime);

        _camController.enabled = true;
        _camRotator.enabled = true;

        //pausamos la musica
        //MusicManager.instance.PauseMusic();

        //reproducomos el sonido de muerte
        //SoundManager.instance.PlayClip(gameOverSound);
    }
    public void UpdateCurrentCheckPoint(int current) {
        _currentCheckPoints = current;
    }
    /// <summary>
    /// Método que realiza las acciones al finalizar el nivel
    /// </summary>
    public void EndLevel() {
        PlayerPrefs.SetInt("levelPassed" + DataManager.instance.index, 1);

        //creo una booleana, y si alguna de las migas de pan no ha sido cogida, se pone a false
        bool levelComplete = true;
        for(int i = 0; i < DataManager.instance.breadcrumbsPicked.Length; i++) {
            if(DataManager.instance.breadcrumbsPicked[i] == 0) {
                levelComplete = false;
                break;
            }
        }

        //Si ha cogido todas las migas de pan...
        if (levelComplete) {
            //Se marca ese nivel como "completo"
            PlayerPrefs.SetInt("levelComplete" + DataManager.instance.index, 1);
        }

        UpdatefinalScore();

        StartCoroutine(EndLevelSecuence());

        //pausamos la musica
        //MusicManager.instance.PauseMusic();

        //reproducomos el sonido de muerte
        //SoundManager.instance.PlayClip(endLevelSound);
    }
    public IEnumerator EndLevelSecuence() {
        yield return new WaitForSeconds(endLevelTime);
        SceneController.instance.ChangeScene("1_TutorialLevel");
    }
    /// <summary>
    ///Método que actualiza la puntuación final
    /// </summary>
    public void UpdatefinalScore() {

        //asigno en una variabletemporal el valor de la puntuación máxima que había guardado
        float timeRecord = DataManager.instance.timeRecord;

        if(_currentTimerScore > timeRecord) {
            //asigno el valor de la puntuación en la variable temporal
            timeRecord = _currentTimerScore;
            //asigno en la variable del data manager el valor de la variable temporal
            //actualizamos la info que hemos modificado en el data manager
            DataManager.instance.timeRecord = timeRecord;
            DataManager.instance.SaveValues();
        }
    }
}
