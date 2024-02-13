using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] AudioSetting _audioSetting;
    [SerializeField] VideoSetting _videoSetting;
    [SerializeField] Menu _settingsMenu;
    public Menu settingsMenu { get => _settingsMenu; }
    [SerializeField] Menu _audioMenu, _videoMenu;

    void Start()
    {
        _videoSetting.Start();
        _audioSetting.Start();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SettingsToggle();
        }
    }

    public void SettingsToggle()
    {
        _settingsMenu.Toggle(!_settingsMenu.isShowed);

        if (!_settingsMenu.isShowed)
        {
            _audioMenu.Toggle(false);
            _videoMenu.Toggle(false);
        }

        if (_settingsMenu.isShowed)
        {
            //Disable player control;

            Time.timeScale = 0;
        }
        else
        {
            //Enable player control;

            Time.timeScale = 1;
        }
    }

    public void AudioToggle()
    {
        _audioMenu.Toggle(!_audioMenu.isShowed);
    }
    public void VideoToggle()
    {
        _videoMenu.Toggle(!_videoMenu.isShowed);
    }

    public void QuitGame() => Application.Quit();
}
