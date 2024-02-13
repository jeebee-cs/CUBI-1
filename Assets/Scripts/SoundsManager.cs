using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManager : MonoBehaviour
{
  [Range(0, 1)][SerializeField] float _general;
  public float general { get => _general; set => OnGeneralChange(value); }
  [Range(0, 1)][SerializeField] float _sound;
  public float sound { get => _sound; set => OnSoundChange(value); }
  [Range(0, 1)][SerializeField] float _music;
  public float music { get => _music; set => OnMusicChange(value); }
  [Range(0, 1)][SerializeField] float _ambient;
  public float ambient { get => _ambient; set => OnAmbientChange(value); }

  public event Action<float> onSoundChange;
  public event Action<float> onMusicChange;
  public event Action<float> onAmbientChange;

  void OnGeneralChange(float value)
  {
    _general = value;

    OnSoundChange(_sound);
    OnMusicChange(_music);
    OnAmbientChange(_ambient);
  }
  void OnSoundChange(float value)
  {
    _sound = value;
    onSoundChange?.Invoke(value * _general);
  }
  void OnMusicChange(float value)
  {
    _music = value;
    onMusicChange?.Invoke(value * _general);
  }
  void OnAmbientChange(float value)
  {
    _ambient = value;
    onAmbientChange?.Invoke(value * _general);
  }
}
