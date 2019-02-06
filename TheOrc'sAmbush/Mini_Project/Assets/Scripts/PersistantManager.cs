using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;

public class PersistantManager : MonoBehaviour
{
    public static PersistantManager PM;

    public uint currentScore = 0;
    public uint highScore = 0;


    private void Awake()
    {
        if(PM==null)
        {
            PM = this;
        }
        else
        {
            if(PM!=this)
            {
                Destroy(this.gameObject);
            }
        }
        DontDestroyOnLoad(this.gameObject);
        
    }

    [Serializable]
    class GameData
    {
        public uint F_highScore;
    }
    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/Gamedata.dat");

        GameData data = new GameData();
        data.F_highScore = highScore;

        bf.Serialize(file, data);

        file.Close();

        Debug.Log("Data saved");
                
    }

    public void Load ()
    {
        if(File.Exists(Application.persistentDataPath+"/Gamedata.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/Gamedata.dat", FileMode.Open);

            GameData data = (GameData)bf.Deserialize(file);
            file.Close();
            highScore = data.F_highScore;
            //Debug.Log("Data Loaded");
        }
        else
        {
            Debug.Log("File couldnt be loaded");
        }
    }

    public void CheckHighScore()
    {
        if(currentScore>highScore)
        {
            highScore = currentScore;
            Save();
            Load();
        }
    }

    


}
