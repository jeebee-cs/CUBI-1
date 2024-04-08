using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] UIManager _uIManager;
    public UIManager uIManager { get => _uIManager; }
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
    public void OnSceneLoaded(UIManager uIManager)
    {
        _uIManager = uIManager;
        _dreamCollection = new DreamCollection();
        _winLoose = new WinLoose();
    }
}
