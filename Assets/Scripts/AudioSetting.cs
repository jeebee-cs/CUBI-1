using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSetting : MonoBehaviour
{
  [SerializeField] Slider _generalSlider;
  [SerializeField] Slider _soundSlider;
  [SerializeField] Slider _musicSlider;
  [SerializeField] Slider _ambientSlider;
  bool _loaded = false;

  public void Start()
  {
    Load();
  }
  public void Load()
  {
    _loaded = true;

    float general = PlayerPrefs.GetFloat("general", _generalSlider.value);
    float sound = PlayerPrefs.GetFloat("sound", _soundSlider.value);
    float music = PlayerPrefs.GetFloat("music", _musicSlider.value);
    float ambient = PlayerPrefs.GetFloat("ambient", _ambientSlider.value);

    SetGeneral(general);
    SetSound(sound);
    SetMusic(music);
    SetAmbient(ambient);
  }

  public void SetGeneral(float generalValue)
  {
    if (!_loaded) return;

    _generalSlider.value = generalValue;
    // Change soundManager general sound
    // soundManager.general = generalValue;

    PlayerPrefs.SetFloat("general", generalValue);
  }
  public void SetSound(float soundValue)
  {
    if (!_loaded) return;

    _soundSlider.value = soundValue;
    // Change soundManager sound sound
    // soundManager.sound = soundValue;

    PlayerPrefs.SetFloat("sound", soundValue);
  }
  public void SetMusic(float musicValue)
  {
    if (!_loaded) return;

    _musicSlider.value = musicValue;
    // Change soundManager music sound
    // soundManager.music = musicValue;

    PlayerPrefs.SetFloat("music", musicValue);
  }
  public void SetAmbient(float ambientValue)
  {
    if (!_loaded) return;

    _ambientSlider.value = ambientValue;
    // Change soundManager ambient sound
    // soundManager.ambient = ambientValue;

    PlayerPrefs.SetFloat("ambient", ambientValue);
  }
}
