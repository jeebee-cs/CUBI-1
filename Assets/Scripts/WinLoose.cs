using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class WinLoose : NetworkBehaviour
{
    [SerializeField] float winningScore = 1;
    [SerializeField] voxelMap[] _voxelMaps;
    public voxelMap[] voxelMaps { get => _voxelMaps; }
    Coroutine resetGameCoroutine = null;
    private bool _gameFinished = false;
    [SerializeField] GameObject _staticBlock;
    public GameObject staticBlock { get => _staticBlock; }

    private void Start()
    {
        AkSoundEngine.PostEvent("Music_Start", this.gameObject);
    }

    public void winCheck(float dreamEnergy, int neutralDreamCollected)
    {
        if (dreamEnergy >= winningScore)
        {
            LevelWin();
        }
        else if (dreamEnergy < 0 || neutralDreamCollected < 0)
        {
            Lose();
        }
    }

    public void Lose()
    {
        AkSoundEngine.PostEvent("Stop_All_Audio", this.gameObject);
        Debug.Log("You lose");
        _gameFinished = true;
        ResetGame("Lose");
    }

    public void LevelWin()
    {
        AkSoundEngine.PostEvent("Stop_All_Audio", this.gameObject);
        Debug.Log("You win");
        _gameFinished = true;
        ResetGame("Win");
    }

    public void ResetGame(string scene)
    {
        if (resetGameCoroutine == null) resetGameCoroutine = StartCoroutine(ResetGameCoroutine(scene));
    }
    public IEnumerator ResetGameCoroutine(string scene)
    {
        if (_gameFinished)
        {
            Time.timeScale = 1;
            NetworkManager.Singleton.Shutdown();
            while (NetworkManager.Singleton.ShutdownInProgress)
            {
                yield return null;
            }
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Destroy(NetworkManager.Singleton.gameObject);
            Destroy(GameManager.instance.gameObject);
            AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync(scene);
            GameManager.instance.saveManager.Save();
            while (!asyncLoadLevel.isDone)
            {
                yield return null;
            }
        }

        else
        {
            GameManager.instance.SetDreamEnergyServerRpc(0);
        }
    }
    public void FirstBlockChange(MoveableBlock block, Vector3 firstPosition)
    {
        int voxelMapID = -1;
        for (int i = 0; i < _voxelMaps.Length; i++)
        {
            if (block.voxelMap == _voxelMaps[i])
            {
                voxelMapID = i;
            }
        }

        if (_voxelMaps[voxelMapID].firstBlockPosThisGame == new Vector3(int.MaxValue, int.MaxValue, int.MaxValue))
        {
            FirstBlockChangeServerRpc(block.transform.position, firstPosition, voxelMapID, block.networkObject);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void FirstBlockChangeServerRpc(Vector3 position, Vector3 firstPosition, int voxelMapID, NetworkObjectReference block)
    {
        if (_voxelMaps[voxelMapID].firstBlockPosThisGame != new Vector3(int.MaxValue, int.MaxValue, int.MaxValue)) return;

        GameObject gameObjectCrystal = Instantiate(staticBlock, position, Quaternion.identity);
        NetworkObject gameObjectCrystalNetworkObject = gameObjectCrystal.GetComponent<NetworkObject>();
        gameObjectCrystalNetworkObject.Spawn();

        _voxelMaps[voxelMapID].firstBlockPosThisGame = position;
        _voxelMaps[voxelMapID].firstBlockOriginalPosThisGame = firstPosition;

        block.TryGet(out NetworkObject blockObject);
        blockObject.Despawn();
    }
}