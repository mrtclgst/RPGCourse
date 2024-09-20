using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI_VolumeSlider : MonoBehaviour
{
    public Slider Slider;
    public string Parameter;
    [SerializeField] private AudioMixer _audioMixer;

    public void SliderValue()
    {
        if (Slider.value < 0.01f)
        {
            _audioMixer.SetFloat(Parameter, -80);
        }
        else
        {

            _audioMixer.SetFloat(Parameter, Mathf.Log10(Slider.value) * 20);
        }
    }
    public void LoadSlider(float value)
    {
        Slider.value = value;
        SliderValue();
    }
}
