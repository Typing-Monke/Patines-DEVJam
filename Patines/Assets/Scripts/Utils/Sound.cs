using UnityEngine.Audio;
using UnityEngine;

/// <summary>
/// Clase para definir tipo de dato para un sonido.
/// </summary>

[System.Serializable]
public class Sound 
{
    public enum SoundType
    {
        Music, 
        FX
    }

    // Nombre del clip
    public string name;
    // Tipo de sonido (Music/FX)
    public SoundType tag;
    // CLip
    public AudioClip clip;
    // Volumen delclip
    [Range(0f, 1f)]
    public float volume;
    // Si es loopeable
    public bool loop;
    // Instancia de audio source para copiar datos del clip
    [HideInInspector]
    public AudioSource source;       
}
