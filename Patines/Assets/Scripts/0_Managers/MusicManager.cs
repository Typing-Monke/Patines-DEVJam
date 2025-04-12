using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {
    //volumen de  la musica
    private float musicVolumeMod = 1;

    //referencia al componmente audiosource
    private AudioSource audioSource;

    //singelton del sound manager
    public static MusicManager instance;

    private void Awake() {
        //si no existe la instancia
        if(instance == null) {
            instance = GetComponent<MusicManager>();
        }
        //si elñ audioSource es nulo(no lo hemos recuperado)
        if(audioSource == null) {
            //lo recupero
            audioSource = GetComponent<AudioSource>();
        }
    }
    void Start() {
        //recupero la referencia al componente audiosource
        audioSource = GetComponent<AudioSource>();
        //cargamos el valor del sonido
        LoadMusic();
    }
    private void Update() {
        LoadMusic();
    }
    /// <summary>
    /// Método que se encarga de reproducir el sonido que recibe como parámetro
    /// </summary>
    /// <param name="clip"></param>
    //public void PlayClip(AudioClip clip) {
    //    //reproduco el sonido que le paso por el parámetro
    //    audioSource.PlayOneShot(clip);
    //}
    public void LoadMusic() {
        //si hemos recuperado la referencia al audiosource
        if(audioSource != null) {
            //cargamos en el audioSource el valor del volumen que tenemos guardado
            audioSource.volume = DataManager.instance.musicVolume * musicVolumeMod;
        }

    }
    ////Sin usar//usadas//
    //public void PauseMusic() {
    //    //cambiamos el valor del volumen a 0 (está muteado)
    //    musicInGameVolume = 0;
    //    //guardamos el valor
    //    LoadMusic();
    //}
    ////Sin usar//
    //public void PlayMusic() {
    //    //cambiamos el valor del volumen a 1 (está con sonido)
    //    musicInGameVolume = 1;
    //    //guardamos el valor
    //    LoadMusic();
    //}
}