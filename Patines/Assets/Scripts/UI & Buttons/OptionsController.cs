using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour
{
    [Header("OPTIONS")]
    public Slider musicSlider;
    public Slider sfxSlider;
    public Slider sensibilitySlider;

    private void Start() {
        musicSlider.value = DataManager.instance.musicVolume;
        sfxSlider.value = DataManager.instance.sfxVolume;
        sensibilitySlider.value = DataManager.instance.sensibilityCam;
    }
    public void Update() {
        DataManager.instance.musicVolume = musicSlider.value;
        DataManager.instance.sfxVolume = sfxSlider.value;
        DataManager.instance.sensibilityCam = sensibilitySlider.value;
    }
}
