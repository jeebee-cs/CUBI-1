using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using UnityEngine.Rendering.Universal;

public class VideoSetting : MonoBehaviour
{
  [SerializeField] TMP_Dropdown _resolutionDropdown;
  [SerializeField] TMP_Dropdown _fullScreenDropdown;
  [SerializeField] TMP_Dropdown _frameRateCapDropdown;
  [SerializeField] Slider _brightnessSlider;
  Resolution[] _resolutions;
  List<Resolution> _filteredResolutions = new List<Resolution>();
  float _currentRefreshRate;
  int _currentResolutionIndex = 0;
  bool _loaded = false;

  public void Start()
  {
    _resolutions = Screen.resolutions;

    _resolutionDropdown.ClearOptions();
    _currentRefreshRate = Screen.currentResolution.refreshRate;

    for (int i = 0; i < _resolutions.Length; i++)
    {
      Resolution resolution = _resolutions[i];
      if (UsefulFunctions.IsBetween(_currentRefreshRate, resolution.refreshRate - 1, resolution.refreshRate + 1)) _filteredResolutions.Add(resolution);
    }

    List<string> options = new List<string>();
    for (int i = 0; i < _filteredResolutions.Count; i++)
    {
      Resolution filtredResolution = _filteredResolutions[i];
      string resolutionOption = filtredResolution.width + "x" + filtredResolution.height + " " + filtredResolution.refreshRate;
      options.Add(resolutionOption);
      if (filtredResolution.width == Screen.width && filtredResolution.height == Screen.height) _currentResolutionIndex = i;
    }

    _resolutionDropdown.AddOptions(options);
    _resolutionDropdown.value = _currentResolutionIndex;
    _resolutionDropdown.RefreshShownValue();

    Load();
  }

  void Load()
  {
    _loaded = true;

    int resolutions = PlayerPrefs.GetInt("resolutions", _currentResolutionIndex);
    int fullScreen = PlayerPrefs.GetInt("fullScreen", _fullScreenDropdown.value);
    int frameRateCap = PlayerPrefs.GetInt("frameRateCap", _frameRateCapDropdown.value);
    float brightness = PlayerPrefs.GetFloat("brightness", _brightnessSlider.value);

    SetResolution(resolutions);
    SetFullScreen(fullScreen);
    SetFrameRateCap(frameRateCap);
  }

  public void SetResolution(int resolutionIndex)
  {
    if (!_loaded) return;

    resolutionIndex = Mathf.Clamp(resolutionIndex, 0, _filteredResolutions.Count - 1);

    Resolution resolution = _filteredResolutions[resolutionIndex];
    Screen.SetResolution(resolution.width, resolution.height, true);

    _resolutionDropdown.value = resolutionIndex;
    _resolutionDropdown.RefreshShownValue();

    PlayerPrefs.SetInt("resolutions", resolutionIndex);
  }
  public void SetFullScreen(int fullScreenIndex)
  {
    if (!_loaded) return;
    Screen.fullScreen = fullScreenIndex == 0;

    _fullScreenDropdown.value = fullScreenIndex;
    _fullScreenDropdown.RefreshShownValue();

    PlayerPrefs.SetInt("fullScreen", fullScreenIndex);
  }
  public void SetFrameRateCap(int frameRateCapIndex)
  {
    if (!_loaded) return;
    if (frameRateCapIndex == 0)
    {
      QualitySettings.vSyncCount = 1;
      Application.targetFrameRate = -1;
    }
    else if (frameRateCapIndex == _frameRateCapDropdown.options.Count - 1)
    {
      QualitySettings.vSyncCount = 0;
      Application.targetFrameRate = -1;
    }
    else
    {
      int frameRateCap = int.Parse(Regex.Match(_frameRateCapDropdown.options[frameRateCapIndex].text, @"\d+").Value);
      QualitySettings.vSyncCount = 0;
      Application.targetFrameRate = frameRateCap;
    }

    _frameRateCapDropdown.value = frameRateCapIndex;
    _frameRateCapDropdown.RefreshShownValue();

    PlayerPrefs.SetInt("frameRateCap", frameRateCapIndex);
  }
}
