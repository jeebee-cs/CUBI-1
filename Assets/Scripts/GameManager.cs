using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    [SerializeField] UIManager _uIManager;
    public UIManager uIManager { get => _uIManager; }
    [SerializeField] CameraManager _cameraManager;
    public CameraManager cameraManager { get => _cameraManager; }
    [SerializeField] PlayerMovements _playerMovement;
    public PlayerMovements playerMovement { get => _playerMovement; }
    [SerializeField] DreamCollection _dreamCollection;
    public DreamCollection dreamCollection { get => _dreamCollection; }
    [SerializeField] WinLoose _winLoose;
    public WinLoose winLoose { get => _winLoose; }
    static GameManager _instance;
    public static GameManager instance { get => _instance; }
    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            _instance.OnSceneLoaded(_uIManager, _cameraManager, _dreamCollection, _winLoose);
            Destroy(gameObject);
            return;
        }
        else
        {
            DontDestroyOnLoad(this);
            _instance = this;
        }
        Time.timeScale = 1;
    }
    public void OnSceneLoaded(UIManager uIManager, CameraManager cameraManager, DreamCollection dreamCollection, WinLoose winLoose)
    {
        _uIManager = uIManager;
        _cameraManager = cameraManager;
        _dreamCollection = dreamCollection;
        _winLoose = winLoose;
    }
}
