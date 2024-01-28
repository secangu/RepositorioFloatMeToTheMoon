using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

namespace FloatMeToTheMoon
{
    public class AudioController : MonoBehaviour
    {
        [SerializeField] AudioMixer _audioMixer;
        [SerializeField] Slider _masterSlider;
        [SerializeField] Slider _melodySlider;
        [SerializeField] Slider effectsSlider;

        float _masterVolume;
        float _melodyVolume;
        float _effectsVolume;

        [SerializeField] AudioSource[] allAudioSources; // Una matriz que contiene todos tus AudioSource
        [SerializeField] AudioSource soundPause; // Una matriz que contiene los sonidos que deseas reproducir
        [SerializeField] bool boolSoundPause; // Una matriz que contiene los sonidos que deseas reproducir
        AudioSource[] pausedSounds;

        private void Start()
        {
            if (PlayerPrefs.HasKey("MasterVolume") == false) PlayerPrefs.SetFloat("MasterVolume", 1.5f);
            if (PlayerPrefs.HasKey("MelodyVolume") == false) PlayerPrefs.SetFloat("MelodyVolume", 1.5f);
            if (PlayerPrefs.HasKey("EffectsVolume") == false) PlayerPrefs.SetFloat("EffectsVolume", 5f);

            // Cargar los valores guardados
            _masterVolume = PlayerPrefs.GetFloat("MasterVolume");
            _melodyVolume = PlayerPrefs.GetFloat("MelodyVolume");
            _effectsVolume = PlayerPrefs.GetFloat("EffectsVolume");

            // Configurar los sliders con los valores
            _masterSlider.value = _masterVolume;
            _melodySlider.value = _melodyVolume;
            effectsSlider.value = _effectsVolume;

            ChangeMasterVolume(_masterVolume);
            ChangeMelodyVolume(_melodyVolume);
            ChangeEffectsVolume(_effectsVolume);
        }

        // Función para reproducir solo los sonidos especificados
        public void PlaySelectedSounds()
        {
            allAudioSources = FindObjectsOfType<AudioSource>();

            // Pausar todos los sonidos primero y guardar los AudioSource pausados.
            pausedSounds = new AudioSource[allAudioSources.Length];
            for (int i = 0; i < allAudioSources.Length; i++)
            {
                if (allAudioSources[i].isPlaying)
                {
                    pausedSounds[i] = allAudioSources[i];
                    allAudioSources[i].Pause();
                }
            }
            if (!boolSoundPause)
            {
                soundPause.Play();
                boolSoundPause = true;
            }
            else soundPause.UnPause();
        }
        public void StopSounds()
        {
            allAudioSources = FindObjectsOfType<AudioSource>();

            // Pausar todos los sonidos primero y guardar los AudioSource pausados.
            pausedSounds = new AudioSource[allAudioSources.Length];
            for (int i = 0; i < allAudioSources.Length; i++)
            {
                if (allAudioSources[i].isPlaying)
                {
                    allAudioSources[i].Pause();
                }
            }
        }

        // Función para reanudar los sonidos pausados
        public void ResumePausedSounds()
        {
            if (pausedSounds != null)
            {
                foreach (var audioSource in pausedSounds)
                {
                    if (audioSource != null)
                    {
                        audioSource.UnPause();
                    }
                }
                soundPause.Pause();
            }
        }
        public void ChangeMasterVolume(float volume)
        {
            _audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);

            PlayerPrefs.SetFloat("MasterVolume", volume);
        }
        public void ChangeMelodyVolume(float volume)
        {
            _audioMixer.SetFloat("MelodyVolume", Mathf.Log10(volume) * 20);

            PlayerPrefs.SetFloat("MelodyVolume", volume);
        }
        public void ChangeEffectsVolume(float volume)
        {
            _audioMixer.SetFloat("EffectsVolume", Mathf.Log10(volume) * 20);

            PlayerPrefs.SetFloat("EffectsVolume", volume);
        }
    }
}
