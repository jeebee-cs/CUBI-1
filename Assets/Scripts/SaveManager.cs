using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    
    public static string directory = "/SaveData/";
    public static string fileName = "GameData.txt";
    public GameObject playerObject;

    void Awake()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        SaveSystem.init();
    }


    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("trying to save...");
            Save();
            Debug.Log("Saved!");
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("Loading...");
            Load();
            Debug.Log("Loaded");
        }
    }

    public void saveCall(string option)
    {
        if (option == "Save")
        {
            Save();
        }
        if (option == "Load")
        {
            Load();
        }
    }


    private void Save()
    {
        GameManager gameManager = GameManager.instance;

        string dir = Application.persistentDataPath + directory;
        Debug.Log(dir);
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        
        //gameManager.winLoose.voxelMaps
        SaveCrystallised saveObject = new SaveCrystallised();

        string json = JsonUtility.ToJson(saveObject, true);
        SaveSystem.Save(json);
    }

    private void Load()
    {
        GameManager gameManager = GameManager.instance;
        string saveString = SaveSystem.Load();
        if (saveString != null)
        {
            SaveCrystallised saveObject = JsonUtility.FromJson<SaveCrystallised>(saveString);
        }
    }


    private class SaveCrystallised
    {
        
    }

}
