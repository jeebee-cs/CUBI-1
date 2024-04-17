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
    [SerializeField] private UnityEvent winning;
    [SerializeField] float winningScore = 1;
    [SerializeField] voxelMap[] _voxelMap;
    Coroutine resetGameCoroutine = null;
    private bool _gameFinished = false;
    [SerializeField] GameObject staticBlock;
    NetworkObject _networkBlock;
    public bool gameOver { get => _gameFinished; set => _gameFinished = value; }

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

    /*public void GameWin()
    {
        Debug.Log("You win");
         _gameFinished = true;
         Reset();
    }*/

    public void Reload(int reload)
    {
        if (GameManager.instance.neutralDreamCollected - reload >= 0)
        {
            GameManager.instance.saveManager.saveCall("respawn");
        }
        else
        {
            Lose();
        }
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
    public void firstBlockChange(MoveableBlock block, Vector3 firstPosition)
    {
        if (block.voxelMap.firstBlockPosThisGame == new Vector3(int.MaxValue, int.MaxValue, int.MaxValue))
        {
            Debug.Log(block.transform.position + "   " + firstPosition);
            firstBlockChangeServerRpc(block.transform.position, firstPosition);
            block.voxelMap.firstBlockPosThisGame = block.transform.position;
            block.voxelMap.firstBlockOriginalPosThisGame = firstPosition;
            block.DespawnServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void firstBlockChangeServerRpc(Vector3 position, Vector3 firstPosition)
    {
        GameObject gameObjectCrystal = Instantiate(staticBlock, position, Quaternion.identity);
        NetworkObject gameObjectCrystalNetworkObject = gameObjectCrystal.GetComponent<NetworkObject>();
        gameObjectCrystalNetworkObject.Spawn();
        
        //firstBlockChangeClientRpc(position, firstPosition);
    }
    // [ClientRpc]
    // public void firstBlockChangeClientRpc(ABlock block, Vector2 firstPosition)
    // {
    //     block.voxelMap.firstBlockPosThisGame = block.transform.position;
    //     block.voxelMap.firstBlockOriginalPosThisGame = firstPosition;
    // }

}