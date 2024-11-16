using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioMixer audioMixer;

    [Header("Sliders")]
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider effectsSlider;

    void Start()
    {
        // Загрузка сохраненных уровней громкости
        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0.2f);
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.2f);
        effectsSlider.value = PlayerPrefs.GetFloat("EffectsVolume", 0.2f);

        // Применение загруженных настроек
        SetMasterVolume(masterSlider.value);
        SetMusicVolume(musicSlider.value);
        SetEffectsVolume(effectsSlider.value);
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetEffectsVolume(float volume)
    {
        audioMixer.SetFloat("EffectsVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("EffectsVolume", volume);
    }

    void OnDisable()
    {
        // Сохранение настроек при завершении
        PlayerPrefs.Save();
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

}

