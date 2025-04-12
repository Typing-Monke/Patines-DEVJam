using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    //cambiar cuando se quiera modular//
    //volumen del sonido
    private float soundVolumeMod = 1;

    //referencia al componmente audiosource
    private AudioSource audioSource;

    //singelton del sound manager
    public static SoundManager instance;

    private void Awake() {
        //si no existe la instancia
        if(instance == null) {
            instance = GetComponent<SoundManager>();
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
        LoadSound();
    }
    private void Update() {
        LoadSound();
    }
    /// <summary>
    /// Método que se encarga de reproducir el sonido que recibe como parámetro
    /// </summary>
    /// <param name="clip"></param>
    public void PlayClip(AudioClip clip) {
        //reproduco el sonido que le paso por el parámetro
        audioSource.PlayOneShot(clip);
    }
    public void StopClip() {
        audioSource.Stop();
    }
    public void LoadSound() {
        //so hemos recuperado la referencia al audiosource
        if(audioSource != null) {
            //cargamos en el audioSource el valor del volumen que tenemos guardado
            audioSource.volume = DataManager.instance.sfxVolume * soundVolumeMod;
        }

    }
}