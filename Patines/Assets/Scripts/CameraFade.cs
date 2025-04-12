using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFade : MonoBehaviour
{

    public float speedScale = 1f;
    public Color fadeColor = Color.white;
    public AnimationCurve curve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(0.5f, 0.5f, -1.5f, -1.5f), 
        new Keyframe(1, 0));
    public bool startFadeIn = false;

    private float _alpha = 0f;
    private Texture2D _texture;
    private int _direction = 0;
    private float _time = 0f;
    private bool startFade; // Para ejecutar el fade cuando se active
    
    void Start()
    {
        if (startFadeIn) // Si la escena comienza con el FadeIn activado
        {
            _alpha = 1f;
            startFade = true;
        }
        else
        {
            _alpha = 0f;
        }
        _texture = new Texture2D(1, 1);
        _texture.SetPixel(0, 0, new Color(fadeColor.r, fadeColor.g, fadeColor.b, _alpha));
        _texture.Apply();
    }

    
    void Update()
    {
        if(_direction == 0 && startFade == true)  // Si se ha establecido que el fade se ha ejecutado...
        {
            if(_alpha >= 1f)
            {
                _alpha = 1f;
                _time = 0;
                _direction = 1;
            }
            else
            {
                _alpha = 0;
                _time = 1;
                _direction = -1;
            }
        }
    }
    /// <summary>
    /// Método que comienza el Fade
    /// </summary>
    public void StartFade() {
        startFade = true;
    }

    public void OnGUI()
    {
        if(_alpha > 0f)
        {
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), _texture);
        }

        if(_direction != 0)
        {
            _time += _direction * Time.deltaTime * speedScale;
            _alpha = curve.Evaluate(_time);
            _texture.SetPixel(0, 0, new Color(fadeColor.r, fadeColor.g, fadeColor.b, _alpha));
            _texture.Apply();
            if(_alpha <= 0f || _alpha >= 1f)
            {
                _direction = 0;
                startFade = false; // Establecemos que el fade se ha ejecutado
            }
        }
    }
}
