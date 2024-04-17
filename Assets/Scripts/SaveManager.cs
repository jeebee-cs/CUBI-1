using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveManager : MonoBehaviour
{

    static string directory = "/SaveData/";
    static string fileName = "GameData.dream";

    public void Save()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        GameManager gameManager = GameManager.instance;

        string path = Application.persistentDataPath + directory;

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        path += fileName;

        FileStream stream = new FileStream(path, FileMode.Create);

        SaveCrystallisedAll data = new SaveCrystallisedAll(gameManager.winLoose.voxelMaps);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public void Load()
    {
        GameManager gameManager = GameManager.instance;

        string path = Application.persistentDataPath + directory + fileName;

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveCrystallisedAll data = formatter.Deserialize(stream) as SaveCrystallisedAll;

            for (int i = 0; i < gameManager.winLoose.voxelMaps.Length; i++)
            {
                Vector3 pos = new Vector3(data.saveCrystalliseds[i].firstPosBlockX, data.saveCrystalliseds[i].firstPosBlockY, data.saveCrystalliseds[i].firstPosBlockZ);

                gameManager.winLoose.voxelMaps[i].firstPosBlock = pos;

                gameManager.winLoose.voxelMaps[i].Load();
            }

            stream.Close();
        }
    }

    [System.Serializable]
    public class SaveCrystallisedAll
    {
        SaveCrystallised[] _saveCrystalliseds;
        public SaveCrystallised[] saveCrystalliseds { get => _saveCrystalliseds;}
        public SaveCrystallisedAll(voxelMap[] voxelMaps)
        {
            _saveCrystalliseds = new SaveCrystallised[voxelMaps.Length];
            
            for (int i = 0; i < _saveCrystalliseds.Length; i++)
            {
                _saveCrystalliseds[i] = new SaveCrystallised(voxelMaps[i].firstBlockOriginalPosThisGame);
            }
        }

    }

    [System.Serializable]
    public class SaveCrystallised
    {
        float _firstPosBlockX;
        public float firstPosBlockX { get => _firstPosBlockX; }
        float _firstPosBlockY;
        public float firstPosBlockY { get => _firstPosBlockY; }
        float _firstPosBlockZ;
        public float firstPosBlockZ { get => _firstPosBlockZ; }
    
        public SaveCrystallised(Vector3 firstPosBlock)
        {
            _firstPosBlockX = firstPosBlock.x;
            _firstPosBlockY = firstPosBlock.y;
            _firstPosBlockZ = firstPosBlock.z;
        }

    }

}
