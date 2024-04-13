using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using JetBrains.Annotations;

public static class SaveSystem
{
    public static void init()
    {
        string dir = Application.persistentDataPath + "/SaveData/";

        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

    }

    public static void Save(string saveString)
    {
        File.WriteAllText(Application.persistentDataPath + "/SaveData/" + "GameData.txt", saveString);
    }

    public static string Load()
    {
        string fullPath = Application.persistentDataPath + "/SaveData/" + "GameData.txt";
        if (File.Exists(fullPath))
        {
            string stringSave = File.ReadAllText(fullPath);
            return stringSave;
        }
        else
        {
            return null;
        }
    }
}