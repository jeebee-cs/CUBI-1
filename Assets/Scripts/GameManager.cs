using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{

    [SerializeField] UIManager _uIManager;
    public UIManager uIManager { get => _uIManager; }
    NetworkVariable<float> _dreamEnergy = new NetworkVariable<float>();
    public float dreamEnergy { get => _dreamEnergy.Value; }
    NetworkVariable<int> _neutralDreamCollected = new NetworkVariable<int>();
    public int neutralDreamCollected { get => _neutralDreamCollected.Value; }
    [SerializeField] CameraManager _cameraManager;
    public CameraManager cameraManager { get => _cameraManager; }
    [SerializeField] List<PlayerMovements> _playerMovements = new List<PlayerMovements>();
    public List<PlayerMovements> playerMovements { get => _playerMovements; }
    [SerializeField] DreamCollection _dreamCollection;
    public DreamCollection dreamCollection { get => _dreamCollection; }
    [SerializeField] WinLoose _winLoose;
    public WinLoose winLoose { get => _winLoose; }
    [SerializeField] SaveManager _saveManager;
    public SaveManager saveManager { get => _saveManager; }
    static GameManager _instance;
    public static GameManager instance { get => _instance; }
    [SerializeField] DreamDisplayer _dreamDisplayer;
    public DreamDisplayer dreamDisplayer { get => _dreamDisplayer; }
    [SerializeField] SkyboxBlender _skyboxBlender;
    public SkyboxBlender skyboxBlender { get => _skyboxBlender; }


    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            _instance.OnSceneLoaded(_uIManager, _cameraManager, _dreamCollection, _winLoose, _saveManager, _dreamDisplayer, _skyboxBlender);
            Destroy(gameObject);
            return;
        }
        else
        {
            DontDestroyOnLoad(this);
            _instance = this;
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
        }
        Time.timeScale = 1;
    }
    public void OnSceneLoaded(UIManager uIManager, CameraManager cameraManager, DreamCollection dreamCollection, WinLoose winLoose, SaveManager saveManager, DreamDisplayer dreamDisplayer, SkyboxBlender skyboxBlender)
    {
        _uIManager = uIManager;
        _cameraManager = cameraManager;
        _dreamCollection = dreamCollection;
        _winLoose = winLoose;
        _dreamDisplayer = dreamDisplayer;
        _skyboxBlender = skyboxBlender;
        _saveManager = saveManager;
    }
    void OnClientConnectedCallback(ulong id)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        _playerMovements = new List<PlayerMovements>();
        for (int i = 0; i < players.Length; i++)
        {
            _playerMovements.Add(players[i].GetComponent<PlayerMovements>());
        }
        _uIManager.neutralDreams.text = _neutralDreamCollected.Value.ToString();
        _uIManager.dreamBar.value = _dreamEnergy.Value;
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetDreamEnergyServerRpc(float dreamEnergy)
    {
        Debug.Log(_dreamEnergy.Value);
        _dreamEnergy.Value = dreamEnergy;
        _uIManager.dreamBar.value = _dreamEnergy.Value;
        SetDreamEnergyClientRpc(dreamEnergy);
        _winLoose.winCheck(dreamEnergy, _neutralDreamCollected.Value);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetNeutralDreamCollectedServerRpc(int neutralDreamCollected)
    {
        Debug.Log(_neutralDreamCollected.Value);
        _neutralDreamCollected.Value = neutralDreamCollected;
        _uIManager.neutralDreams.text = _neutralDreamCollected.Value.ToString();
        SetNeutralDreamCollectedClientRpc(neutralDreamCollected);
        _winLoose.winCheck(_dreamEnergy.Value, neutralDreamCollected);
    }
    [ClientRpc]
    public void SetDreamEnergyClientRpc(float dreamEnergy)
    {
        Debug.Log(dreamEnergy);
        _uIManager.dreamBar.value = dreamEnergy;
        _winLoose.winCheck(dreamEnergy, _neutralDreamCollected.Value);
    }

    [ClientRpc]
    public void SetNeutralDreamCollectedClientRpc(int neutralDreamCollected)
    {
        Debug.Log(_neutralDreamCollected.Value);
        _uIManager.neutralDreams.text = neutralDreamCollected.ToString();
        _winLoose.winCheck(_dreamEnergy.Value, neutralDreamCollected);
    }
}
