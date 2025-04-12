using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public float delayTransition = 2;
    public CameraFade cameraFade;

    //singelton
    public static SceneController instance;
    private void Awake() {
        //si no existe
        if (instance == null) {
            //la creamos
            instance = this;
        }
    }
    private void Start() {
        
    }
    /// <summary>
    /// Metodo que cambia a la escena que recibe como parametro
    /// </summary>
    /// <param name="sceneName"></param>
    public void ChangeScene(string sceneName) {
        Debug.Log(sceneName);
        StartCoroutine(ChangeSceneWithDelay(sceneName, delayTransition, true));
    }
    public IEnumerator ChangeSceneWithDelay(string nexScene, float delay, bool fade) {
        if (fade){ // Si hemos dicho que haga fade
            cameraFade.StartFade(); // Hacemos fade
        }
        yield return new WaitForSecondsRealtime(delay); // Esperamos el tiempo indicado
        Time.timeScale = 1; // Reanudamos
        SceneManager.LoadScene(nexScene); // Cargamos la siguiente escena
        cameraFade.startFadeIn = true; // Para que cuando entrea  una escena, entre con el fade in
    }
    /// <summary>
    /// método que sale del juego
    /// </summary>
    public void QuitGame() {
        //cerrar el juego
        Application.Quit();
    }
}
