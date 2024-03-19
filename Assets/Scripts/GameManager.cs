using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] CameraManager _cameraManager;
    public CameraManager cameraManager { get => _cameraManager; }
    [SerializeField] UIManager _uIManager;
    public UIManager uIManager { get => _uIManager; }
    [SerializeField] PlayerMovements _playerMovement;
    public PlayerMovements playerMovement { get => _playerMovement; }
    [SerializeField] DreamCollection _neutralDreamCollection;
    public DreamCollection neutralDreamCollection { get => _neutralDreamCollection; }

    static GameManager _instance;
    public static GameManager instance { get => _instance; }
    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            _instance.OnSceneLoaded(_cameraManager, _uIManager);
            Destroy(gameObject);
            return;
        }
        else
        {
            DontDestroyOnLoad(this);
            _instance = this;
            OnSceneLoaded(_cameraManager, _uIManager);
        }
        Time.timeScale = 1;
    }
    public void OnSceneLoaded(CameraManager cameraManager, UIManager uIManager)
    {
        _cameraManager = cameraManager;
        _uIManager = uIManager;
        _neutralDreamCollection = new DreamCollection();
    }
}
