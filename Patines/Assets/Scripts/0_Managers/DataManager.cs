using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    //indice del nivel
    public int index;
    //numero maximo y minimo de niveles
    public int minLevelIndex = 2;
    public int maxLevelIndex = 4;
    //variable que contiene la cantidad de miguitas de pan que el player ha cogido
    public int[] breadcrumbsPicked = new int[5];
    //variavle que contiene el record en tiempo de cada nivel
    public float timeRecord;
    //variable que contiene el volumen del sonido para saber si esta activo o muteado
    [Range(0f, 1f)] public float sfxVolume;
    //variable que contiene el volumen del sonido para saber si esta activo o muteado
    [Range(0f, 1f)] public float musicVolume;
    //variable que contiene el volumen del sonido para saber si esta activo o muteado
    [Range(0.1f, 4f)] public float sensibilityCam;

    //singelton
    public static DataManager instance;

    private void Awake() {
        //si no existe
        if (instance == null) {
            //la creamos
            instance = this;
        }
        Debug.Log("tutorial" + PlayerPrefs.GetInt("tutorialMade"));
        //Cargamos los datos guardados
        Load();
    }
    private void Start()  {
        //le decimos al juego que no esta en pausa para iniciarlo
        Time.timeScale = 1f;
        //MusicManager.instance.PlayMusic();
    }
    private void Update() {
        if(Input.GetKeyDown(KeyCode.M)) {
            ResetValues();
            Load();          
        }
        if(Input.GetKeyDown(KeyCode.T)) {
            if(PlayerPrefs.GetInt("tutorialMade") == 0) {
                PlayerPrefs.SetInt("tutorialMade", 1);
            } else {
                PlayerPrefs.SetInt("tutorialMade", 0);
            }
            Debug.Log("tutorial" + PlayerPrefs.GetInt("tutorialMade"));
        }
    }
    /// <summary>
    /// Método que realiza el guardado de datos
    /// </summary>
    public void SaveValues() {
        //almaceno y guardo la informacion de las miguitas de pan
        for(int i = 0; i < breadcrumbsPicked.Length; i++) {
            PlayerPrefs.SetInt("breadcrumbsScore" + index.ToString() + i.ToString(), breadcrumbsPicked[i]);
        }
        //almaceno y guardo la informacion del record de tiempo
        PlayerPrefs.SetFloat("timeRecord" + index, timeRecord);
    }
    public void SaveSettings() {
        //almaceno y guardo la informacion del sonido en un playerPref
        PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
        //almaceno y guardo la informacion del sonido en un playerPref
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
        //almaceno y guardo la informacion del sonido en un playerPref
        PlayerPrefs.SetFloat("sensibilityCam", sensibilityCam);
    }
    /// <summary>
    /// Métodod que reccupera la info almacenada en el playerPref
    /// </summary>
    public void Load() {
        //asigno el valor almacenado en el playerPref en la variable de cada manager
        for(int i = 0; i < breadcrumbsPicked.Length; i++) {
            breadcrumbsPicked[i] = PlayerPrefs.GetInt("breadcrumbsScore" + index.ToString() + i.ToString());
        }
        timeRecord = PlayerPrefs.GetFloat("timeRecord" + index);

        //compruebo si tengo almacenado en un playerPref del sfxVolume
        if (PlayerPrefs.HasKey("sfxVolume")) {
            sfxVolume = PlayerPrefs.GetFloat("sfxVolume");
        }
        //compruebo si tengo almacenado en un playerPref del musicVolume
        if (PlayerPrefs.HasKey("musicVolume")) {
            musicVolume = PlayerPrefs.GetFloat("musicVolume");
        }
        //compruebo si tengo almacenado en un playerPref del musicVolume
        if (PlayerPrefs.HasKey("sensibilityCam")) {
            sensibilityCam = PlayerPrefs.GetFloat("sensibilityCam");
        }
    }

    /// <summary>
    /// Método que resetea los valores
    /// </summary>
    public void ResetValues() {
        //recorremos cada uno de los indices de escena que tenemos
        for (int i = minLevelIndex; i <= maxLevelIndex; i++) {
            for(int j = 0; j < breadcrumbsPicked.Length; j++) {
                PlayerPrefs.SetInt("breadcrumbsScore" + i.ToString() + j.ToString(), 0);
                Debug.Log("breadcrumbsScore" + index.ToString() + i.ToString());
            }           
            PlayerPrefs.SetFloat("timeRecord" + i, 0);
            PlayerPrefs.SetInt("levelPassed" + i, 0);
            PlayerPrefs.SetInt("levelComplete" + i, 0);

        }
        PlayerPrefs.SetInt("tutorialMade", 0);
    }
    private void ResetSettings() {
        PlayerPrefs.SetFloat("sfxVolume", 0.5f);
        PlayerPrefs.SetFloat("musicVolume", 0.5f);
        PlayerPrefs.SetFloat("sensibilityCam", 2);
    }
    public void PlayURL(string URL) {
        Application.OpenURL(URL);
    }
}
